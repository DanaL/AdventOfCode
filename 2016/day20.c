#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>

#include "utils.h"

struct range {
  uint32_t lo;
  uint32_t hi;
};

int cmp(const void *a, const void *b)
{
  struct range *r1 = (struct range *)a;
  struct range *r2 = (struct range *)b;

  if (r1->lo < r2->lo)
    return -1;
  else if (r1->lo == r2->lo)
    return 0;
  else
    return 1;
}

void p1(void)
{
  size_t lc;
  char **lines = read_all_lines("inputs/day20.txt", &lc);
  struct range *ranges = malloc(lc * sizeof(struct range));

  for (size_t j = 0; j < lc; j++) {
    uint32_t lo, hi;
    sscanf(lines[j], "%zu-%zu", &lo, &hi);
    ranges[j].lo = lo;
    ranges[j].hi = hi;
  }
  lines_free(lines, lc);

  qsort(ranges, lc, sizeof(struct range), cmp);

  uint32_t lowest = ranges[0].hi + 1;
  for (size_t j = 1; j < lc - 1; j++) {
    if (lowest >= ranges[j].lo && lowest <= ranges[j].hi)
      lowest = ranges[j].hi + 1;
  }

  printf("%zu\n", lowest);

  free(ranges);
}

int main()
{
  p1();
}
