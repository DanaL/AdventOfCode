#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <string.h>
#include <stdbool.h>

#include "utils.h"

#define UP 0
#define DOWN 1
#define LEFT 2
#define RIGHT 3

#define OPEN(c) ((c) >= 'b' && (c) <= 'f')

struct node {
  uint8_t row;
  uint8_t col;
  char *path;
};

struct node *node_create(uint8_t row, uint8_t col, const char *path) 
{
  struct node *n = malloc(sizeof(struct node *));
  n->row = row;
  n->col = col;
  n->path = malloc((strlen(path) + 1) * sizeof(char));
  strcpy(n->path, path);

  return n;
}

void node_free(struct node *n)
{
  free(n->path);
  free(n);
}

bool in_bounds(uint8_t row, uint8_t col)
{
  return row > 0 && row < 5 && col > 0 && col < 5;
}

int priority(const void *item)
{
  struct node *n = item;

  return strlen(n->path);
}

void p1(const char *seed)
{
  char path[100];
  struct node *initial = node_create(1, 1, seed);
  uint32_t shortest = UINT32_MAX;
  uint32_t longest = 0;
  struct heap *q = heap_new();
  min_heap_push(q, initial, priority);

  char buffer[1000];
  while (q->num_of_elts) {
    struct node *curr = min_heap_pop(q, priority);
    char *hash = md5(curr->path);

    //if (strlen(curr->path) > shortest) {
    //  goto iterate;
    //}

    if (curr->row == 4 && curr->col == 4) {
      size_t path_len = strlen(curr->path) - strlen(seed);
      if (path_len < shortest) {
        shortest = path_len;
        strcpy(path, &curr->path[strlen(seed)]);
      }
      if (path_len > longest) {
        longest = path_len;
      }
      goto iterate;
    }

    size_t path_len = strlen(curr->path);
    strcpy(buffer, curr->path);
    buffer[path_len+1] = '\0';

    if (OPEN(hash[UP]) && in_bounds(curr->row-1, curr->col)) {
      buffer[path_len] = 'U';
      struct node *n = node_create(curr->row-1, curr->col, buffer);
      min_heap_push(q, n, priority);
    }
    if (OPEN(hash[DOWN]) && in_bounds(curr->row+1, curr->col)) {
      buffer[path_len] = 'D';
      struct node *n = node_create(curr->row+1, curr->col, buffer);
      min_heap_push(q, n, priority);
    }
    if (OPEN(hash[LEFT]) && in_bounds(curr->row, curr->col-1)) {
      buffer[path_len] = 'L';
      struct node *n = node_create(curr->row, curr->col-1, buffer);
      min_heap_push(q, n, priority);
    }
    if (OPEN(hash[RIGHT]) && in_bounds(curr->row, curr->col+1)) {
      buffer[path_len] = 'R';
      struct node *n = node_create(curr->row, curr->col+1, buffer);
      min_heap_push(q, n, priority);
    }
iterate:
    free(hash);
    node_free(curr);
  }

  printf("P1: %s\n", path);
  printf("P2: %u\n", longest);
}

int main(void)
{
  //p1("rrrbmfta");
  p1("ulqzkmiv");
}
