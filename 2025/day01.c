#include <stdio.h>
#include <stdlib.h>

#define BUFF_SIZE 1024

#define mod(n) ((((n) % 100) + 100) % 100)

int main(void) {
  FILE *fp;
  char s[BUFF_SIZE];

  int zeroes = 0;
  int digit = 50;
  fp = fopen("data/day01.txt", "r");
  while (fgets(s, BUFF_SIZE, fp)) {
    char *c = (s+1);
    int v = atoi(c);
    if (*s == 'L')
      v *= -1;
    
    digit = mod(digit + v);
    if (digit == 0)
      ++zeroes;
  }
  fclose(fp);

  printf("P1: %d\n", zeroes);

  return 0;
}
