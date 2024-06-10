#include <stdio.h>
#include <stdlib.h>
#include <string.h>

char *calc_decomp(char *p, int *char_count, int *reps)
{
  char *n = ++p;

  while (*n != ')')
    ++n;

  int d = n - p;
  char buffer[50];
  strncpy(buffer, p, d);
  buffer[d] = '\0';
  sscanf(buffer, "%dx%d", char_count, reps);

  return ++n;
}

// returns a string that's the next_char_count characters in s
// repeated reps # of times
char *expand(char *s, int char_count, int reps)
{
  char *ns = malloc(char_count * reps + 1);

  int x = 0;
  for (int j = 0; j < reps; j ++) {
    for (int k = 0; k < char_count; k++) {
      ns[x++] = s[k];
    }
  }
  ns[x] = '\0';

  return ns;
}

char *decompress(char *src)
{
  int total_length = 0;
  char *decompressed = NULL;
  char *prev = src, *curr = src;
  do {
    while (*curr != '(' && *curr != '\0')
      ++curr;
    
    if (curr > prev)
    {
      int segment_length = curr - prev;
      total_length += curr - prev;
      decompressed = realloc(decompressed, total_length + 1);
      int pos = strlen(decompressed);
      strncpy(&decompressed[pos], prev, segment_length);
      pos += segment_length;
      decompressed[pos] = '\0';
      total_length = strlen(decompressed);
    }

    if (*curr == '(') {
      int char_count = 0, reps = 0;
      char *s2 = calc_decomp(curr, &char_count, &reps);

      char *tmp = decompressed;
      char *s3 = expand(s2, char_count, reps);
      total_length += strlen(s3);
      decompressed = realloc(decompressed, total_length + 1);
      if (tmp == NULL)
        tmp = decompressed;
      else
        tmp = &decompressed[strlen(decompressed)];
      strcpy(tmp, s3);
      decompressed[total_length] = '\0';
      free(s3);

      curr = s2 + char_count;
      prev = curr;
    }
    else if (*curr == '\0') {
      break;
    }    
  } while (1);

  return decompressed;
}

void p1(char *txt)
{
  char *decompressed = decompress(txt);
  printf("P1: %lu\n", strlen(decompressed));
  free(decompressed);  
}

unsigned long calc_decompressed_len(char *txt)
{
  unsigned long total = 0;
  char *c = txt;

  while (*c != '\0') {
    if (*c == '(') {
      int char_count = 0, reps = 0;
      char *s2 = calc_decomp(c, &char_count, &reps);
      
      char *sub_s = malloc(char_count * sizeof(char) + 1);
      strncpy(sub_s, s2, char_count);
      sub_s[char_count] = '\0';
  
      total += reps * calc_decompressed_len(sub_s);
      c = s2 + char_count;
      free(sub_s);      
    }
    else {
      ++total;
      ++c;
    }    
  }

  return total;
}

void p2(char *txt)
{
  unsigned long total = calc_decompressed_len(txt);
  printf("P2: %lu\n", total);
}

int main(void)
{
  FILE *fp = fopen("inputs/day09.txt", "r");
  fseek(fp, 0L, SEEK_END);
  long bytes = ftell(fp);
  fseek(fp, 0L, SEEK_SET);

  char *txt = calloc(bytes, sizeof(char));
  fread(txt, sizeof(char), bytes, fp);  
  fclose(fp);

  p1(txt);
  p2(txt);

  free(txt);
}