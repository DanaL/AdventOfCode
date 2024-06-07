#include <stdio.h>
#include <string.h>

#define SCR_W 50
#define SCR_H 6

int count_lit(unsigned int screen[SCR_H][SCR_W]) 
{
  int count = 0;
  for (int r = 0; r < SCR_H; r++) {
    for (int c = 0; c < SCR_W; c++) {
      if (screen[r][c])
       ++count;
    }
  }

  return count;
}

void draw_rect(unsigned int screen[SCR_H][SCR_W], int width, int height)
{
  for (int r = 0; r < height; r++) {
    for (int c = 0; c < width; c++) {
      screen[r][c] = 1;
    }
  }
}


void rotate_col(unsigned int screen[SCR_H][SCR_W], int col, int n)
{
  unsigned int scratch[SCR_H];
  for (int r = 0; r < SCR_H; r++) {
    int i = (r + n) % SCR_H;
    scratch[i] = screen[r][col];
  }

  for (int r = 0; r < SCR_H; r++) {
    screen[r][col] = scratch[r];
  }
}

void rotate_row(unsigned int screen[SCR_H][SCR_W], int row, int n)
{
  unsigned int scratch[SCR_W];
  for (int c = 0; c < SCR_W; c++) {
    int i = (c + n) % SCR_W;
    scratch[i] = screen[row][c];
  }

  for (int c = 0; c < SCR_W; c++) {
    screen[row][c] = scratch[c];
  }
}

void display(unsigned int screen[SCR_H][SCR_W])
{
  for (int r = 0; r < SCR_H; r++) {
    for (int c = 0; c < SCR_W; c++) {
      printf("%c", screen[r][c] ? '#' : '.');
    }
    printf("\n");
  }
}

int main(void)
{
  unsigned int screen[SCR_H][SCR_W];
  for (int r = 0; r < SCR_H; r++) {
    for (int c = 0; c < SCR_W; c++) {
      screen[r][c] = 0;
    }
  }

  char buffer[100];
  FILE *fp = fopen("inputs/day08.txt", "r");
  while (fgets(buffer, sizeof buffer, fp) != NULL)
  {
    if (strncmp("rect", buffer, 4) == 0) {
      int width, height;
      sscanf(buffer, "rect %dx%d", &width, &height);
      draw_rect(screen, width, height);
    }
    else if (strncmp(buffer, "rotate col", 8) == 0) {
      int col, n;
      sscanf(buffer, "rotate column x=%d by %d", &col, &n);
      rotate_col(screen, col, n);
    }
    else {
      int row, n;
      sscanf(buffer, "rotate row y=%d by %d", &row, &n);
      rotate_row(screen, row, n);
    }
  }
  fclose(fp);

  display(screen);
  printf("P1: %d\n", count_lit(screen));
}
