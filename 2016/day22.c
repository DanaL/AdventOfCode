#include <stdio.h>
#include <stdint.h>
#include <stdlib.h>
#include <string.h>
#include "utils.h"

struct node {
  int x;
  int y;
  int size;
  int used;
  int available;
  int used_p;
};

int scan_int(char *s, int *pos)
{
  while (s[*pos] < '0' || s[*pos] > '9')
    *pos += 1;

  int val = 0;
  while (s[*pos] >= '0' && s[*pos] <= '9') {
    val = val * 10 + (s[*pos] - '0');
    *pos += 1;
  }

  return val;
}

struct node *node_from_line(char *s)
{
  struct node *n = malloc(sizeof(struct node));
  int pos = 0;    
  n->x = scan_int(s, &pos);
  n->y = scan_int(s, &pos);
  n->size = scan_int(s, &pos);
  n->used = scan_int(s, &pos);
  n->available = scan_int(s, &pos);
  n->used_p = scan_int(s, &pos);

  return n;
}

void p1(void)
{
  size_t lc;
  char **lines = read_all_lines("inputs/day22.txt", &lc);

  struct node *nodes = malloc(lc * sizeof(struct node));

  for (int j = 0; j < lc; j++) {
    struct node *n = node_from_line(lines[j]);
    nodes[j].x = n->x;
    nodes[j].y = n->y;
    nodes[j].size = n->size;
    nodes[j].used = n->used;
    nodes[j].available = n->available;
    nodes[j].used_p = n->used_p;
    free(n);
  }
  lines_free(lines, lc);

  int viable = 0;
  for (int i = 0; i < lc; i++) {
    if (nodes[i].used == 0)
      continue;

    for (int j = 0; j< lc; j++) {
      if (i == j)
        continue;
      if (nodes[i].used < nodes[j].available)
        ++viable;
    } 
  }
  printf("P1: %d\n", viable);

  free(nodes);
}

void p2(void)
{
  size_t lc;
  char **lines = read_all_lines("inputs/day22.txt", &lc);

  // First, find the max row and col so I know how much space
  // to allocate for my 2d array. Also, may as well look for the
  // start square here
  struct node **grid = malloc(lc * sizeof(struct node *));
  uint32_t width = 0;
  uint32_t height = 0;
  size_t start_x, start_y;
  int empty_size = 0;
  for (int j = 0; j < lc; j++) {
    struct node *n = node_from_line(lines[j]);
    if (n->x > width)
      width = n->x;
    if (n->y > height)
      height = n->y;
    if (n->used_p == 0) {
      start_x = n->x;
      start_y = n->y;
    }
    grid[j] = n; 

    if (n->used == 0)
      empty_size = n->size;
  }
  ++width;
  ++height;
  printf("\nwidth %lu\n", width);
  printf("start: %lu, %lu\n", start_x, start_y);

  char *map = malloc(width * height * sizeof(char));
  for (int j = 0; j < lc; j++) {
    struct node *n = grid[j];
    size_t idx = n->y * width + n->x;
    char ch;
    if (n->used == 0)
      map[idx] = '_';
    else if (n->used > empty_size)
      map[idx] = '#';
    else
      map[idx] = '.';
  }
  
  
  for (int r = 0; r < height; r++) {
    for (int c = 0; c < width; c++) {
      printf("%c", map[r*width + c]);
    }
    printf("\n");
  }

  for (int j = 0; j < lc; j++) {
    free(grid[j]);
  }
  free(grid);
  free(map);
}

int main(void)
{
  p1();
  p2();
}
