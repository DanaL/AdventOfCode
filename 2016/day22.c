#include <stdio.h>
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

int main(void)
{
  size_t lc;
  char **lines = read_all_lines("inputs/day22.txt", &lc);

  struct node *nodes = malloc(lc * sizeof(struct node));

  for (int j = 0; j < lc; j++) {
    int pos = 0;    
    nodes[j].x = scan_int(lines[j], &pos);
    nodes[j].y = scan_int(lines[j], &pos);
    nodes[j].size = scan_int(lines[j], &pos);
    nodes[j].used = scan_int(lines[j], &pos);
    nodes[j].available = scan_int(lines[j], &pos);
    nodes[j].used_p = scan_int(lines[j], &pos);
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
      //else if (nodes[j].used > 0 && nodes[j].used < nodes[i].available)
      //  ++viable;
    } 
  }
  printf("P1: %d\n", viable);

  free(nodes);
}