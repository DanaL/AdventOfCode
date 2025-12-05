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
  if (!n) return NULL;

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
      // insert
      // check for merge with prev?
      // return head
    }
    else if (!curr->next) {
      curr->next = n;
      return head;
    }

    curr = curr->next;
  }

  return NULL; // actually an error condition later
}

void print_list(Node *head)
{
  printf("Listy list:\n");
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

int main(void)
{
  FILE *fp = fopen("data/day05.txt", "rb");
  char buffer[BUFF_LEN];

  int read_state = 0;
  Node *head = NULL;
  int x = 0;
  while (fgets(buffer, BUFF_LEN, fp)) {
    ++x;
    if (strcmp(buffer, "\n") == 0) {
      read_state = 1;
    }
    else if (read_state == 0) {
      u_int64_t a, b;
      sscanf(buffer, "%llu-%llu", &a, &b);
      Node *n = make_node(a, b);
      head = insert(head, n);
    }
    else {
      uint64_t v = strtoull(buffer, NULL, 10);
      printf("%llu\n", v);
    }
    if (x > 2)
      break;
  }

  print_list(head);
  free_list(head);

  fclose(fp);

  return 0;
}
