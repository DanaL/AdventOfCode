#include <stdio.h>
#include <stdbool.h>

#define TABLE_SIZE 2000

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

void p1(void)
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
 
  printf("P1: %d\n", valid_count);

  fclose(fp);
}

// return the # of valids triangles found in a column
int check_col(int *arr, int len)
{
  int valid_count = 0;
  for (int j = 0; j < len - 2; j += 3) {
    if (valid(arr[j], arr[j+1], arr[j+2])) {
      ++valid_count;
    }
  }

  return valid_count;
}

void p2(void)
{
  int col_a[TABLE_SIZE];
  int col_b[TABLE_SIZE];
  int col_c[TABLE_SIZE];
  FILE *fp;
  char line[20];

  fp = fopen("inputs/day03.txt", "r");

  int a, b, c;
  int n = 0;
  while (fgets(line, sizeof line, fp) != NULL) {
    sscanf(line, "%d %d %d", &a, &b, &c);
    col_a[n] = a;
    col_b[n] = b;
    col_c[n] = c;
    ++n;
  }

  int total = check_col(col_a, n) + check_col(col_b, n) + check_col(col_c, n);
  printf("P2: %d", total);

  fclose(fp);
}

int main(void)
{
  p1();
  p2();
}
