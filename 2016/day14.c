#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>

#include "utils.h"

struct repeat {
  size_t index;
  char repeat_char;
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

int count_keys(struct repeat *hashes, size_t hash_count, size_t i, char ch, int curr_total)
{  
  printf("checking %zu %c\n", i, ch);
  int start_index = hash_count - 1;
  do
  {
   if (i - hashes[start_index].index > 1000) 
    break;
    start_index--;
  } while (start_index > 0);
  printf("  si %d\n", start_index);

  for (int j = start_index; j < hash_count; j++) {
    if (hashes[j].repeat_char == ch) {
      ++curr_total;
      if (curr_total == 64) {
        printf("P1: %zu\n", hashes[j].index);
        break;
      }
    }
  }

  return curr_total;
}

// I think what should work is keep generating hashes and record
// the ones with 3 repeated characters. When I find one with 5
// repeated charaters, work backwards and find its index.
// Or: is there ever a 5-repeater that doesn't have a 3-repeat within
// 1000 characters? Do I just need to find the 64th 5-repeater and then
// work backwards to find the corresponding key?
void p1(void)
{
  char *salt = "jlmsuwbz";
  //char *salt = "abc";
  char buffer[100];
  struct repeat *hashes = NULL;
  size_t hash_count = 0;  
  int total_keys = 0;

  size_t j = 0;
  while (true) {
    sprintf(buffer, "%s%zu", salt, j);
    char *s = md5(buffer);    
    char ch3 = repeat3(s);
    char ch5 = repeat5(s);
    free(s);

    if (ch5 != '\0') { 
      total_keys = count_keys(hashes, hash_count, j, ch5, total_keys);
      printf("total: %d\n", total_keys);
      if (total_keys >= 64) {
        printf("kf %d\n", total_keys);
        for (int k = hash_count - 1; k >= 0; k--) {
          if (hashes[k].repeat_char == ch5) {
            printf("P1: %zu\n", hashes[k].index);
            break;
          }
        }
        break;
      }      
    }
    
    int ch = ch3 != '\0' ? ch3 : ch5;
    if (ch)
    {
      hashes = realloc(hashes, ++hash_count * sizeof(struct repeat));
      size_t i = hash_count - 1;
      hashes[i].index = j;
      hashes[i].repeat_char = ch;
    }

    ++j;

    //if (j > 30000) break;
  }

  //printf("%d %zu\n", j, hash_count);

  free(hashes);
}

int main(void)
{
  p1();
}
