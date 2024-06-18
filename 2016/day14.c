#include <stdio.h>
#include <stdbool.h>

#include "utils.h"

char has_repeater(char *s, int count)
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
        return s[j];
    }
  }

  return '\0';
}

void p1(void)
{
  char *salt = "abc";
  char buffer[100];

  for (size_t j = 0; j < 30000; j++) {
    sprintf(buffer, "%s%d", salt, j);
    char *s = md5(buffer);
    //printf("%s\n", s);
    char ch3 = has_repeater(s, 3);
    char ch5 = has_repeater(s, 5);
    if (ch3 != '\0' || ch5 != '\0')
      printf("%d", j); 
    if (ch3 != '\0' && ch3 != ch5)
      printf(" 3 repeater");
    if (ch5 != '\0')
      printf(" 5 repeater");
    if (ch3 != '\0' || ch5 != '\0')
      printf("\n");
    free(s);
  }
}

int main(void)
{
  p1();
}
