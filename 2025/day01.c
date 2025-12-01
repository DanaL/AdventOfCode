#include <stdio.h>
#include <stdlib.h>

#define BUFF_SIZE 1024

#define mod(n) ((((n) % 100) + 100) % 100)

int main(void) {
  FILE *fp;
  char s[BUFF_SIZE];

  int zeroes = 0, wraps = 0, dial = 50;
  fp = fopen("data/day01.txt", "r");
  while (fgets(s, BUFF_SIZE, fp)) {
    int v = atoi(s + 1);
    if (*s == 'L')
      v *= -1;

    int dial_pre = dial;
    dial = mod(dial + v);

    if (v < 0 && dial_pre < abs(v)) {
      wraps += 1 + (abs(v) - dial_pre) / 100;
      if (dial_pre == 0 || dial == 0)
        --wraps;
    }
    else if (v > 100 - dial_pre) {
      wraps += (dial_pre + v) / 100;
      if (dial == 0)
      --wraps;
    }
    
    if (dial == 0)
      ++zeroes;
  }
  fclose(fp);

  printf("P1: %d\n", zeroes);
  printf("P2: %d\n", zeroes + wraps);

  return 0;
}
