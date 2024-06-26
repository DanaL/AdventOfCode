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

void p1(const char *seed)
{
  struct node *n = node_create(1, 1, seed);
  
  // gonna need a queue of nodes to visit starting
  // with the initial one
  //
  // does it make sense to track visited? or are 
  // all paths going to be unique?
  //
  // I guess I can at least insert into heap ordered
  // by path length and then bail on paths longer
  // than known shortest
  char *h = md5(n->path);
  printf("%s\n", h);

  // up, down, left, right
  for (int j = 0; j < 4; j++) {
    if (h[j] >= 'b' && h['j'] <= 'f') {
      if (j == UP && in_bounds(n->row - 1, n->col)) {
        printf("up\n");
      }
      else if (j == DOWN && in_bounds(n->row + 1, n->col)) {
        printf("down\n");
      }
      else if (j == LEFT && in_bounds(n->row, n->col - 1)) {
       printf("left\n");
      }
      else if (j == RIGHT && in_bounds(n->row, n->col + 1)) {
        printf("right\n");
      }
    }    
  }

  free(h);
  free(n);
}

int priority(const void *item)
{
  struct node *n = item;

  return strlen(n->path);
}


struct num {
  int x;
};

int p(const void *x)
{
  struct num *n = x;

  return n->x;
}



int main(void)
{
  //p1("hijkl");

  struct heap *h = heap_new();

  srand(120091189);
  for (int j = 0; j < 10; j++) {
    struct num *n = malloc(sizeof(struct num));
    n->x = 1000 - (rand() % 2000);
    min_heap_push(h, n, p);
    printf("%d\n", n->x);
  }
  
  printf("\n");
  while (h->num_of_elts) {
    struct num *n = min_heap_pop(h, p);
    printf("%d\n", n->x);
  }
}