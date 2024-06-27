#include <stdio.h>
#include <string.h>
#include <stdlib.h>

int count_safe(const char *s)
{
  int safe = 0;
  while (*s != '\0') {
    if (*s++ == '.')
      ++safe;
  }

  return safe;
}

char calc_trap(char left, char centre, char right) 
{
  if (left == '^' && centre == '^' && right == '.')
    return '^';
  else if (left == '.' && centre == '^' && right == '^')
    return '^';
  else if (left == '^' && centre == '.' && right == '.')
    return '^';
  else if (left == '.' && centre == '.' && right == '^')
    return '^';

  return '.';
}
char *next_gen(const char *s) 
{
  size_t len = strlen(s);
  char *n = calloc(len + 1, sizeof(char));

  n[0] = calc_trap('.', s[0], s[1]);
  n[len-1] = calc_trap(s[len-2], s[len-1], '.');
  for (size_t j = 1; j < len - 1; j++) {
    n[j] = calc_trap(s[j-1], s[j], s[j+1]);
  }

  return n;
}

int solve(char *seed, size_t generations)
{
  char *s = calloc(strlen(seed) + 1, sizeof(char)); 
  strcpy(s, seed);
  int safe = 0;

  for (size_t j = 0; j < generations; j++) {
    safe += count_safe(s);    
    char *n = next_gen(s);
    free(s);
    s = n;
  }
  
  free(s);

  return safe;
}

int main(void)
{
  char *seed = ".^^.^^^..^.^..^.^^.^^^^.^^.^^...^..^...^^^..^^...^..^^^^^^..^.^^^..^.^^^^.^^^.^...^^^.^^.^^^.^.^^.^.";
  printf("P1: %d\n", solve(seed, 40));
  printf("P1: %d\n", solve(seed, 400000));
}