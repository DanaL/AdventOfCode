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

bool is_3_seq(char *s, int i)
{
  return s[i] != s[i+1] && s[i] == s[i+2];
}

bool valid_ssl(char *s)
{
  int c = 0;
  bool in_supernet = true;

  while (c + 2 < strlen(s)) {
    if (s[c] == '[')
      in_supernet = false;
    else if (s[c] == ']')
      in_supernet = true;
    else if (in_supernet && is_3_seq(s, c))
    {
      bool in_hypernet = false;
      char a = s[c];
      char b = s[c+1];

      for (int i = 0; i < strlen(s) - 2; i++)
      {
        if (s[i] == '[')
          in_hypernet = true;
        else if (s[i] == ']')
          in_hypernet = false;
        else if (in_hypernet && is_3_seq(s, i) && s[i] == b && s[i+1] == a)
          return true;
      }
    }
    ++c;
  }

  return false;
}

int main(void)
{
  char buffer[1024];

  int num_valid_abba = 0;
  int num_valid_ssl = 0;
  FILE *fp = fopen("inputs/day07.txt", "r");
  while (fgets(buffer, sizeof buffer, fp) != NULL) {
    if (valid(buffer))
      ++num_valid_abba;
    if (valid_ssl(buffer))
      ++num_valid_ssl;
  }

  printf("P1: %d\n", num_valid_abba);
  printf("P2: %d\n", num_valid_ssl);
}