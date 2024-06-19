#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>

#include "utils.h"

#define SALT "abc"
//#define SALT "jlmsuwbz"

struct repeat {
  size_t index;
  char repeat_char;
  int repeat_count;
};

char repeat3(char *s) 
{
  for (size_t j = 0; j < strlen(s) - 2; j++) {
    char ch = s[j];
    if (s[j+1] == ch && s[j+2] == ch)
      return ch;
  }

  return '\0';
}

char repeat5(char *s) {
  for (size_t j = 0; j < strlen(s) - 4; j++) {
    char ch = s[j];
    if (s[j+1] == ch && s[j+2] == ch && s[j+3] == ch && s[j+4] == ch)
      return ch;
  }

  return '\0';
}

struct repeat *expand_table(struct repeat *hashes, size_t *hash_count, size_t curr_hash, size_t *curr_index)
{
  char buffer[100];
  size_t index = hashes[curr_hash].index;
  for (size_t i = curr_hash + 1; i < *hash_count; i++) {
    if (hashes[i].index - *curr_index > 1000)
      return hashes; // we don't need to further expand the table
    index = hashes[i].index;
  }
  ++index;

  while (index <= *curr_index + 1000) {
    sprintf(buffer, "%s%zu", SALT, index);
    char *s = md5(buffer);    
    char ch3 = repeat3(s);
    char ch5 = repeat5(s);
    free(s);

    if (ch3 != '\0' || ch5 != '\0') {
      hashes = realloc(hashes, ++(*hash_count) * sizeof(struct repeat));
    
      size_t i = *hash_count - 1;
      hashes[i].index = index;
      hashes[i].repeat_char = ch3;
      hashes[i].repeat_count = ch5 != '\0' ? 5 : 3;
    }

    ++index;
  }

  *curr_index = index;

  return hashes;
}

void p1(void)
{
  char buffer[100];
  struct repeat *hashes = malloc(sizeof(struct repeat));
  
  size_t index = 0;
  // find first candidate
  do {
    sprintf(buffer, "%s%zu", SALT, index);
    char *s = md5(buffer);    
    char ch3 = repeat3(s);
    char ch5 = repeat5(s);
    free(s);

    if (ch3 != '\0' || ch5 != '\0') {
      hashes[0].index = index;
      hashes[0].repeat_char = ch3;
      hashes[0].repeat_count = ch5 != '\0' ? 5 : 3;
      break;
    }

    ++index;
  } while (true);

  size_t hash_count = 1;
  printf("foo %zu %c\n", hashes[0].index, hashes[0].repeat_char);
  printf("flag %zu\n", index);
  size_t key_candidate = 0;
  int keys_found = 0;
  
  do {
    // expand table
    hashes = expand_table(hashes, &hash_count, key_candidate, &index);
    // search table to see if key

    // move to next candidate (ie., inc key_candidate)
    break;
  } while (true);
  
  printf("hc %zu \n", hash_count);
  printf("index %zu \n", index);
  for (int i = 0; i < hash_count; i++) {
    printf("hash %d %lu %d %c\n", i, hashes[i].index, hashes[i].repeat_count, hashes[i].repeat_char);
  }
  
  
  free(hashes);
}

int main(void)
{
  p1();
}
