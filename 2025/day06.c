#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <string.h>
#include <stdbool.h>

#define GRID_HEIGHT 10
#define GRID_WIDTH 2048

void skip_spaces(const char *s, size_t *pos) {
  while (s[*pos] == ' ') {
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
  
  return strtoull(digits, NULL, 10);
}

void p1(void)
{
  char buffer[4096];
  char ops[4096];
  uint64_t *grid = malloc(GRID_HEIGHT * GRID_WIDTH * sizeof(uint64_t));

  size_t rows = 0;
  FILE *fp = fopen("data/day06.txt", "r");
  while (fgets(buffer, 4096, fp)) {
    if (buffer[0] >= '0' && buffer[0] <= '9') {
      buffer[strlen(buffer) - 1] = '\0'; // skip the newline char
      
      size_t cols = 0, pos = 0;
      do {        
        grid[rows * GRID_WIDTH + cols++] = scan_p1(buffer, &pos);
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
    uint64_t n = grid[col];
    char op = ops[pos++];
    for (size_t r = 1; r < rows; r++) {
      if (op == '+')
        n += grid[r * GRID_WIDTH + col];
      else
        n *= grid[r * GRID_WIDTH + col];
    }
    p1 += n;
    ++col;
    
    skip_spaces(ops, &pos);  
  }
  
  printf("P1: %llu\n", p1);

  free(grid);
}

void p2(void)
{
  char lines[10][4096];
  size_t rows = 0;

  FILE *fp = fopen("data/day06.txt", "r");
  while (fgets(lines[rows], 4096, fp)) {
    size_t len = strlen(lines[rows]);
    if (lines[rows][len - 1] == '\n') {
      lines[rows][len - 1] = '\0';
    }
    ++rows;
  }

  uint64_t nums[10];
  size_t num_idx = 0;
  uint64_t total = 0;
  for (int col = strlen(lines[0]) - 1; col >= 0; col--) {
    uint64_t n = 0;
    bool digit_found = false;

    for (size_t r = 0; r < rows; r++) {
      char ch = lines[r][col];
      if (ch >= '0' && ch <= '9') {
        n = n * 10 + ch - '0';
        digit_found = true;
      }
      else if (ch == '*') {
        uint64_t prod = n;
        for (size_t j = 0; j < num_idx; j++)
          prod *= nums[j];
        total += prod;
      }
      else if (ch == '+') {
        uint64_t sum = n;
        for (size_t j = 0; j < num_idx; j++)
          sum += nums[j];
        total += sum;
      }
    }

    if (digit_found) {
      nums[num_idx++] = n;
    }
    else {
      num_idx = 0; // spacer column
    }
  }

  printf("P2: %llu\n", total);
}

int main(void) 
{
  p1();
  p2();

  return 0;
}
