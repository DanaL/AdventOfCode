#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdint.h>

#define BUFF_LEN 128

typedef struct {
  uint64_t lower;
  uint64_t upper;
} Range;

int compare_ranges(const void *a, const void *b)
{
  Range *r1 = (Range *)a;
  Range *r2 = (Range *)b;

  return (r1->lower > r2->lower) - (r1->lower < r2->lower);
}

size_t merge(Range *ranges, size_t range_count)
{
  qsort(ranges, range_count, sizeof(Range), compare_ranges);

  size_t write_idx = 0;
  for (size_t j = 1; j < range_count; j++) {
    if (ranges[write_idx].upper >= ranges[j].lower) {
      if (ranges[j].upper > ranges[write_idx].upper)
        ranges[write_idx].upper = ranges[j].upper;
    }
    else {
      ranges[++write_idx] = ranges[j];
    }
  }

  return write_idx + 1;
}

int p1_check(const Range ranges[], size_t range_count, uint64_t v) {
  for (size_t j = 0; j < range_count; j++)
  {
    if (v >= ranges[j].lower && v <= ranges[j].upper)
      return 1;
  }
 
  return 0;
}

int main(void)
{
  FILE *fp = fopen("data/day05.txt", "rb");
  char buffer[BUFF_LEN];

  Range ranges[1000];
  size_t range_count = 0;
  int read_state = 0;  
  int p1 = 0;
  while (fgets(buffer, BUFF_LEN, fp)) {    
    if (buffer[0] == '\n' || buffer[0] == '\r') {
      read_state = 1;
    }
    else if (read_state == 0) {
      uint64_t a, b;
      sscanf(buffer, "%llu-%llu", &a, &b);
      ranges[range_count].lower = a;
      ranges[range_count].upper = b;
      ++range_count;
    }
    else {
      uint64_t v = strtoull(buffer, NULL, 10);
      p1 += p1_check(ranges, range_count, v);
    }
  }
  fclose(fp);

  printf("P1: %d\n", p1);

  range_count = merge(ranges, range_count);
  uint64_t p2 = 0;
  for (size_t j = 0; j < range_count; j++) {
    p2 += ranges[j].upper - ranges[j].lower + 1;
  }
  printf("P2: %llu\n", p2);

  return 0;
}
