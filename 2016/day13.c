#include <stdio.h>
#include <stdbool.h>
#include <limits.h>

#define MAGIC_NUMBER 10

#define WIDTH 100
#define HEIGHT 100

struct point {
  int x;
  int y;
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

  printf("%d %d %d\n", n, ones, ones % 2);

  return (ones % 2 != 0);
}

bool in_bounds(int x, int y)
{
  return x >= 0 && y >= 0 && x < WIDTH && y < HEIGHT;
}

int shortest_path(int start_x, int start_y) 
{ 
  int grid[HEIGHT][WIDTH];
  for (int i = 0; i < HEIGHT; i++) {
    for (int j = 0; j < WIDTH; j++) {
      grid[i][j] = INT_MAX;
    }
  }
  grid[start_x][start_y] = 0;

  return grid[1][1];
}

void p1(void)
{
  printf("P1: %d\n", shortest_path(7, 4));
}

int main(void)
{
  p1();
}