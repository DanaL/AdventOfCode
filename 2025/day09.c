#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <limits.h>

#define AREA(a, b) ((llabs((a)->x - (b)->x) + 1) * (llabs((a)->y - (b)->y) + 1))
#define MIN(a, b) ((a) < (b) ? (a) : (b))
#define MAX(a, b) ((a) > (b) ? (a) : (b))

typedef enum { INSIDE, OUTSIDE, ON_CORNER } SearchResult;
typedef enum { CORNER, HORIZONTAL_EDGE, VERTICAL_EDGE } EdgeType;

typedef struct {
  long long x;
  long long y;
  EdgeType type;
} Point;

typedef struct ht_node {
  Point pt;
  struct ht_node *next;
} HTNode;

long long lowest_y = LLONG_MAX, highest_y = 0, lowest_x = LLONG_MAX, highest_x = 0;

int cmp_by_y_then_x(const void *a, const void *b)
{
  Point *pa = (Point *)a;
  Point *pb = (Point *)b;
  
  if (pa->y != pb->y)
    return (pa->y > pb->y) - (pa->y < pb->y);  // Sort by y first
  
  return (pa->x > pb->x) - (pa->x < pb->x);    // Then by x
}

int cmp_by_x_then_y(const void *a, const void *b)
{
  Point *pa = (Point *)a;
  Point *pb = (Point *)b;
  
  if (pa->x != pb->x)
    return (pa->x > pb->x) - (pa->x < pb->x);
  
  return (pa->y > pb->y) - (pa->y < pb->y);
}

/* A quick, dirty hashtable implementation */
#define HASH_TABLE_CAPACITY 1000003
#define HASH(x, y) ((((x) * 73856093) ^ ((y) * 19349663)) % HASH_TABLE_CAPACITY)
bool ht_contains(HTNode **ht, Point *p)
{
  size_t hash = HASH(p->x, p->y);
  HTNode *node = ht[hash];

  while (node) {
    if (node->pt.x == p->x && node->pt.y == p->y) {
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
  node->pt = *p;
  node->next = ht[hash];
  ht[hash] = node;
}

HTNode* ht_get(HTNode **ht, Point *p)
{
  size_t hash = HASH(p->x, p->y);
  HTNode *node = ht[hash];
  while (node) {
    if (node->pt.x == p->x && node->pt.y == p->y)
      return node;
    node = node->next;
  }

  return NULL;
}

void ht_free_elts(HTNode **ht)
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

void p1(const Point *pts, const size_t num_pts)
{
  unsigned long long largest_area = 0;
  for (size_t j = 0; j < num_pts; j++) {
    for (size_t k = j + 1; k < num_pts; k++) {
      unsigned long long area = AREA(&pts[j], &pts[k]);
      if (area > largest_area)
        largest_area = area;
    }
  }

  printf("P1: %lld\n", largest_area);
}

void print_grid(HTNode **ht)
{
  for (size_t r = 0; r < 30; r++) {
    for (size_t c = 0;c < 30; c++) {
      Point pt = { .x=c, .y=r };
      HTNode *n = ht_get(ht, &pt);
      if (!n)
        printf(".");
      else if (n->pt.type == CORNER)
        printf("+");
      else if (n->pt.type == HORIZONTAL_EDGE)
        printf("-");
      else if (n->pt.type == VERTICAL_EDGE)
        printf("|");        
    }
    printf("\n");
  }
}

SearchResult check_north(HTNode **ht, Point *pt)
{  
  Point p = { .x = pt->x };
  int crossings = 0;
  for (long long y = pt->y; y >= lowest_y; y--) {
    p.y = y;

    HTNode *node = ht_get(ht, &p);
    
    if (node && node->pt.type == CORNER) {
      return ON_CORNER;
    }
    else if (node && node->pt.type == HORIZONTAL_EDGE) {
      ++crossings;
    }
  }

  return crossings % 2 == 0 ? OUTSIDE : INSIDE;
}

SearchResult check_south(HTNode **ht, Point *pt)
{  
  Point p = { .x = pt->x };
  int crossings = 0;
  for (long long y = pt->y; y <= highest_y; y++) {
    p.y = y;

    HTNode *node = ht_get(ht, &p);
    
    if (node && node->pt.type == CORNER) {
      return ON_CORNER;
    }
    else if (node && node->pt.type == HORIZONTAL_EDGE) {
      ++crossings;
    }
  }

  return crossings % 2 == 0 ? OUTSIDE : INSIDE;
}

SearchResult check_east(HTNode **ht, Point *pt)
{  
  Point p = { .y = pt->y };
  int crossings = 0;
  for (long long x = pt->x; x <= highest_x; x++) {
    p.x = x;

    HTNode *node = ht_get(ht, &p);
    
    if (node && node->pt.type == CORNER) {
      return ON_CORNER;
    }
    else if (node && node->pt.type == VERTICAL_EDGE) {
      ++crossings;
    }
  }

  return crossings % 2 == 0 ? OUTSIDE : INSIDE;
}

SearchResult check_west(HTNode **ht, Point *pt)
{  
  Point p = { .y = pt->y };
  int crossings = 0;
  for (long long x = pt->x; x >= lowest_x; x--) {
    p.x = x;

    HTNode *node = ht_get(ht, &p);
    
    if (node && node->pt.type == CORNER) {
      return ON_CORNER;
    }
    else if (node && node->pt.type == VERTICAL_EDGE) {
      ++crossings;
    }
  }

  return crossings % 2 == 0 ? OUTSIDE : INSIDE;
}

bool pt_in_polygon(HTNode **ht, Point *pt)
{
  if (ht_contains(ht, pt))
    return true;

  SearchResult result = check_north(ht, pt);
  if (result == ON_CORNER)
    result = check_south(ht, pt);
  if (result == ON_CORNER)
    result = check_east(ht, pt);
  if (result == ON_CORNER)
    result = check_west(ht, pt);
  
  return result == INSIDE;
}

bool rect_contained(HTNode **ht, Point *a, Point *b)
{  
  long long low_y = MIN(a->y, b->y), low_x = MIN(a->x, b->x);
  long long hi_y = MAX(a->y, b->y), hi_x = MAX(a->x, b->x);
  
  Point p;
  for (long long y = low_y; y <= hi_y; y++) {
    
    p.x = low_x;
    p.y = y;
    if (!pt_in_polygon(ht, &p))
      return false;

    p.x = hi_x;
    if (!pt_in_polygon(ht, &p))
      return false;
  }

  for (long long x = low_x; x <= hi_x; x++) {
    p.y = low_y;
    p.x = x;
    if (!pt_in_polygon(ht, &p))
      return false;

    p.y = hi_y;
    if (!pt_in_polygon(ht, &p))
      return false;
  }

  return true;
}

void p2(Point *pts, size_t num_pts)
{
  HTNode **ht = calloc(HASH_TABLE_CAPACITY, sizeof(HTNode*));

  for (size_t j = 0; j < num_pts; j++) {
    ht_insert(ht, &pts[j]);
    if (pts[j].y < lowest_y)
      lowest_y = pts[j].y;
    if (pts[j].y > highest_y)
      highest_y = pts[j].y;
    if (pts[j].x < lowest_x)
      lowest_x = pts[j].x;
    if (pts[j].x > highest_x)
      highest_x = pts[j].x;
  }

  // Draw horizontal line segments
  qsort(pts, num_pts, sizeof(Point), cmp_by_y_then_x);  
  for (size_t j = 0; j < num_pts - 1; j += 2) {
    long long y =  pts[j].y;
    long long lo_x = MIN(pts[j].x, pts[j+1].x);
    long long hi_x = MAX(pts[j].x, pts[j+1].x);
    
    for (long long x = lo_x; x <= hi_x; x++) {
      Point pt = { .x = x, .y = y, .type = HORIZONTAL_EDGE };
      ht_insert(ht, &pt);
    }
  }

  // Draw vertical line segments
  qsort(pts, num_pts, sizeof(Point), cmp_by_x_then_y);  
  for (size_t j = 0; j < num_pts - 1; j += 2) {
    long long x =  pts[j].x;
    long long lo_y = MIN(pts[j].y, pts[j+1].y);
    long long hi_y = MAX(pts[j].y, pts[j+1].y);
    
    for (long long y = lo_y; y <= hi_y; y++) {
      Point pt = { .x = x, .y = y, .type = VERTICAL_EDGE };
      ht_insert(ht, &pt);
    }
  }
  
  //print_grid(ht);

  unsigned long long largest_area = 0;
  for (size_t j = 0; j < num_pts; j++) {
    for (size_t k = j + 1; k < num_pts; k++) {      
      if (rect_contained(ht, &pts[j], &pts[k])) {
        unsigned long long area = AREA(&pts[j], &pts[k]);
        if (area > largest_area)
          largest_area = area;
      }
    }
  }

  printf("P2: %llu\n", largest_area);

  // Point pp = { .x=9, .y=3 };
  // printf("hmm %d\n", ht_contains(ht, &pp));

  // print_grid(ht);
  // Point a = { .x=16, .y=0};
  // Point b = { .x=18, .y=9};
  // printf("\n?? %d\n", rect_contained(ht, &a, &b));
  // printf("area %lld\n", AREA(&a, &b));
  
  ht_free_elts(ht);
  free(ht);
}

#define BUFF_LEN 128
#define LIST_LEN 1024

int main(void)
{
  FILE *fp = fopen("data/day09.txt", "r");

  Point pts[LIST_LEN];
  size_t num_pts = 0;

  char buffer[BUFF_LEN];
  long long x, y;
  while (fgets(buffer, BUFF_LEN, fp)) {
    sscanf(buffer, "%lld,%lld", &x, &y);
    pts[num_pts].x = x;
    pts[num_pts].y = y;
    pts[num_pts].type = CORNER;
    ++num_pts;
  }
  fclose(fp);

  p1(pts, num_pts);
  p2(pts, num_pts);

  return 0;
}
