#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdint.h>

#include "utils.h"

void p1(void)
{
  char pwd[9] = { '\0' };
  int digit = 0;
  uint32_t x = 0;
  char buffer[100];
  while (digit < 8) {
    sprintf(buffer, "uqwqemis%d", x);
    char *s = md5(buffer);
    if (strncmp(s, "00000", 5) == 0) {
      pwd[digit++] = s[5];
    }
    free(s);
    x++;
  }

  printf("P1: %s\n", pwd);
}

void p2(void)
{
  char pwd[9];
  for (int j = 0; j < 8; j++)
    pwd[j] = '_';
  pwd[8] = '\0';
  printf("%s\n", pwd);
  int i = 0;
  uint32_t x = 0;
  char buffer[100];
  while (i < 8) {
    sprintf(buffer, "uqwqemis%d", x);
    char *s = md5(buffer);
    if (strncmp(s, "00000", 5) == 0) {
      int spot = s[5] - '0';
      char digit = s[6];      
      if (spot >= 0 && spot < 8 && pwd[spot] == '_') {
        pwd[spot] = digit;        
        printf("%  s\n", pwd);
        ++i;
      }
    }
    free(s);
    x++;
  }

  printf("P2: %s\n", pwd);
}

int main(void)
{
  p1();
  p2();
}

