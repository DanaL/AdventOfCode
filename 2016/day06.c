#include <stdio.h>
#include <string.h>

#define NUM_OF_COLS 8

int main(void)
{  
  char buffer[1024];
  int table[NUM_OF_COLS][26];

  for (int j = 0; j < NUM_OF_COLS; j++)
  {
    for(int k =0; k < 26; k++)
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

  for (int j = 0; j < NUM_OF_COLS; j++) {
    char ch = '\0';
    int max = 0;

    for (int k = 0; k < 26; k++) {
       if (table[j][k] > max) {
        ch = k + 'a';
        max = table[j][k];
       }
    }

    printf("%c", ch);
  }
  
  printf("\n");
}