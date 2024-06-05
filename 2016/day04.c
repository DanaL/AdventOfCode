#include <stdio.h>
#include <stdbool.h>
#include <string.h>
#include <ctype.h>

int check_line(char *line)
{
  char *ch = line;
  int table[26] = { 0 };

  // First, scan the encrypted name of the room
  while (*ch != '\0' && !isdigit(*ch)) {
    if (*ch >= 'a' && *ch <= 'z') {
      int i = *ch - 'a';
      table[i]++;
    }
    ++ch;
  }

  // Next, scan the sectorID
  int sector_id = 0;
  while (isdigit(*ch)) {
    sector_id = sector_id * 10 + *ch - '0';
    ++ch;
  }
  
  ++ch; // skip the [ character

  char checksum[5];
  memcpy(checksum, ch, 5);

  char freqs[26];
  for (int j = 0; j < 26; j++) {
    freqs[j] = 'a' + j;
  }

  // Bubble sort time!
  for (int j = 0; j < 26; j++) {
    for (int k = j + 1; k < 26; k++) {
      char a = freqs[j];
      char b = freqs[k];
      int freq_a = table[a - 'a'];
      int freq_b = table[b - 'a'];

      // We want to sort by frequency count highest-to-lowest
      // and break ties by alphabetical order
      if (freq_b > freq_a)
      {
        // swap!
        freqs[j] = b;
        freqs[k] = a;
      }
      else if (freq_b == freq_a && a > b)
      {
        freqs[j] = b;
        freqs[k] = a;
      }
    }
  }

  bool valid = true;
  for (int j = 0; j < 5; j++) {
    if (freqs[j] != checksum[j]) {
      valid = false;
      break;
    }
  }

  return valid ? sector_id : 0;
}

int main(void)
{
  FILE *fp;
  char line[100];

  int sector_id_totals = 0;
  fp = fopen("inputs/day04.txt", "r");
  while (fgets(line, sizeof line, fp) != NULL) {
    sector_id_totals += check_line(line);
  }
  fclose(fp);

  printf("P1: %d\n", sector_id_totals);
}
