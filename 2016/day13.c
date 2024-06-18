#include <stdio.h>
#include <stdbool.h>
#include <stdint.h>

#define MAGIC_NUMBER 10

#define WIDTH 100
#define HEIGHT 100

struct point {
  uint32_t x;
  uint32_t y;
  struct point *next;
};

// x*x + 3*x + 2*x*y + y + y*y + magic number
bool is_wall(uint32_t x, uint32_t y)
{
  uint32_t n = x*x + 3*x + 2*x*y + y + y*y + MAGIC_NUMBER;
  uint32_t i = 1;
  uint32_t ones = 0;

  while (i <= n) {
    if ((i & n) != 0)
      ++ones;
    i <<= 1;
  }

  printf("%d %d %d\n", n, ones, ones % 2);

  return (ones % 2 != 0);
}

uint32_t shortest_path(uint32_t start_x, uint32_t start_y) 
{
  uint32_t grid[HEIGHT][WIDTH];
  for (int i = 0; i < HEIGHT; i++) {
    for (int j = 0; j < WIDTH; j++) {
      grid[i][j] = UINT32_MAX;
    }
  }
  grid[start_x][start_y] = 0;

  return grid[1][1];
}

void p1(void)
{
  printf("P1: %lu\n", shortest_path(7, 4));
}

int main(void)
{
  p1();
}