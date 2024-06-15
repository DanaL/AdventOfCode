#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <string.h>
#include <stdbool.h>

#define FNV_OFFSET 14695981039346656037UL
#define FNV_PRIME 1099511628211UL

#define VT_SIZE 101719

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

int main(void)
{ 
  struct vt_entry **vt = visited_table_create();

  visited_table_insert(vt, "hello, world?");
  visited_table_insert(vt, "test");
  visited_table_insert(vt, "lorem ipsum");

  char *s = "foo";
  printf("%d\n", visited_table_contains(vt, "hello, world?"));
  printf("%d\n", visited_table_contains(vt, "hello, world!"));
  printf("%d\n", visited_table_contains(vt, "test"));
  printf("%d\n", visited_table_contains(vt, s));

  visited_table_destroy(vt);
}