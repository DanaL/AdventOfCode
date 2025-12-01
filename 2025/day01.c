#include <stdio.h>
#include <stdlib.h>

#define BUFF_SIZE 1024

#define mod(n) ((((n) % 100) + 100) % 100)

void main(void) {
  FILE *fp;
  char s[BUFF_SIZE];

  int zeroes = 0;
  int wraps = 0;
  int dial = 50;
  fp = fopen("data/day01.txt", "r");
  while (fgets(s, BUFF_SIZE, fp)) {
    char *c = (s+1);
    int v = atoi(c);
    if (*s == 'L')
      v *= -1;

    int dial_pre = dial;
    dial = mod(dial + v);

    int delta_wrap = 0;
    if (v < 0 && dial_pre < abs(v)) {
      delta_wrap += 1 + (abs(v) - dial_pre) / 100;
      if (dial_pre == 0 || dial == 0)
        --delta_wrap;
    }
    else if (v > 100 - dial_pre) {
      delta_wrap += (dial_pre + v) / 100;
      if (dial == 0)
        --delta_wrap;
    }
    wraps += delta_wrap;

    if (dial == 0)
      ++zeroes;
  }
  fclose(fp);

  printf("P1: %d\n", zeroes);
  printf("P2: %d\n", zeroes + wraps);
}
