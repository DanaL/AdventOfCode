#include <stdio.h>
#include <stdlib.h>

#define BUFF_LEN 128
#define LIST_LEN 1024

typedef struct {
  long x;
  long y;
} Point;

int main(void)
{
  FILE *fp = fopen("data/day09.txt", "r");

  Point pts[LIST_LEN];
  size_t num_pts = 0;

  char buffer[BUFF_LEN];
  long x, y;
  while (fgets(buffer, BUFF_LEN, fp)) {
    sscanf(buffer, "%ld,%ld", &x, &y);
    pts[num_pts].x = x;
    pts[num_pts].y = y;
    ++num_pts;
  }
  fclose(fp);

  long long largest_area = 0;
  for (size_t j = 0; j < num_pts; j++) {
    for (size_t k = j + 1; k < num_pts; k++) {
      long long area = llabs((pts[j].x - pts[k].x + 1) * (pts[j].y - pts[k].y + 1));
      if (area > largest_area)
        largest_area = area;
    }
  }

  printf("P1: %lld\n", largest_area);

  return 0;
}
