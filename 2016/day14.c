#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>

#include "utils.h"

//#define SALT "abc"
#define SALT "jlmsuwbz"

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

char *calc_hash(size_t index, size_t stretch)
{
  char buffer[100];
  sprintf(buffer, "%s%zu", SALT, index);

  for (int j = 0; j < stretch; j++) {
    char *s = md5(buffer);    
    strcpy(buffer, s);
    free(s);
  }
  
  char *hashed = malloc((strlen(buffer) + 1) * sizeof(char));
  strcpy(hashed, buffer);

  return hashed;
}

struct repeat *expand_table(struct repeat *hashes, size_t *hash_count, size_t curr_index, size_t stretch)
{
  char buffer[100];

  struct repeat last = hashes[*hash_count - 1];
  size_t index = last.index + 1;

  while (index <= curr_index + 1000) {
    char *hashed = calc_hash(index, stretch);
    char ch3 = repeat3(hashed);
    char ch5 = repeat5(hashed);
    free(hashed);

    if (ch3 != '\0' || ch5 != '\0') {
      hashes = realloc(hashes, ++(*hash_count) * sizeof(struct repeat));
      size_t i = *hash_count - 1;
      hashes[i].index = index;
      hashes[i].repeat_char = ch3;
      hashes[i].repeat_count = ch5 != '\0' ? 5 : 3;
    }

    ++index;
  }

  return hashes;
}

bool is_key(struct repeat *hashes, size_t hash_count, size_t i, char seeking, size_t index)
{
  while (i < hash_count && hashes[i].index < index + 1000) {
    if (hashes[i].repeat_count == 5 && hashes[i].repeat_char == seeking)
      return true;
    ++i;
  }

  return false;
}

void calc(size_t stretch)
{
  struct repeat *hashes = malloc(sizeof(struct repeat));
  
  size_t index = 0;
  // find first candidate
  do {
    char *hashed = calc_hash(index, stretch);
    char ch3 = repeat3(hashed);
    char ch5 = repeat5(hashed);
    free(hashed);
    
    if (ch3 != '\0' || ch5 != '\0') {
      hashes[0].index = index;
      hashes[0].repeat_char = ch3;
      hashes[0].repeat_count = ch5 != '\0' ? 5 : 3;
      break;
    }

    ++index;
  } while (true);

  size_t hash_count = 1;
  size_t key_candidate = 0;
  int keys_found = 0;
  
  do {
    // expand table if needed
    struct repeat r = hashes[key_candidate];
    hashes = expand_table(hashes, &hash_count, r.index, stretch);

    if (is_key(hashes, hash_count, key_candidate + 1, r.repeat_char, r.index)) {
      ++keys_found;
      if (keys_found == 64) {
        printf("done! %zu\n", r.index);
        break;
      }
    }
    
    ++key_candidate;
  } while (true);
  free(hashes);
}

int main(void)
{
  calc(1);
  // lol part 2 is extrmeley slow because I am doing so many malloc()s 
  // free()s while hashing the keys but I've little interest in making
  // it less dumb
  calc(2017);
}
