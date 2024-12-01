#include <stdio.h>
#include <stdlib.h>

#define BUFF_SIZE 1024

int compare(const void *a, const void *b)
{
    const int *x = a, *y = b;

    if (*x > *y) return 1;
    if (*x < *y) return -1;
    return 0;
}

int main(void) {
  FILE *fp;
  char s[BUFF_SIZE];

  // First pass to count lines
  int count = 0;
  fp = fopen("inputs/day01.txt", "r");
  while (fgets(s, BUFF_SIZE, fp)) {
    count++;
  }
  fclose(fp);

  int *a, *b;
  a = malloc(count * sizeof(int));
  b = malloc(count * sizeof(int));

  fp = fopen("inputs/day01.txt", "r");
  for (int i = 0; i < count; i++) {
    fgets(s, BUFF_SIZE, fp);
    sscanf(s, "%d %d", &a[i], &b[i]);
  }

  qsort(a, count, sizeof(int), compare);
  qsort(b, count, sizeof(int), compare);

  int totalDistance = 0;
  for (int i = 0; i < count; i++) {
    totalDistance += abs(b[i] - a[i]);
  }

  printf("P1: %d\n", totalDistance);

  free(a);
  free(b);
}
