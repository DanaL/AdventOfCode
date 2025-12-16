#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <string.h>

#define BUFF_LEN 128

typedef struct connections {
  size_t num_items;
  char (*items)[3];
} Connections;

typedef struct ht_node {
  char name[3];
  char connected_to[3];
  struct ht_node *next;
} HTNode;

/* A quick, dirty hashtable implementation */
#define HASH_TABLE_CAPACITY 1699

size_t ht_hash(const char n[3])
{
  return (size_t) ((n[0] * 73856093) ^ (n[1] * 19349663) ^ (n[2] * 977923)) % HASH_TABLE_CAPACITY;
}

void connections_free(Connections *cons)
{
  if (cons) {
    free(cons->items);
    free(cons);
  }
}

// Functions for the hash table storing the graph nodes
Connections* ht_get_connection(HTNode **ht, const char name[3])
{
  size_t hash = ht_hash(name);
  char buffer[HASH_TABLE_CAPACITY][3];
  size_t num_items = 0;

  HTNode *n = ht[hash];
  while (n) {
    if (memcmp(n->name, name, 3) == 0) {
      memcpy(buffer[num_items], n->connected_to, 3);
      num_items++;
    }
    n = n->next;
  }

  if (num_items == 0)
    return NULL;

  Connections *cons = malloc(sizeof(Connections));
  cons->items = malloc(num_items * 3);
  memcpy(cons->items, buffer, num_items * 3);
  cons->num_items = num_items;

  return cons;
}

void ht_add_connection(HTNode **ht, const char from[3], const char to[3])
{
  size_t hash = ht_hash(from);
  HTNode *node = malloc(sizeof(HTNode));

  memcpy(node->name, from, 3);
  memcpy(node->connected_to, to, 3);

  node->next = ht[hash];
  ht[hash] = node;
}

void ht_free(HTNode **ht)
{
  for (size_t i = 0; i < HASH_TABLE_CAPACITY; i++) {
    HTNode *node = ht[i];
    while (node) {
      HTNode *next = node->next;
      free(node);
      node = next;
    }
  }
  free(ht);
}

// Functions the memoized results table
typedef struct memo_node {
  char name[3];
  uint64_t count;
  struct memo_node *next;
} MemoNode;
MemoNode **path_counts;

void path_counts_insert(MemoNode **pc, const char name[3], uint64_t count)
{
  size_t hash = ht_hash(name);
  MemoNode *node = malloc(sizeof(MemoNode));
  memcpy(node->name, name, 3);
  node->count = count;

  node->next = pc[hash];
  pc[hash] = node;
}

uint64_t path_counts_find(MemoNode **pc, const char name[3])
{
  size_t hash = ht_hash(name);
  MemoNode *node = pc[hash];

  while (node) {
    if (memcmp(node->name, name, 3) == 0) {
      return node->count;
    }
    node = node->next;
  }

  return 0;
}

void path_counts_free(MemoNode **pc)
{
  for (size_t i = 0; i < HASH_TABLE_CAPACITY; i++) {
    MemoNode *node = pc[i];
    while (node) {
      MemoNode *next = node->next;
      free(node);
      node = next;
    }
  }
  free(pc);
}

uint64_t count_paths(HTNode **ht, const char node[3], const char goal[3])
{
  if (memcmp(node, goal, 3) == 0) {
    return 1;
  }

  uint64_t cached = path_counts_find(path_counts, node);
  if (cached > 0) {
    return cached;
  }

  uint64_t total = 0;
  Connections *c = ht_get_connection(ht, node);
  for (size_t j = 0; j < c->num_items; j++) {
    total += count_paths(ht, c->items[j], goal);
  }
  connections_free(c);

  path_counts_insert(path_counts, node, total);

  return total;
}

int main(void)
{
  HTNode **ht = calloc(HASH_TABLE_CAPACITY, sizeof(HTNode*));
  path_counts = calloc(HASH_TABLE_CAPACITY, sizeof(MemoNode*));

  char buffer[BUFF_LEN];
  char name[3], con[3];
  FILE *fp = fopen("data/day11.txt", "r");
  while (fgets(buffer, BUFF_LEN, fp)) {
    memcpy(name, buffer, 3);
  
    for (size_t j = 4; j < strlen(buffer); j++) {
      if (buffer[j] >= 'a' && buffer[j] <= 'z') {
        memcpy(con, &buffer[j], 3);
        j += 3;
        ht_add_connection(ht, name, con);
      }
    }
  }
  fclose(fp);

  name[0] = 'y';
  name[1] = 'o';
  name[2] = 'u';  
  
  char goal[3] = { 'o', 'u', 't'};
  printf("P1: %llu\n", count_paths(ht, name, goal));

  path_counts_free(path_counts);
  ht_free(ht);

  return 0;
}