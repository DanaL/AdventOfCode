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

uint32_t lowest_moves = UINT32_MAX;

struct state {
  uint32_t moves;
  uint8_t *config;
  struct state *next;
};

struct vt_entry {
  char *key;
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
  n->moves = p->moves;
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

void visited_table_insert(struct vt_entry **vt, const char *key)
{
  struct vt_entry *entry = malloc(sizeof(struct vt_entry));
  entry->next = NULL;
  entry->key = malloc(sizeof(char) * (strlen(key) + 1));
  strcpy(entry->key, key);

  uint64_t hash = calc_hash(key) % VT_SIZE;
  
  if (!vt[hash]) {
    vt[hash] = entry;
  }
  else {
    entry->next = vt[hash];
    vt[hash] = entry;
  }
}

bool visited_table_contains(struct vt_entry  **vt, const char *key)
{
  uint64_t hash = calc_hash(key) % VT_SIZE;
  if (!vt[hash])
    return false;

  struct vt_entry *e = vt[hash];
  do {
    if (strcmp(key, e->key) == 0)
      return true;
    e = e->next;
  }
  while (e);

  return false;
}

char *config_to_key(const uint8_t *config)
{
  char *key = calloc(CONFIG_LEN, sizeof(char));
  for (int i = 0; i < CONFIG_LEN - 1; i++) {
    key[i] = (char) config[i+1] + '0';
  }

  return key;
}

bool finished(uint8_t *config)
{
  for (int j = 0; j < CONFIG_LEN; j++) {
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
      other_state->moves = curr_state->moves + 1;
      for (int j = 0; j < CONFIG_LEN; j++)
        other_state->config[j] = curr_state->config[j];

      // check up one floor
      other_state->config[0] = elevator + 1;
      other_state->config[i] = elevator + 1;

      //printf("  check ");
      //for (int i = 0; i < CONFIG_LEN; i++)
      //  printf(" %d", other_state->config[i]);
      //printf("\n");
      if (valid_config(other_state->config)) {
        *moves_count += 1;
        moves = realloc(moves, *moves_count * sizeof(struct state *));
        moves[*moves_count - 1] = state_copy(other_state);
      }
     
      // check down one floor
      other_state->config[0] = elevator - 1;
      other_state->config[i] = elevator - 1;

      //printf("  check ");
      //for (int i = 0; i < CONFIG_LEN; i++)
      //  printf(" %d", other_state->config[i]);
      //printf("\n");
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

          //printf("  check ");
          //for (int j = 0; j < CONFIG_LEN; j++)
          //  printf(" %d", other_state->config[j]);
          //printf("\n");
          if (valid_config(other_state->config)) {
            *moves_count += 1;
            moves = realloc(moves, *moves_count * sizeof(struct state *));
            moves[*moves_count - 1] = state_copy(other_state);
          }

          other_state->config[0] = elevator - 1;
          other_state->config[i] = elevator - 1;
          other_state->config[k] = elevator - 1;

          //printf("  check ");
          //for (int j = 0; j < CONFIG_LEN; j++)
          //  printf(" %d", other_state->config[j]);
          //printf("\n");
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

void dump_config(uint8_t *config) 
{
  for (int j = 0; j < CONFIG_LEN; j++)
    printf(" %d", config[j]);
  printf("\n");
}

void p1() {
  struct vt_entry **vt = visited_table_create();
  uint32_t shortest = UINT32_MAX;

  struct state *initial = malloc(sizeof(struct state));
  initial->moves = 0;
  initial->next = NULL;

  initial->config = calloc(CONFIG_LEN, sizeof(uint8_t));
  initial->config[0] = 1; // elevator
  // in order of chip then generator so chips are ood indexes
  // and their corresponding generators are even indexes
  initial->config[1] = 1;
  initial->config[2] = 2;
  initial->config[3] = 1;
  initial->config[4] = 3;

  struct state *to_test_q = initial;

  while (to_test_q) {
    struct state *curr = to_test_q;
    printf("Testing: ");
    dump_config(curr->config);

    if (finished(curr->config)) {
      if (curr->moves < lowest_moves)
        lowest_moves = curr->moves;
      to_test_q = to_test_q->next;
      state_destroy(curr);
      continue;
    }

    // add to visited table
    char *key = config_to_key(curr->config);
    visited_table_insert(vt, key);
    free(key);

    struct state **moves = NULL;
    int moves_count = 0;
    moves = find_valid_moves(moves, &moves_count, to_test_q);

    for (int j = 0; j < moves_count; j++) {
      char *move_key = config_to_key(moves[j]->config);
      if (moves[j]->moves < lowest_moves && !visited_table_contains(vt, move_key)) {
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
  }

  // need to free any remaining items in to_test_q

  visited_table_destroy(vt);

  printf("P1: %d\n", lowest_moves);
}

int main(void)
{ 
  p1();
  // visited_table_insert(vt, "hello, world?");
  // visited_table_insert(vt, "test");
  // visited_table_insert(vt, "lorem ipsum");

  // char *s = "foo";
  // printf("%d\n", visited_table_contains(vt, "hello, world?"));
  // printf("%d\n", visited_table_contains(vt, "hello, world!"));
  // printf("%d\n", visited_table_contains(vt, "test"));
  // printf("%d\n", visited_table_contains(vt, s));


  //uint8_t **next_moves = NULL;
  //int moves_len = 0;

  //find_valid_moves(next_moves, &moves_len, config);

}
