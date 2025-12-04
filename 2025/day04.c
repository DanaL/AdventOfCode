#include <stdio.h>
#include <stdbool.h>
#include <string.h>

#define DIM 150

int adj(bool grid[DIM][DIM], int r, int c) {
  int total = 0;
  if (grid[r-1][c-1]) ++total;
  if (grid[r-1][c]) ++total;
  if (grid[r-1][c+1]) ++total;
  if (grid[r][c-1]) ++total;
  if (grid[r][c+1]) ++total;
  if (grid[r+1][c-1]) ++total;
  if (grid[r+1][c]) ++total;
  if (grid[r+1][c+1]) ++total;

  return total;
}

int calc_p1(bool grid[DIM][DIM], int height, int width) {
  int total = 0;

  for (int r = 1; r <= height; r++) {
    for (int c = 1; c <= width; c++) {
      if (grid[r][c] && adj(grid, r, c) < 4) {
        ++total;        
      }
    }
  }

  return total;
}

int calc_p2(bool grid[DIM][DIM], int height, int width) {
  int total = 0;

  while (true) {
    int iter_total = 0;
    for (int r = 1; r <= height; r++) {
      for (int c = 1; c <= width; c++) {
        if (grid[r][c] && adj(grid, r, c) < 4) {
          ++iter_total;
          grid[r][c] = false;
        }
      }
    }

    if (iter_total == 0)
      break;
    total += iter_total;
  }

  return total;
}

int main(void) 
{
  bool grid[DIM][DIM] = { false };
  char buffer[DIM];

  FILE *fp = fopen("data/day04.txt", "r");
  int line_count = 0, line_width = 0;
  while (fgets(buffer, DIM, fp)) {
    line_width = strlen(buffer);
    ++line_count;

    for (int j = 0; j < line_width; j++)
      grid[line_count][j + 1] = buffer[j] == '@';
  }
  
  printf("P1: %d\n", calc_p1(grid, line_count, line_width));
  printf("P2: %d\n", calc_p2(grid, line_count, line_width));

  return 0;
}
