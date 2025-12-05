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

void free_list(Node *head) 
{
  Node *n = head;
  while (n) {
    Node *c = n;
    n = n->next;
    free(c);
  }
}

Node* insert(Node *head, Node *n)
{
  if (head == NULL) {    
    return n;
  }

  Node *curr = head;
  while (curr) {
    if (n->lower < curr->lower) {
      if (curr == head) {
        n->next = curr;
        curr->prev = n;
        head = n;
      }
      else {
        Node *prev = curr->prev;
        n->next = curr;
        n->prev = prev;
        prev->next = n;
        curr->prev = n;
      }

      return head;
    }
    else if (!curr->next) {
      curr->next = n;
      n->prev = curr;
      return head;
    }

    curr = curr->next;
  }

  return NULL; // actually would bean error condition
}

Node* merge(Node *head)
{
  Node *curr = head;

  while (curr && curr->next) {
    Node *next = curr->next;

    if (curr->upper >= next->lower) {
      // merge with the next in the list and drop/free curr
      if (next->upper > curr->upper) {
        curr->upper = next->upper;
      }
      if (next->lower < curr->lower) {
        curr->lower = next->lower;
      }

      curr->next = next->next;
      if (next->next) {
        next->next->prev = curr;
      }

      free(next);
    }
    else {
      curr = next;
    }
  }

  return head;
}

int p1_check(const Node *head, uint64_t v) {
  const Node *curr = head;

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
  Node *head = NULL;
  int p1 = 0;
  while (fgets(buffer, BUFF_LEN, fp)) {    
    if (buffer[0] == '\n' || buffer[0] == '\r') {
      read_state = 1;
    }
    else if (read_state == 0) {
      uint64_t a, b;
      sscanf(buffer, "%llu-%llu", &a, &b);
      
      Node *n = make_node(a, b);            
      head = insert(head, n);
    }
    else {
      uint64_t v = strtoull(buffer, NULL, 10);
      p1 += p1_check(head, v);
    }
  }

  printf("P1: %d\n", p1);

  head = merge(head);

  uint64_t p2 = 0;
  Node *n = head;
  while (n) {
    p2 += n->upper - n->lower + 1;
    n = n->next;
  }
  printf("P2: %llu\n", p2);

  free_list(head);

  fclose(fp);

  return 0;
}
