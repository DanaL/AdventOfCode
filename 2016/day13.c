#include <stdio.h>
#include <stdbool.h>
#include <limits.h>

#define MAGIC_NUMBER 1362

#define WIDTH 100
#define HEIGHT 100

struct point {
  int x;
  int y;
  int steps;
  struct point *next;
};

// x*x + 3*x + 2*x*y + y + y*y + magic number
bool is_wall(int x, int y)
{
  int n = x*x + 3*x + 2*x*y + y + y*y + MAGIC_NUMBER;
  int i = 1;
  int ones = 0;

  while (i <= n) {
    if ((i & n) != 0)
      ++ones;
    i <<= 1;
  }

  return ones % 2 != 0;
}

bool in_bounds(int x, int y)
{
  return x >= 0 && y >= 0 && x < WIDTH && y < HEIGHT;
}

struct point *point_make(int x, int y, int steps) 
{
  struct point *pt = malloc(sizeof(struct point));
  pt->x = x;
  pt->y = y;
  pt->steps = steps;
  pt->next = NULL;

  return pt;
}

int shortest_path(int start_x, int start_y) 
{ 
  int grid[HEIGHT][WIDTH];
  for (int i = 0; i < HEIGHT; i++) {
    for (int j = 0; j < WIDTH; j++) {
      grid[i][j] = INT_MAX;
    }
  }

  struct point *q = point_make(start_x, start_y, 0);

  while (q) {
    struct point *curr = q;
    q = q->next;

    if (curr->steps >= grid[curr->y][curr->x]) {
      free(curr);
      continue;
    }

    grid[curr->y][curr->x] = curr->steps;

    if (curr->x == 1 && curr->y == 1) {
      free(curr);
      continue;
    }
    
    int nx, ny;

    // north
    nx = curr->x, ny = curr->y - 1;
    if (in_bounds(nx, ny) && !is_wall(nx, ny)) {
      struct point *north = point_make(nx, ny, curr->steps + 1);
      north->next = q;
      q = north;
    }
    // south
    ny = curr->y + 1;
    if (in_bounds(nx, ny) && !is_wall(nx, ny)) {
      struct point *south = point_make(nx, ny, curr->steps + 1);
      south->next = q;
      q = south;
    }
    // east
    nx = curr->x + 1, ny = curr->y;
    if (in_bounds(nx, ny) && !is_wall(nx, ny)) {
      struct point *east = point_make(nx, ny, curr->steps + 1);
      east->next = q;
      q = east;
    }
    // west
    nx = curr->x - 1;
    if (in_bounds(nx, ny) && !is_wall(nx, ny)) {
      struct point *west = point_make(nx, ny, curr->steps + 1);
      west->next = q;
      q = west;
    }

    free(curr);
  }

  return grid[1][1];
}

void p1(void)
{
  printf("P1: %d\n", shortest_path(31, 39));
}

int main(void)
{
  p1();
}
