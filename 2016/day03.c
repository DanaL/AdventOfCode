#include <stdio.h>
#include <stdbool.h>

bool valid(int a, int b, int c)
{
  if (a + b <= c)
    return false;
  if (a + c <= b)
    return false;
  if (b + c <= a)
    return false;

  return true;
}

int main(void)
{
  FILE *fp;
  char line[20];

  fp = fopen("inputs/day03.txt", "r");

  int a, b, c;
  int valid_count = 0;
  while (fgets(line, sizeof line, fp) != NULL) {
    sscanf(line, "%d %d %d", &a, &b, &c);
    if (valid(a, b, c))
      ++valid_count;
  }
 
  printf("P1: %d", valid_count);

  fclose(fp);
}
