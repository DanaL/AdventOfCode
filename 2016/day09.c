#include <stdio.h>
#include <stdlib.h>
#include <string.h>

char *decompress(char *compressed)
{
  char *decompressed = NULL;
  int m = 0, n = 0;
  char *start = compressed, *c = compressed;

  while (*c != '\0' && *c != '(') {
    printf("%d %c\n", n, *c);
    ++c;
    ++n;
  }

  int i = n - m;
  decompressed = malloc(sizeof(char) * (i + 1));
  
  strncpy(decompressed, start, i);
  decompressed[i] = '\0';

  return decompressed;
}

int main(void)
{
  char *s = "XYZ(1x4)GHIK";

  char *d = decompress(s);
  printf("d: %s is length %d\n", d, strlen(d));

  printf("%d %d\n", s, d);
  free(d);
}
