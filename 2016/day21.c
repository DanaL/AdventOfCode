#include <stdio.h>
#include <string.h>
#include <stdlib.h>

#include "utils.h"

char *goal = "fbgdceah";
char **lines;
size_t lc;

void rotate_left(char buffer[], size_t buff_len)
{
  char tmp[10];
  strcpy(tmp, buffer);
  strncpy(&buffer[buff_len - 1], tmp, 1);
  strncpy(buffer, &tmp[1], buff_len - 1);
}

void rotate_right(char buffer[], size_t buff_len)
{
  char tmp[10];
  strcpy(tmp, buffer);
  strncpy(&buffer[1], tmp, buff_len - 1);
  strncpy(buffer, &tmp[buff_len - 1], 1);
}

void scramble(char *buffer)
{
  size_t buff_len = strlen(buffer);

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

      for (int k = 0; k < steps; k++)
        rotate_left(buffer, buff_len);      
    }
    else if (str_starts_with(lines[j], "rotate right")) {
      int steps;
      sscanf(lines[j], "rotate right %d step", &steps);
      
      for (int k = 0; k < steps; k++)
        rotate_right(buffer, buff_len);
    }
    else if (str_starts_with(lines[j], "rotate based on pos")) {
      char ch = lines[j][strlen(lines[j]) - 1];
      int i = 0;
      for ( ; buffer[i] != ch; i++)
        ;
      int rotations = 1 + i;
      if (i >= 4)
        ++rotations;

      for (int k = 0; k < rotations; k++)
        rotate_right(buffer, buff_len);
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
  }  
}

void swap(char *buffer, size_t a, size_t b)
{
  char tmp = buffer[a];
  buffer[a] = buffer[b];
  buffer[b] = tmp;
}

void permute(char *buffer, size_t start, size_t end)
{
  // check the permutation to see if it's our password
  if (start == end) {
    char copy[10];
    strcpy(copy, buffer);
    scramble(copy);

    if (strcmp(goal, copy) == 0) {
      printf("original pwd: %s\n", buffer);
      exit(0);
    }
    return;
  }

  permute(buffer, start+1, end);

  for (size_t i = start+1; i < end; i++) {
    if (buffer[start] == buffer[i])
      continue;
    swap(buffer, start, i);
    permute(buffer, start+1, end);
    swap(buffer, start, i); // restore order
  }
}

int main(void)
{
  lines = read_all_lines("inputs/day21.txt", &lc);  
  
  char buffer[] = "abcdefgh";
  scramble(buffer);
  printf("P1: %s\n", buffer);

  char buffer2[] = "abcdefgh";
  permute(buffer2, 0, strlen(buffer2));

  lines_free(lines, lc);
}
