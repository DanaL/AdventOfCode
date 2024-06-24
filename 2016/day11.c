#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <string.h>
#include <stdbool.h>

#define FNV_OFFSET 14695981039346656037UL
#define FNV_PRIME 1099511628211UL

#define VT_SIZE 101719

#define HT_SIZE 1000
#define PARENT(i) ((i) - 1) / 2

static size_t config_len = 0;

struct state {
  uint32_t move_count;
  uint8_t *config;
};

struct heap {
  struct state **table;
  size_t table_size;
  size_t num_of_elts;
};

struct heap *heap_new(void)
{
  struct heap *h = malloc(sizeof(struct heap));
  h->num_of_elts = 0;
  h->table_size = HT_SIZE;
  h->table = calloc(HT_SIZE, sizeof(struct state*));

  return h;
}

void heap_free(struct heap *h)
{
  for (size_t i = 0; i < h->num_of_elts; i++)
    free(h->table[i]);
  free(h);
}

void state_print(const struct state *s)
{
  for (size_t i = 0; i < config_len; i++) {
    printf("%zu ", s->config[i]);
  }
  printf(" | mc: %zu\n", s->move_count);
}

// Calc the distance from our goal state (ie., having all chips and generators
// on floor four). Used for the heap ordering to make a priority queue
uint32_t dist_from_goal(const struct state *s) 
{
  uint32_t distance = 0;

  for (int j = 0; j < config_len; j++)
    distance += (4 - s->config[j]) * 2;

  return distance;
}

void min_heap_push(struct heap *h, struct state *s)
{
  if (h->num_of_elts == h->table_size) {
    // need to expand the table
    h->table_size += HT_SIZE;
    h->table = realloc(h->table, h->table_size * sizeof(struct state*));
  }

  size_t i = h->num_of_elts;
  h->table[i] = s;
  uint32_t dist = dist_from_goal(s);

  while (i > 0 && dist < dist_from_goal(h->table[PARENT(i)])) {
    size_t parent = PARENT(i);
    struct state *tmp = h->table[parent];
    h->table[parent] = s;
    h->table[i] = tmp;
    i = parent;
  }

  ++h->num_of_elts;
}

void min_heapify(struct heap *h, size_t i)
{
  uint32_t left_child_dist = UINT32_MAX;
  size_t left_child_i = 2 * i + 1;
  if (left_child_i < h->num_of_elts) {
    left_child_dist = dist_from_goal(h->table[left_child_i]);
  }
  uint32_t right_child_dist = UINT32_MAX;
  size_t right_child_i = 2 * i + 2;
  if (right_child_i < h->num_of_elts) {
    right_child_dist = dist_from_goal(h->table[right_child_i]);
  }

  uint32_t dist = dist_from_goal(h->table[i]);
  if (dist > left_child_dist || dist > right_child_dist) {
    if (left_child_dist < right_child_dist) {
      // swap i and left and reheapify left branch
      struct state *tmp = h->table[left_child_i];
      h->table[left_child_i] = h->table[i];
      h->table[i] = tmp;
      min_heapify(h, left_child_i);
    }
    else {
      // swap i and right and reheapify right branch
      struct state *tmp = h->table[right_child_i];
      h->table[right_child_i] = h->table[i];
      h->table[i] = tmp;
      min_heapify(h, right_child_i);
    }
  }
}

struct state *min_heap_pop(struct heap *h)
{
  struct state *result = h->table[0];
  --h->num_of_elts;

  h->table[0] = h->table[h->num_of_elts];
  h->table[h->num_of_elts] = NULL;

  if (h->num_of_elts > 0)
    min_heapify(h, 0);

  return result;
}

struct vt_entry {
  char *key;
  uint32_t move_count;
  struct vt_entry *next;
};

// FNV-1a from wikipedia
uint64_t calc_hash(const char *key)
{
  uint64_t hash = FNV_OFFSET;

  for (const char *c = key; *c; c++) {
    hash ^= (uint64_t) *c;
    hash *= FNV_PRIME;
  }

  return hash % VT_SIZE;
}

struct state *state_copy(struct state *p)
{
  struct state *n = malloc(sizeof(struct state));
  n->move_count = p->move_count;
  n->config = malloc(config_len * sizeof(uint8_t));

  for (int i = 0; i < config_len; i++)
    n->config[i] = p->config[i];

  return n;
}

void state_destroy(struct state *state) 
{
  free(state->config);
  free(state);
}

struct vt_entry **visited_table_create(void)
{
  struct vt_entry **vt = calloc(VT_SIZE, sizeof(struct vt_entry *));
  if (!vt) {
    printf("Unable to allocate memory for visited table.\n");
    exit(-1);
  }

  return vt;
}

void visited_table_destroy(struct vt_entry **vt) {
  for (int i = 0; i < VT_SIZE; i++) {
    struct vt_entry *e = vt[i];
    while (e) {
      struct vt_entry *n = e->next;
      free(e->key);
      free(e);
      e = n;
    }
  }

  free(vt);
}

void visited_table_insert(struct vt_entry **vt, const char *key, int move_count)
{
  struct vt_entry *entry = malloc(sizeof(struct vt_entry));
  entry->next = NULL;
  entry->move_count = move_count;
  entry->key = malloc(sizeof(char) * (strlen(key) + 1));
  strcpy(entry->key, key);

  uint64_t hash = calc_hash(key);
  
  // Easy case -- the table location is empty
  if (!vt[hash]) {
    vt[hash] = entry;
    return;
  }

  // if the same key exists with higher move count, replace move found 
  // with smaller value
  struct vt_entry *p = vt[hash];
  while (p) {   
    if (strcmp(key, p->key) == 0) {
      if (move_count < p->move_count) {
        p->move_count = move_count;
      }
      free(entry->key);
      free(entry);

      return;
    }

    p = p->next;
  }
  
  entry->next = vt[hash];
  vt[hash] = entry;
}

struct vt_entry *visited_table_contains(struct vt_entry  **vt, const char *key)
{
  uint64_t hash = calc_hash(key);
  if (!vt[hash])
    return NULL;

  struct vt_entry *e = vt[hash];
  do {
    if (strcmp(key, e->key) == 0)
      return e;
    e = e->next;
  }
  while (e);

  return NULL;
}

uint32_t visited_table_check(struct vt_entry **vt, const char *key) 
{
  uint64_t hash = calc_hash(key);

  if (vt[hash]) {
    struct vt_entry *e = vt[hash];

    do {
      if (strcmp(key, e->key) == 0)
        return e->move_count;
      e = e->next;
    }
    while (e);
  }

  return UINT32_MAX;
}

char *state_to_key(const struct state *state)
{ 
  char *key = calloc(config_len + 1, sizeof(char));

  for (int i = 0; i < config_len; i++) {    
    key[i] = (char) state->config[i] + '0';
  }
  
  return key;
}

bool finished(uint8_t *config)
{
  for (int j = 1; j < config_len; j++) {
    if (config[j] != 4)
      return false;
  }

  return true;
}

bool valid_config(uint8_t *config)
{  
  // check where the elevator is
  if (config[0] < 1 || config[0] > 4) {
    return false;
  }

  // check to make sure no elements are on a floor with a
  // generator when their generator isn't present
  for (int i = 1; i < config_len; i += 2) {
    // if the generator is on the same floor as the element,
    // we're good, otherwise we have to make sure there are
    // no other generators on the same floor.
    uint8_t elt_floor = config[i];
    uint8_t elt_gen_floor = config[i+1];
    if (elt_floor == elt_gen_floor) {
      continue;
    }

    for (int j = 2; j < config_len; j += 2) {
      if (config[j] == elt_floor) {
        return false;
      }
    }
  }

  return true;
}

bool move_already_in_list(struct state **list, int mc, struct state *a) 
{
  for (int m = 0; m < mc; m++) {
    bool match = true;
    for (int j = 0; j < config_len; j++) {
      if (list[m]->config[j] != a->config[j]) {
        match = false;
        break;
      }
    }

    if (match)
      return true;
  }

  return false;
}

struct state **find_valid_moves(int *moves_count, struct state *curr_state)
{
  uint8_t elevator = curr_state->config[0];
  struct state **moves = NULL;
  *moves_count = 0;
  struct state *other;

  for (size_t j = 1; j < config_len; j++) {
    if (curr_state->config[j] == elevator) {      
      // test move pos by itself up
      other = state_copy(curr_state);
      other->move_count += 1;
      other->config[0] += 1;
      other->config[j] += 1;

      if (valid_config(other->config)) {   
        ++(*moves_count);
        moves = realloc(moves, *moves_count * sizeof(struct state*));
        moves[*moves_count - 1] = other;
      }
      else {
        state_destroy(other);
      }

      // test move pos by itself down
      other = state_copy(curr_state);
      other->move_count += 1;
      other->config[0] -= 1;
      other->config[j] -= 1;
      if (valid_config(other->config)) {
        ++(*moves_count);
        moves = realloc(moves, *moves_count * sizeof(struct state*));
        moves[*moves_count - 1] = other;
      }
      else {
        state_destroy(other);
      }

      // test moving pos with each of the other moveable things (up and down)
      for (size_t k = 1; k < config_len; k++) 
      {
        if (j == k)
          continue;

        if (curr_state->config[k] == elevator) {
          other = state_copy(curr_state);
          other->move_count += 1;
          other->config[0] += 1;
          other->config[j] += 1;
          other->config[k] += 1;

          if (move_already_in_list(moves, *moves_count, other)) {
            state_destroy(other);
            continue;
          }

          if (valid_config(other->config)) {   
            ++(*moves_count);
            moves = realloc(moves, *moves_count * sizeof(struct state*));
            moves[*moves_count - 1] = other;
          }
          else {
            state_destroy(other);
          }

          other = state_copy(curr_state);
          other->move_count += 1;
          other->config[0] -= 1;
          other->config[j] -= 1;
          other->config[k] -= 1;

          if (move_already_in_list(moves, *moves_count, other)) {
            state_destroy(other);
            continue;            
          }

          if (valid_config(other->config)) {   
            ++(*moves_count);
            moves = realloc(moves, *moves_count * sizeof(struct state*));
            moves[*moves_count - 1] = other;
          }
          else {
            state_destroy(other);
          }
        }
      }
    }
  }

  return moves;
}

void p1(void) {
  config_len = 11;
  struct vt_entry **vt = visited_table_create();
  uint32_t shortest = UINT32_MAX;

  struct state *initial = malloc(sizeof(struct state));
  initial->move_count = 0;

  initial->config = calloc(config_len, sizeof(uint8_t));
  initial->config[0] = 1; // elevator
  
  // in order of chip then generator so chips are odd indexes
  // and their corresponding generators are even indexes

  initial->config[1] = 1; // promethium
  initial->config[2] = 1;
  initial->config[3] = 3; // cobalt
  initial->config[4] = 2;
  initial->config[5] = 3; // curium
  initial->config[6] = 2;
  initial->config[7] = 3; // ruthenium
  initial->config[8] = 2;
  initial->config[9] = 3; // plutonium
  initial->config[10] = 2;

  struct heap *queue = heap_new();
  min_heap_push(queue, initial);

  uint64_t x = 0;
  while (queue->num_of_elts > 0) {
    struct state *curr = min_heap_pop(queue);
    char *curr_key = state_to_key(curr);

    uint32_t visited_mc = visited_table_check(vt, curr_key);
    free(curr_key);
    if (curr->move_count >= visited_mc || curr->move_count >= shortest) {
      goto iterate;
    }

    if (finished(curr->config)) {
      printf("found one %d %d\n", x, curr->move_count);      
      if (curr->move_count < shortest)
        shortest = curr->move_count;
      goto iterate;
    }

    // add to visited table
    char *key = state_to_key(curr);
    visited_table_insert(vt, key, curr->move_count);
    free(key);
    
    struct state **moves = NULL;
    int moves_count = 0;
    moves = find_valid_moves(&moves_count, curr);

    ++x;
    for (int j = 0; j < moves_count; j++) {
      char *move_key = state_to_key(moves[j]);
      uint32_t visited_moves = visited_table_check(vt, move_key);
      uint32_t mc = moves[j]->move_count;
      uint32_t mv_distance = dist_from_goal(moves[j]);
    
      uint32_t sum = 0;
      for (int k = 0; k < config_len; k++)
        sum += 4 - moves[j]->config[k];

      if (mc + sum < shortest && mc < visited_moves) {      
        min_heap_push(queue, moves[j]);
      }
      else {
        state_destroy(moves[j]);
      }
  
      free(move_key);
    }
iterate:
    state_destroy(curr);
  }

  printf("interations: %d\n", x);

  visited_table_destroy(vt);
  heap_free(queue);

  printf("P1: %d\n", shortest);  
}

int main(void)
{ 
  p1();
} 
