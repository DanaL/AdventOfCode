#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>

#define CELL(data, row, col) ((data)->data[(row) * (data)->row_width + (col)])

typedef struct {
  char *data;
  size_t row_width;
  size_t height;
} Data;

void p1(Data *data)
{
  bool *beams = calloc(data->row_width, sizeof(bool));
  char *s = strchr(data->data, 'S');
  size_t start_col = (size_t)(s - data->data);
  beams[start_col] = true;

  int splits = 0;
  for (size_t row = 1; row < data->height; row++) {
    for (size_t b = 0; b < data->row_width - 1; b++) {
      if (!beams[b]) 
        continue;
      
      if (CELL(data, row, b) == '^') {
        ++splits;
        beams[b] = false;
        if (b > 0) beams[b-1] = true;
        if (b < data->row_width - 1) beams[b+1] = true;
      }
    }
  }

  printf("P1: %d\n", splits);

  free(beams);
}

unsigned long long memoized[150][150] = { 0 };

unsigned long long count_paths(Data *data, size_t row, size_t col)
{
  if (row >= data->height)
    return 1;

  if (memoized[row][col] != 0)
    return memoized[row][col];
  
  unsigned long long result;
  char cell = CELL(data, row, col);
  if (cell == '.')
    result = count_paths(data, row + 1, col);
  else {
    unsigned long long left = 0, right = 0;
    if (col > 0)
      left = count_paths(data, row + 1, col - 1);
    if (col < data->row_width - 1)
      right = count_paths(data, row + 1, col + 1);

    result = left + right;
  }

  memoized[row][col] = result;

  return result;
}

void p2(Data *data) 
{
  unsigned long long total = 0;

  char *s = strchr(data->data, 'S');
  size_t start_col = s - data->data;
  
  printf("P2: %llu\n", count_paths(data, 1, start_col));
}

int main(void)
{
  FILE *fp = fopen("data/day07.txt", "r");

  fseek(fp, 0, SEEK_END);
  size_t file_size = ftell(fp);
  rewind(fp);

  char *contents = malloc(file_size + 1);
  fread(contents, file_size, 1, fp);
  contents[file_size] = '\0';
  fclose(fp);

  char *nl = strchr(contents, '\n');
  if (!nl) {
    printf("hmm...there should have been a newline...\n");
    exit(-1);
  }

  // need to include \n in row/column calcs
  size_t rw = (size_t)(nl - contents) + 1; 
  size_t height = file_size / rw;

  Data data = { .data=contents, .row_width=rw, .height=height };
  
  p1(&data);
  p2(&data);

  free(contents);

  return 0;
}
