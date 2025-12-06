#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <string.h>

void skip_spaces(const char *s, size_t *pos) {
  while (s[*pos] == ' ' && s[*pos] != '\0') {
   ++(*pos);
  }
}

uint64_t scan_p1(const char *s, size_t *pos) {
  char digits[50];
  size_t write_idx = 0;
  while (s[*pos] >= '0' && s[*pos] <= '9') {
    digits[write_idx++] = s[*pos];
    ++(*pos);
  }
  digits[write_idx] = '\0';
  uint64_t result = atoi(digits);

  return result;
}

int main(void)
{
  char buffer[4096];
  char ops[4096];
  uint64_t grid[10][2048];

  size_t rows = 0;
  FILE *fp = fopen("data/day06.txt", "r");
  while (fgets(buffer, 4096, fp)) {
    if (buffer[0] >= '0' && buffer[0] <= '9') {
      buffer[strlen(buffer) - 1] = '\0'; // skip the newline char
      
      size_t cols = 0;
      size_t pos = 0;
      do {        
        grid[rows][cols++] = scan_p1(buffer, &pos);
        skip_spaces(buffer, &pos);
      } 
      while (buffer[pos] != '\0');
      
      ++rows;
    }
    else {
      strcpy(ops, buffer);
      ops[strlen(ops) - 1] = '\0';
    }
  }
  fclose(fp);

  uint64_t p1 = 0;
  size_t col = 0, pos = 0;
  while (ops[pos] != '\0') {
    uint64_t n = grid[0][col];
    char op = ops[pos++];
    for (size_t r = 1; r < rows; r++) {
      if (op == '+')
        n += grid[r][col];
      else
        n *= grid[r][col];
    }
    p1 += n;
    ++col;
    
    skip_spaces(ops, &pos);  
  }
  
  printf("P1: %llu\n", p1);

  return 0;
}
