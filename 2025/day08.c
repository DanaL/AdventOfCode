#include <stdio.h>
#include <stdlib.h>
#include <math.h>

#define BUFF_LEN 128
#define LIST_LEN 1024

typedef struct {
  long long x;
  long long y;
  long long z;
} Point;

static inline int distance(const Point *a, const Point *b)
{
    long long dx = a->x - b->x;
    long long dy = a->y - b->y;
    long long dz = a->z - b->z;

    return (int)sqrt((double)(dx*dx + dy*dy + dz*dz));
}

typedef struct {
  size_t a;
  size_t b;
  int d;
} Pair;

static inline int cmp_pairs(const void* a, const void* b)
{
  Pair *p1 = (Pair *)a;
  Pair *p2 = (Pair *)b;

  return p1->d - p2->d;
}

int find(int *circuits, int a)
{
  if (circuits[a] < 0)
    return a;
  else
    return circuits[a] = find(circuits, circuits[a]);
}

void union_set(int *circuits, int a, int b)
{
  int root_a = find(circuits, a);
  int root_b = find(circuits, b);

  if (root_a != root_b)
    circuits[root_b] = root_a;
}

// Global variables, so sweet and convenient
Point points[LIST_LEN];
size_t num_pts = 0;

size_t num_pairs;
Pair *pairs;

void p1(void)
{    
  int *circuits = malloc(num_pts * sizeof(int));
  for (size_t j = 0; j < num_pts; j++) {
    circuits[j] = -1;
  }

  for (size_t j = 0; j < 1000; j++) {
    union_set(circuits, pairs[j].a, pairs[j].b);
  }

  // count up the sizes of the sets (circuits)
  int *set_sizes = calloc(num_pts, sizeof(int));
  for (size_t j = 0; j < num_pts; j++) {
    int root = find(circuits, j);
    set_sizes[root]++;
  }

  int largest[3] = {1, 1, 1};
  for (size_t j = 0; j < num_pts; j++) {
    if (set_sizes[j] > largest[0]) {
      largest[2] = largest[1];
      largest[1] = largest[0];
      largest[0] = set_sizes[j];
    } else if (set_sizes[j] > largest[1]) {
      largest[2] = largest[1];
      largest[1] = set_sizes[j];
    } else if (set_sizes[j] > largest[2]) {
      largest[2] = set_sizes[j];
    }
  }

  unsigned long long total_sets = 1;
  for (size_t j = 0; j < 3; j++) {
    total_sets *= largest[j];
  }

  printf("P1: %llu\n", total_sets);

  free(set_sizes);  
  free(circuits);  
}

int count_distinct_sets(int *circuits)
{
  int result = 0;
  for (size_t j = 0; j < num_pts; j++) {
    if (circuits[j] == -1)
      ++result;
  }

  return result;
}

void p2(void)
{
  int *circuits = malloc(num_pts * sizeof(int));
  for (size_t j = 0; j < num_pts; j++) {
    circuits[j] = -1;
  }

  size_t j = 0;
  while (count_distinct_sets(circuits) > 1) {
    union_set(circuits, pairs[j].a, pairs[j].b);    
    ++j;
  }
  --j;
  
  long long last_x1 = points[pairs[j].a].x;
  long long last_x2 = points[pairs[j].b].x;
  printf("P2: %lld\n", last_x1 * last_x2);  
}

int main(void)
{
  char buffer[BUFF_LEN];
  FILE *fp = fopen("data/day08.txt", "r");
  long long x, y, z;
  while (fgets(buffer, BUFF_LEN, fp)) {
    sscanf(buffer, "%lld,%lld,%lld", &x, &y, &z);
    points[num_pts].x = x;
    points[num_pts].y = y;
    points[num_pts].z = z;
    ++num_pts;
  }
  fclose(fp);

  num_pairs = (num_pts * (num_pts - 1)) / 2;
  pairs = malloc(num_pairs * sizeof(Pair));
  
  size_t num_of_pairs = 0;
  for (size_t i = 0; i < num_pts - 1; i++) {
      for (size_t j = i + 1; j < num_pts; j++) {
          pairs[num_of_pairs].a = i;
          pairs[num_of_pairs].b = j;
          pairs[num_of_pairs].d = distance(&points[i], &points[j]);
          num_of_pairs++;
      }
  }

  qsort(pairs, num_of_pairs, sizeof(Pair), cmp_pairs);

  p1();
  p2();

  free(pairs);

  return 0;
}