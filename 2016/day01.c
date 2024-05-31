#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define BUFF_SIZE 1024

#define NORTH 0
#define SOUTH 1
#define EAST 2
#define WEST 3

void clear_str(char* s, int length);
int taxi_d(int x, int y);
int turn(int dir, char turn);

void p1(void) 
{
  FILE *fp;
  char s[BUFF_SIZE];
  char num[8];

  fp = fopen("inputs/day01.txt", "r");
  fgets(s, sizeof s, fp);
  fclose(fp);

  int c = 0, j, val;
  int x = 0, y = 0, dir = NORTH;
  do {
    // skip to next move
    while (s[c] != '\0' && s[c] != 'R' && s[c] != 'L') 
      ++c;

    if (s[c] == '\0')
      break;
    
    j = c + 1;
    while (s[j] != ',' && s[j] != '\0')
      j++;

    clear_str(num, 8);
    strncpy(num, s + c + 1, j - c - 1);
    sscanf(num, "%d", &val);

    dir = turn(dir, s[c]);
    if (dir == NORTH)
      y -= val;
    else if (dir == SOUTH)
      y += val;
    else if (dir == EAST)
      x += val;
    else if (dir == WEST)
      x -= val;

    c = j + 1;
  }
  while (c < BUFF_SIZE && s[c] != '\0');

  printf("P1: %d\n", taxi_d(x, y));
}

int main(void)
{
  p1();
}

void clear_str(char* s, int length)
{
  for (int j = 0; j < length; j++)
    s[j] = '\0';
}

// For this problem, I am always calculating from (0, 0)
int taxi_d(int x, int y)
{
  return abs(x) + abs(y);
}

int turn(int dir, char turn)
{
  switch (dir) {
    case NORTH:
      return turn == 'R' ? EAST : WEST;
    case SOUTH:
      return turn == 'R' ? WEST : EAST;
    case EAST:
      return turn == 'R' ? SOUTH : NORTH;
    default: // WEST
      return turn == 'R' ? NORTH : SOUTH;
  };
}
