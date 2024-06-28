#include <stdio.h>

#include "utils.h"

int main(void)
{
  size_t lc;
  char **lines = read_all_lines("inputs/day21.txt", &lc);
  char buffer[] = "abcde";
  size_t buff_len = strlen(buffer);

  printf("before: %s\n", buffer);

  for (size_t j = 0; j < lc; j++) {
    if (str_starts_with(lines[j], "swap position")) {
      int pos1, pos2;
      sscanf(lines[j], "swap position %d with position %d", &pos1, &pos2);

      char c = buffer[pos1];
      buffer[pos1] = buffer[pos2];
      buffer[pos2] = c;
    }
    else if (str_starts_with(lines[j], "swap letter")) {
      char ch1, ch2;
      sscanf(lines[j], "swap letter %c with letter %c", &ch1, &ch2);

      for (size_t k = 0; k < strlen(buffer); k++) {
        if (buffer[k] == ch1)
          buffer[k] = ch2;
        else if (buffer[k] == ch2)
          buffer[k] = ch1;
      }
    }
    else if (str_starts_with(lines[j], "reverse")) {
      int lo, hi;
      sscanf(lines[j], "reverse positions %d through %d", &lo, &hi);

      while (lo < hi) {
        int tmp = buffer[hi];
        buffer[hi--] = buffer[lo];
        buffer[lo++] = tmp;
      }
    }
    else if (str_starts_with(lines[j], "rotate left")) {
      int steps;
      sscanf(lines[j], "rotate left %d step", &steps);
      steps %= buff_len;

      char tmp[100];
      strcpy(tmp, buffer);
      strncpy(&buffer[buff_len - steps], tmp, steps);
      strncpy(buffer, &tmp[steps], buff_len - steps);
    }
    else if (str_starts_with(lines[j], "rotate right")) {
      int steps;
      sscanf(lines[j], "rotate right %d step", &steps);
      steps %= buff_len;

      char tmp[100];
      strcpy(tmp, buffer);
      strncpy(&buffer[steps], tmp, buff_len - steps);
      strncpy(buffer, &tmp[buff_len - steps], steps);
    }
    else if (str_starts_with(lines[j], "move position")) {
      int from, to;
      sscanf(lines[j], "move position %d to position %d", &from, &to);
      char c = buffer[from];
      char tmp[100];
      strcpy(tmp, buffer);
      size_t i = 0;
      for (size_t k = 0; k < buff_len; k++) {
        if (k == from)
          continue;

        if (k == to)
          buffer[i++] = tmp[k];
        buffer[i++] = tmp[k];
      }
      buffer[to] = c;
    }

    // printf("%d) %s\n", j, buffer);
  }

  printf("after: %s\n", buffer);

  lines_free(lines, lc);
}
