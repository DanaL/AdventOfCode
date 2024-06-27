#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>

struct elf {
  uint32_t num;
  struct elf *left;
  struct elf *right;
};

struct elf* elf_create(uint32_t n)
{
  struct elf *e = malloc(sizeof(struct elf));
  e->num = n;
  e->left = NULL;
  e->right = NULL;

  return e;
}

uint32_t solve(uint32_t num_of_elves)
{
  struct elf *first = elf_create(1);
  struct elf *prev = first;

  for (uint32_t j = 1; j < num_of_elves; j++) {
    struct elf *e = elf_create(j + 1);

    // elves are in a circle, left is clockwise,
    // right is anticlockwise
    e->right = prev;
    prev->left = e;
    prev = e;
  }

  prev->left = first;
  first->right = prev;

  struct elf *curr = first;
  while (curr->left != curr && curr->right != curr) {
    struct elf *to_remove = curr->left;
    curr->left = to_remove->left;
    curr->left->right = curr;
    free(to_remove);

    curr = curr->left;
  }

  return curr->num;
}

int main(void)
{
  printf("P1: %zu\n", solve(3012210));
}


