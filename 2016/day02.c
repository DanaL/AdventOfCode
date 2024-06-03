#include <stdio.h>

void p1(void)
{
  int ch, row = 1, col = 1;
  int nr, nc;
  FILE *fp = fopen("inputs/day02.txt", "r");
  printf("P1: ");
  while ((ch = fgetc(fp)) != EOF) {
    nr = row;
    nc = col;
    switch (ch) {
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
        printf("%d", row * 3 + col + 1);
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


  printf("%d\n", row * 3 + col + 1);

  fclose(fp);
}

void p2(void)
{
  unsigned int keypad[5][5] = {
    { 0,  0,  1,  0, 0 },
    { 0,  2,  3,  4, 0 },
    { 5,  6,  7,  8, 9 },
    { 0, 10, 11, 12, 0 },
    { 0,  0, 13,  0, 0 }
  };

  int ch, row = 2, col = 0;
  int nr, nc, x;
  FILE *fp = fopen("inputs/day02.txt", "r");
  printf("P2: ");
  while ((ch = fgetc(fp)) != EOF) {
    nr = row;
    nc = col;
    switch (ch) {
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
        printf("%X", keypad[row][col]);
        break;
      default:
        continue;
    }

    if (nr >= 0 && nr < 5 && nc >= 0 && nc < 5 && keypad[nr][nc] != 0)
    {
      row = nr;
      col = nc;
    }
  }

  printf("%X\n", keypad[row][col]);
}

int main(void)
{
  p1();
  p2();
}
