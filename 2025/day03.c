#include <stdio.h>
#include <string.h>
#include <stdint.h>

#define BUFF_SIZE 128

int main(void) {
  FILE *fp;
  char buffer[BUFF_SIZE];

  fp = fopen("data/day03.txt", "r");
  uint32_t joltage = 0; 
  while (fgets(buffer, BUFF_SIZE, fp)) {
    char a  = '\0', b = '\0';

    size_t n = strlen(buffer) - 1;
    for (size_t j = 0; j < n; j++) {
      if (buffer[j] > a && j < n - 1) {
        a = buffer[j];
        b = '\0';
      }
      else if (buffer[j] > b) {
        b = buffer[j];
      }
    }

    joltage += (a - '0') * 10 + (b - '0');
  }

  printf("P1: %u\n", joltage);

  fclose(fp);

  return 0;
}
