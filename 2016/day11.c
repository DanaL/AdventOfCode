#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <string.h>
#include <stdbool.h>

#define FNV_OFFSET 14695981039346656037UL
#define FNV_PRIME 1099511628211UL

#define VT_SIZE 101719

#define NUM_OF_ELTS 2
#define STATE_LEN (2 * NUM_OF_ELTS + 1)

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

bool valid_state(uint8_t *state)
{  
  // check where the elevator is
  if (state[0] < 1 || state[0] > 4) {
    return false;
  }

  // check to make sure no elements are on a floor with a
  // generator when their generator isn't present
  for (int i = 1; i < STATE_LEN; i += 2) {
    // if the generator is on the same floor as the element,
    // we're good, otherwise we have to make sure there are
    // no other generators on the same floor.
    uint8_t elt_floor = state[i];
    uint8_t elt_gen_floor = state[i+1];
    if (elt_floor == elt_gen_floor) {
      continue;
    }

    for (int j = 2; j < STATE_LEN; j += 2) {
      if (state[j] == elt_floor) {
        return false;
      }
    }
  }

  return true;
}

// moves should start off as null
void find_valid_moves(uint8_t **moves, int *moves_len, uint8_t *curr_state)
{
  uint8_t elevator = curr_state[0];
  uint8_t *other_state = malloc(STATE_LEN * sizeof(uint8_t));

  printf("Elevator at: %d\n", elevator);
  
  // I'm going to loop over each thing on the floor and see what is
  // possible to be moved. Nested loop to check all possible combos?
  for (int i = 1; i < STATE_LEN; i++) {
    if (curr_state[i] == elevator) {
      for (int j = 0; j < STATE_LEN; j++)
        other_state[j] = curr_state[j];

      other_state[0] = elevator + 1;
      other_state[i] = curr_state[i] + 1;
      printf("Checking: \nup ");
      for (int k = 0; k < STATE_LEN; k++)
        printf(" %d ", other_state[k]);
      printf("\n");
      
      printf("  valid? %d\n", valid_state(other_state));
      other_state[0] = elevator - 1;
      other_state[i] = curr_state[i] - 1;
      printf(" down ");
      for (int k = 0; k < STATE_LEN; k++)
        printf(" %d ", other_state[k]);
      printf("\n  valid? %d\n\n", valid_state(other_state));

      // for (int j = i + 1; j < STATE_LEN; j++) {
      //   if (curr_state[i] == curr_state[j]) {
          
      //     printf("  -- possibly can also move %d\n", j);
      //   } 
      // }
    }
  }
  
  free(other_state);

  *moves_len = 44;  
}

int main(void)
{ 
  struct vt_entry **vt = visited_table_create();

  // visited_table_insert(vt, "hello, world?");
  // visited_table_insert(vt, "test");
  // visited_table_insert(vt, "lorem ipsum");

  // char *s = "foo";
  // printf("%d\n", visited_table_contains(vt, "hello, world?"));
  // printf("%d\n", visited_table_contains(vt, "hello, world!"));
  // printf("%d\n", visited_table_contains(vt, "test"));
  // printf("%d\n", visited_table_contains(vt, s));

  uint8_t *state = calloc(STATE_LEN, sizeof(uint8_t));
  state[0] = 1; // elevator
  
  // in order of chip then generator so chips are ood indexes
  // and their corresponding generators are even indexes
  state[1] = 1;
  state[2] = 2;
  state[3] = 1;
  state[4] = 3;

  uint8_t **next_moves = NULL;
  int moves_len = 0;

  find_valid_moves(next_moves, &moves_len, state);

  //  2  2  2  1  3 
  state[0] = 2;
  state[1] = 2;
  state[2] = 2;
  state[3] = 1;
  state[4] = 3;
  printf("fucking valid? %d\n", valid_state(state));

  visited_table_destroy(vt);

  free(state);
}