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

struct move {
  int distance;
  char turn;
};

void load_moves(struct move *moves, int *count) 
{
  FILE *fp;
  char s[BUFF_SIZE];
  char num[8];

  fp = fopen("inputs/day01.txt", "r");
  fgets(s, sizeof s, fp);
  fclose(fp);

  int c = 0, j, val;
  int m = 0;
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

    moves[m].distance = val;
    moves[m].turn = s[c];
    ++m;

    c = j + 1;
  }
  while (c < BUFF_SIZE && s[c] != '\0');

  *count = m;
}

void p1(void)
{
  struct move moves[BUFF_SIZE];
  int count;

  load_moves(moves, &count);

  int dir = NORTH, x = 0, y = 0;
  for (int j = 0; j < count; j++) {
    dir = turn(dir, moves[j].turn);
    if (dir == NORTH)
      y -= moves[j].distance;
    else if (dir == SOUTH)
      y += moves[j].distance;
    else if (dir == EAST)
      x += moves[j].distance;
    else if (dir == WEST)
      x -= moves[j].distance;
  }

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
