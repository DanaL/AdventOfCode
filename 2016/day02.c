#include <stdio.h>

#define BUFFER_SIZE 1024

unsigned int calc_num(int num, int row, int col)
{
  return num * 10 + row * 3 + col + 1;
}

void p1(void)
{
  FILE *fp;
  char s[BUFFER_SIZE];
  int c, num_of_moves = 0;

  int code[BUFFER_SIZE];
  int row = 1, col = 1;
  int nr, nc;
  unsigned int num = 0;
  fp = fopen("inputs/day02.txt", "r");
  while ((c = fgetc(fp)) != EOF) {
    nr = row;
    nc = col;
    switch (c) {
      case 'U':
        nr = row - 1;
        break;
      case 'D':
        nr = row + 1;
        break;
      case 'L':
        nc = col - 1;
        break;
      case 'R':
        nc = col + 1;
        break;
      case '\n':
        num = calc_num(num, row, col);
        break;
      default:
        continue;
    }

    if (nr >= 0 && nr < 3 && nc >= 0 && nc < 3)
    {
      row = nr;
      col = nc;
    }
  }

  num = calc_num(num, row, col);
  printf("P1: %u\n", num);

  fclose(fp);
}

int main(void)
{
  p1();
}
