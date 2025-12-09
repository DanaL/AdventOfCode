#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>

#define AREA(a, b) llabs(((a)->x - (b)->x + 1) * ((a)->y - (b)->y + 1))

typedef struct {
  long x;
  long y;
} Point;

typedef struct ht_node {
  Point *pt;
  struct ht_node *next;
} HTNode;

/* A quick, dirty hashtable implementation */
#define HASH_TABLE_CAPACITY 1000003
#define HASH(x, y) ((((x) * 73856093) ^ ((y) * 19349663)) % HASH_TABLE_CAPACITY)
#define CMP_PT(a, b) ((a)->x == (b)->x && (a)->y == (b)->y)
bool ht_contains(HTNode **ht, Point *p)
{
  size_t hash = HASH(p->x, p->y);
  HTNode *node = ht[hash];

  while (node) {
    if (CMP_PT(node->pt, p)) {
      return true;
    }

    node = node->next;
  }

  return false;
}

void ht_insert(HTNode **ht, Point *p)
{  
  if (ht_contains(ht, p))
    return;

  size_t hash = HASH(p->x, p->y);
  HTNode *node = malloc(sizeof(HTNode));
  node->pt = p;
  node->next = ht[hash];
  ht[hash] = node;
}

void ht_free(HTNode **ht)
{
  for (size_t i = 0; i < HASH_TABLE_CAPACITY; i++) {
    HTNode *node = ht[i];
    while (node) {
      HTNode *temp = node;
      node = node->next;
      free(temp);
    }
  }
}

void p1(Point *pts, size_t num_pts)
{
  long long largest_area = 0;
  for (size_t j = 0; j < num_pts; j++) {
    for (size_t k = j + 1; k < num_pts; k++) {
      long long area = AREA(&pts[j], &pts[k]);
      if (area > largest_area)
        largest_area = area;
    }
  }

  printf("P1: %lld\n", largest_area);
}

void p2(Point *pts, size_t num_pts)
{
  HTNode *ht[HASH_TABLE_CAPACITY] = {0};
  
  Point nw_est = pts[0];
  for (size_t j = 0; j < num_pts; j++) {
    ht_insert(ht, &pts[j]);

    if (pts[j].y < nw_est.y || (pts[j].y == nw_est.y && pts[j].x < nw_est.x))
      nw_est = pts[j];
  }

  Point p = { .x = 88676, .y = 18691};

  printf("%d\n", ht_contains(ht, &pts[0]));
  printf("%d\n", ht_contains(ht, &pts[1]));
  printf("%d\n", ht_contains(ht, &p));
  printf("%d\n", ht_contains(ht, &pts[400]));

  ht_free(ht);
}

#define BUFF_LEN 128
#define LIST_LEN 1024

int main(void)
{
  FILE *fp = fopen("data/day09.txt", "r");

  Point pts[LIST_LEN];
  size_t num_pts = 0;

  char buffer[BUFF_LEN];
  long x, y;
  while (fgets(buffer, BUFF_LEN, fp)) {
    sscanf(buffer, "%ld,%ld", &x, &y);
    pts[num_pts].x = x;
    pts[num_pts].y = y;
    ++num_pts;
  }
  fclose(fp);

  p1(pts, num_pts);
  p2(pts, num_pts);

  return 0;
}
