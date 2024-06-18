#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

#include "utils.h"
int extract_int(char *s) 
{
  int val = 0;
  while (isdigit(*s)) {
    val = val * 10 + *s - '0';
    ++s;
  }

  return val;
}

int exec_vm(int reg_c_val)
{
  int num_of_lines = 0;
  char **program = read_all_lines("inputs/day12.txt", &num_of_lines);

  int registers[] = { 0, 0, reg_c_val, 0 };
  int pc = 0;

  while (pc < num_of_lines) {
    if (strncmp("inc", program[pc], 3) == 0) {
      int reg = program[pc][4] - 'a';
      ++registers[reg];
      
    }
    else if (strncmp("dec", program[pc], 3) == 0) {
      int reg = program[pc][4] - 'a';
      --registers[reg];
    }
    else if (strncmp("cpy", program[pc], 3) == 0) {
      size_t n = strlen(program[pc]);
      int val = 0;
      int reg = program[pc][n - 1] - 'a';
      
      if (isdigit(program[pc][4])) {        
        char *s = &program[pc][4];
        val = extract_int(s);       
      }
      else {
        val = registers[program[pc][4] - 'a'];
      }

      registers[reg] = val;
    }
    else if (strncmp("jnz", program[pc], 3) == 0) {
      int reg = program[pc][4] - 'a';
      int reg_val = registers[reg];
      int jump_val = atoi(&program[pc][6]);

      if (reg_val != 0) {
        pc += jump_val;
        continue;
      }
    }

    ++pc;
  }

  for (int j = 0; j < num_of_lines; j++)
    free(program[j]);
  free(program);

  return registers[0];
}

void p1(void)
{
  printf("P1: %d\n", exec_vm(0));
  printf("P2: %d\n", exec_vm(1));
}

int main(void)
{
  p1();
}
