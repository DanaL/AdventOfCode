#include <stdio.h>
#include <limits.h>
#include <string.h>

#define NUM_OF_COLS 8

int main(void)
{  
  char buffer[1024];
  int table[NUM_OF_COLS][26];

  for (int j = 0; j < NUM_OF_COLS; j++)
  {
    for(int k = 0; k < 26; k++)
    {
      table[j][k] = 0;
    }
  }

  FILE *fp = fopen("inputs/day06.txt", "r");
  while (fgets(buffer, sizeof buffer, fp) != NULL) {
    buffer[strlen(buffer) - 1] = '\0';
    for (int j = 0; j < strlen(buffer); j++)
    {
      int i = buffer[j] - 'a';
      ++table[j][i];
    }
  }  
  fclose(fp);

  char p1[NUM_OF_COLS + 1];
  p1[NUM_OF_COLS] = '\0';
  char p2[NUM_OF_COLS + 1];
  p2[NUM_OF_COLS] = '\0';

  for (int j = 0; j < NUM_OF_COLS; j++) {
    char ch_p1 = '\0', ch_p2 = '\0';
    int max = 0, min = INT_MAX;

    for (int k = 0; k < 26; k++) {
       if (table[j][k] > max) {
        ch_p1 = k + 'a';
        max = table[j][k];
       }

       if (table[j][k] > 0 && table[j][k] < min)
       {
        ch_p2 = k + 'a';
        min = table[j][k];
       }
    }

    p1[j] = ch_p1;
    p2[j] = ch_p2;
  }
  
  printf("P1: %s\n", p1);
  printf("P2: %s\n", p2);
}