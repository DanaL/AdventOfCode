#include <stdio.h>
#include <stdbool.h>
#include <stdint.h>

#define MAGIC_NUMBER 10

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

void p1(void)
{
  printf("%d\n", is_wall(9, 5));
}

int main(void)
{
  p1();
}