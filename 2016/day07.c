#include <stdio.h>
#include <stdbool.h>
#include <string.h>

bool is_abba(char *s, int i)
{
  return s[i] != s[i+1] && s[i] == s[i+3] && s[i+1] == s[i+2];
}

bool valid(char *s)
{
  int c = 0;
  bool in_brackets = false;
  bool valid_abba = false;

  while (c + 3 < strlen(s)) {
    if (s[c] == '[')
      in_brackets = true;
    else if (s[c] == ']') 
      in_brackets = false;
    else {
      bool abba = is_abba(s, c);
      if (abba && in_brackets) 
        return false;      
      else if (abba)
        valid_abba = true;
    }
    
    ++c;
  }

  return valid_abba;
}

int main(void)
{
  char buffer[1024];

  int num_valid = 0;
  FILE *fp = fopen("inputs/day07.txt", "r");
  while (fgets(buffer, sizeof buffer, fp) != NULL) {
    if (valid(buffer))
      ++num_valid;
  }
  printf("P1: %d\n", num_valid);
}