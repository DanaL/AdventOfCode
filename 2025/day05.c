#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define BUFF_LEN 128

int main(void)
{
  FILE *fp = fopen("data/day05.txt", "rb");
  char buffer[BUFF_LEN];

  int read_state = 0;
  while (fgets(buffer, BUFF_LEN, fp)) {
    if (strcmp(buffer, "\n") == 0) {
      read_state = 1;
    }
    else if (read_state == 0) {
      unsigned long long a, b;
      sscanf(buffer, "%llu-%llu", &a, &b);
      printf("%llu, %llu\n", a, b);
    }
    else {
      unsigned long long v = strtoull(buffer, NULL, 10);
      printf("%llu\n", v);
    }
  }

  fclose(fp);

  return 0;
}
