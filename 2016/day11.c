#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <string.h>
#include <stdbool.h>

#define FNV_OFFSET 14695981039346656037UL
#define FNV_PRIME 1099511628211UL

#define VT_SIZE 101719

#define NUM_OF_ELTS 2
#define CONFIG_LEN (2 * NUM_OF_ELTS + 1)

struct state {
  uint32_t move_count;
  uint8_t *config;
  struct state *next;
};

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

  return hash;
}

struct state *state_copy(struct state *p)
{
  struct state *n = malloc(sizeof(struct state));
  n->move_count = p->move_count;
  n->next = NULL;
  n->config = malloc(CONFIG_LEN * sizeof(uint8_t));

  for (int i = 0; i < CONFIG_LEN; i++)
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

  uint64_t hash = calc_hash(key) % VT_SIZE;
  
  // Easy case -- the table location is empty
  if (!vt[hash]) {
    vt[hash] = entry;
    return;
  }

  // if the same key exists with higher move count, replace move found 
  // with smaller value
  struct vt_entry *p = vt[hash];
  while (p) {
    if (strcmp(key, p->key)) {
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
  uint64_t hash = calc_hash(key) % VT_SIZE;
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

void dump_config(uint8_t *config) 
{
  for (int j = 0; j < CONFIG_LEN; j++)
    printf(" %d", config[j]);
  printf("\n");
}

char *state_to_key(const struct state *state)
{ 
  char *key = calloc(CONFIG_LEN, sizeof(char));

  for (int i = 1; i < CONFIG_LEN; i++) {    
    key[i - 1] = (char) state->config[i] + '0';
  }
  
  return key;
}

bool finished(uint8_t *config)
{
  for (int j = 1; j < CONFIG_LEN; j++) {
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
  for (int i = 1; i < CONFIG_LEN; i += 2) {
    // if the generator is on the same floor as the element,
    // we're good, otherwise we have to make sure there are
    // no other generators on the same floor.
    uint8_t elt_floor = config[i];
    uint8_t elt_gen_floor = config[i+1];
    if (elt_floor == elt_gen_floor) {
      continue;
    }

    for (int j = 2; j < CONFIG_LEN; j += 2) {
      if (config[j] == elt_floor) {
        return false;
      }
    }
  }

  return true;
}

// moves should start off as null
struct state **find_valid_moves(struct state **moves, int *moves_count, struct state *curr_state)
{
  *moves_count = 0;
  uint8_t elevator = curr_state->config[0];
  struct state *other_state = malloc(sizeof(struct state));
  other_state->config = malloc(CONFIG_LEN * sizeof(uint8_t));
  other_state->next = NULL;
  moves = NULL;

  // I'm going to loop over each thing on the floor and see what is
  // possible to be moved. Nested loop to check all possible combos?
  for (int i = 1; i < CONFIG_LEN; i++) {
    if (curr_state->config[i] == elevator) {
      other_state->move_count = curr_state->move_count + 1;
      for (int j = 0; j < CONFIG_LEN; j++)
        other_state->config[j] = curr_state->config[j];

      // check up one floor
      other_state->config[0] = elevator + 1;
      other_state->config[i] = elevator + 1;

      if (valid_config(other_state->config)) {
        *moves_count += 1;
        moves = realloc(moves, *moves_count * sizeof(struct state *));
        moves[*moves_count - 1] = state_copy(other_state);
      }
     
      // check down one floor
      other_state->config[0] = elevator - 1;
      other_state->config[i] = elevator - 1;

      if (valid_config(other_state->config)) {
        *moves_count += 1;
        moves = realloc(moves, *moves_count * sizeof(struct state *));
        moves[*moves_count - 1] = state_copy(other_state);
      }
    
      for (int k = i + 1; k < CONFIG_LEN; k++) {
        if (curr_state->config[k] == elevator) {
          other_state->config[0] = elevator + 1;
          other_state->config[i] = elevator + 1;
          other_state->config[k] = elevator + 1;

          if (valid_config(other_state->config)) {
            *moves_count += 1;
            moves = realloc(moves, *moves_count * sizeof(struct state *));
            moves[*moves_count - 1] = state_copy(other_state);
          }

          other_state->config[0] = elevator - 1;
          other_state->config[i] = elevator - 1;
          other_state->config[k] = elevator - 1;

          if (valid_config(other_state->config)) {
            *moves_count += 1;
            moves = realloc(moves, *moves_count * sizeof(struct state *));
            moves[*moves_count - 1] = state_copy(other_state);
          }
        }
      }
    }
  }

  state_destroy(other_state);

  return moves;
}

void p1() {
  struct vt_entry **vt = visited_table_create();
  uint32_t shortest = UINT32_MAX;

  struct state *initial = malloc(sizeof(struct state));
  initial->move_count = 0;
  initial->next = NULL;

  initial->config = calloc(CONFIG_LEN, sizeof(uint8_t));
  initial->config[0] = 1; // elevator
  // in order of chip then generator so chips are ood indexes
  // and their corresponding generators are even indexes

  // initial->config[1] = 1; // promethium
  // initial->config[2] = 1;
  // initial->config[3] = 3; // cobalt
  // initial->config[4] = 2;
  // initial->config[5] = 3; // curium
  // initial->config[6] = 2;
  // initial->config[7] = 3; // ruthenium
  // initial->config[8] = 2;
  // initial->config[9] = 3; // plutonium
  // initial->config[10] = 2;

  initial->config[1] = 1;
  initial->config[2] = 2;
  initial->config[3] = 1;
  initial->config[4] = 3;

  struct state *to_test_q = initial;

  int x = 0;
  while (to_test_q) {
    struct state *curr = to_test_q;
    //printf("Testing: ");
    //dump_config(curr->config);

    if (finished(curr->config)) {
      if (curr->move_count < shortest)
        shortest = curr->move_count;
      to_test_q = to_test_q->next;
      state_destroy(curr);
      continue;
    }

    // add to visited table
    char *key = state_to_key(curr);
    visited_table_insert(vt, key, curr->move_count);
    free(key);

    struct state **moves = NULL;
    int moves_count = 0;
    moves = find_valid_moves(moves, &moves_count, to_test_q);

    // Maybe I need to structure visited table as key -> moves, and skip
    // visiting a state with identical config but more moves
    for (int j = 0; j < moves_count; j++) {
      char *move_key = state_to_key(moves[j]);
      struct vt_entry *entry = visited_table_contains(vt, move_key);
      uint32_t mc = moves[j]->move_count;
      if (mc < shortest && (!entry || mc < entry->move_count)) {
        //if (entry)
        //  entry->move_count = mc;
        moves[j]->next = to_test_q->next;
        to_test_q->next = moves[j];
      }
      else {
        state_destroy(moves[j]);
      }

      free(move_key);
    }

    to_test_q = to_test_q->next;
    state_destroy(curr);

    if (++x > 500000)
      break;
  }

  printf("interations: %d\n", x);

  // need to free any remaining items in to_test_q
  x = 0;
  struct state *t = to_test_q;
  while (t) {
    ++x;
    printf("T %d", t->config[0]);
    dump_config(t->config);
    t = t->next;    
  }
  printf("Leftover: %d\n", x);

  visited_table_destroy(vt);

  printf("P1: %d\n", shortest);  
}

int main(void)
{ 
  p1();
}
