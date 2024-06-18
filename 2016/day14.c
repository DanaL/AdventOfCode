#include <stdio.h>
#include <stdbool.h>

#include "utils.h"

bool has_repeater(char *s, int count)
{
  for (size_t j = 0; j < strlen(s) - count; j++) {
    if (s[j+1] == s[j]) {
      int n = 2;
      for (size_t k = 1; k < count - 1 ; k++) {
        if (s[j+1+k] == s[j]) 
          ++n;
        else
          break;
      }

      if (n == count)
        return true;
    }
  }

  return false;
}

void p1(void)
{
  char *s = md5("abc200");

  printf("%s\n", s);
  printf("%d\n", has_repeater(s, 5));

  free(s);
}

int main(void)
{
  p1();
}
