#include <stdio.h>
#include <stdlib.h>

#define D(a, b) llabs(((a)->x - (b)->x + 1) * ((a)->y - (b)->y + 1))

#define BUFF_LEN 128
#define LIST_LEN 1024

typedef struct {
  long x;
  long y;
} Point;

void p1(Point *pts, size_t num_pts)
{
  long long largest_area = 0;
  for (size_t j = 0; j < num_pts; j++) {
    for (size_t k = j + 1; k < num_pts; k++) {
      long long area = D(&pts[j], &pts[k]);
      if (area > largest_area)
        largest_area = area;
    }
  }

  printf("P1: %lld\n", largest_area);
}

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

  p1(pts, num_pts);

  return 0;
}
