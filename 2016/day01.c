// Welcome to me practicing some C by working through Advent of Code problems!
// I've following Beej's Guide to C Programming and will probably also bust
// out K&R. In the meantime, I've begun coding. In theory I know enough to do
// this problem with dynamic allocation and part 2 is crying out for a 
// hash table but I've relearned enough about malloc and pointers
#include <stdio.h>
#include <stdbool.h>
#include <stdlib.h>
#include <string.h>

#define BUFF_SIZE 1024
#define VT_TABLE_SIZE 10000

#define NORTH 0
#define SOUTH 1
#define EAST 2
#define WEST 3

struct point {
  int x, y;
};

void clear_str(char *s, int length);
int taxi_d(int x, int y);
int turn(int dir, char turn);
bool check_visited(struct point *arr, int length, struct point target);

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

// I don't want to implement a real hash table so I'm just going to accumulate
// a list of visited points and loop over it entirely while checking for 
// previously visited locations
void p2(void)
{
  struct move moves[BUFF_SIZE];
  int count;
  load_moves(moves, &count);

  // who needs a hash table...
  struct point visited[VT_TABLE_SIZE];
  visited[0].x = 0;
  visited[0].y = 0;

  struct point curr = { .x = 0, .y = 0 };
  int dir = NORTH, curr_move = 0, vc = 1;
  struct point delta;
  do {
    dir = turn(dir, moves[curr_move].turn);
    if (dir == NORTH) {
      delta.x = 0;
      delta.y = -1;
    }
    else if (dir == SOUTH) {
      delta.x = 0;
      delta.y = 1;
    }
    else if (dir == EAST) {
      delta.x = 1;
      delta.y = 0;
    }
    else if (dir == WEST) {
      delta.x = -1;
      delta.y = 0;
    }

    for (int d = 0; d < moves[curr_move].distance; d++) {
      curr.x += delta.x;
      curr.y += delta.y;
      if (check_visited(visited, vc , curr)) {
          printf("P2: %d\n", taxi_d(curr.x, curr.y));
          return;
      }
      visited[vc++] = curr;
    }

    ++curr_move;
    if (curr_move >= count)
      curr_move = 0;
  }
  while (true);
}

int main(void)
{
  p1();
  p2();
}

bool check_visited(struct point *arr, int length, struct point target)
{
  for (int j = 0; j < length; j++) {
    if (arr[j].x == target.x && arr[j].y == target.y)
      return true;
  }

  return false;
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
