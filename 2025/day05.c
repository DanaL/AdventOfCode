#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdint.h>

#define BUFF_LEN 128

typedef struct node_t {
  uint64_t lower;
  uint64_t upper;
  struct node_t *prev;
  struct node_t *next;
} Node;

Node* make_node(uint64_t a, uint64_t b)
{
  Node *n = malloc(sizeof(Node));
  if (!n) {
    return NULL;
  }

  n->lower = a;
  n->upper = b;
  n->prev = NULL;
  n->next = NULL;

  return n;
}

Node* insert(Node *head, Node *n)
{
  if (head == NULL)
    return n;

  if (n->upper < head->lower) {
    n->next = head;
    head->prev = n;
  }

  Node *curr = head;
  while (curr) {
    if (n->upper >= curr->lower && n->upper <= curr->upper) {
      // merge n and curr
      // free n
      // return head
    }
    else if (n->upper < curr->lower) {
      n->next = curr;
      n->prev = curr->prev;

      curr->prev->next = n;      
      curr->prev = n;

      // check for merge with prev?
      
      return head;      
    }
    else if (!curr->next) {
      curr->next = n;
      n->prev = curr;

      return head;
    }

    curr = curr->next;
  }

  return NULL; // actually an error condition later
}

void print_list(Node *head)
{
  printf("\nListy list:\n");
  Node *n = head;
  while (n) {
    printf("  %llu - %llu\n", n->lower, n->upper);
    n = n->next;
  }
}

void free_list(Node *head) 
{
  Node *n = head;
  while (n) {
    Node *c = n;
    n = n->next;
    free(c);
  }
}

int search(const Node *head, uint64_t v) {
  Node *curr = head;

  while (curr) {
    if (v >= curr->lower && v <= curr->upper)
      return 1;
    curr = curr->next;
  }

  return 0;
}

int main(void)
{
  FILE *fp = fopen("data/day05.txt", "rb");
  char buffer[BUFF_LEN];

  int read_state = 0;
  Node *head = NULL, *curr;
  int p1 = 0;
  while (fgets(buffer, BUFF_LEN, fp)) {    
    if (buffer[0] == '\n' || buffer[0] == '\r') {
      read_state = 1;
    }
    else if (read_state == 0) {
      uint64_t a, b;
      sscanf(buffer, "%llu-%llu", &a, &b);
      
      Node *n = make_node(a, b);
      
      if (!head) {
        head = n;
        curr = n; 
      }
      else {
        curr->next = n;
        curr = n;
      }
      
      //head = insert(head, n);
    }
    else {
      uint64_t v = strtoull(buffer, NULL, 10);
      p1 += search(head, v);
    }
  }

  printf("P1: %d\n", p1);

  //printf("%llu %llu\n", head->lower, head->upper);

  //Node *n0 = make_node(6, 8);
  //head = insert(head, n0);
  //print_list(head);

  free_list(head);

  fclose(fp);

  return 0;
}
