#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
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

void toggle(char *stmt)
{
  if (strncmp("tgl", stmt, 3) == 0 || strncmp("dec", stmt, 3) == 0) {
    stmt[0] = 'i';
    stmt[1] = 'n';
    stmt[2] = 'c';
  }
  else if (strncmp("cpy", stmt, 3) == 0)
  {
    stmt[0] = 'j';
    stmt[1] = 'n';
    stmt[2] = 'z';
  }
  else if (strncmp("inc", stmt, 3) == 0)
  {
    stmt[0] = 'd';
    stmt[1] = 'e';
    stmt[2] = 'c';
  }
  else if (strncmp("jnz", stmt, 3) == 0)
  {
    stmt[0] = 'c';
    stmt[1] = 'p';
    stmt[2] = 'y';
  }
}

int64_t calc_val(int64_t registers[], const char *s, int param)
{
  char *cs = malloc(strlen(s) + 1);
  strcpy(cs, s);

  // assuming well-formed assembunny instructions here
  char *token = strtok(cs, " ");
  token = strtok(NULL, " ");
  if (param == 2)
    token = strtok(NULL, " ");

  int64_t val;
  if (token[0] >= 'a' && token[0] <= 'e')
    val = registers[token[0] - 'a'];
  else
    val = atoi(token); 

  free(cs);

  return val;
}

int64_t exec_vm(int64_t reg_a_val)
{
  size_t lc = 0;
  char **program = read_all_lines("inputs/day23.txt", &lc);

  int64_t registers[] = { reg_a_val, 0, 0, 0 };
  int pc = 0;

  while (pc < lc) {
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
      char ch = program[pc][n - 1];
      if (ch < 'a' || ch > 'e') {
        // due to the tgl instr, we could end up with invalid
        // cpy instructions like cpy 4 7. Skip invalid instructions
        ++pc;
        continue;
      }

      int dest_reg = ch - 'a';
      int64_t val = calc_val(registers, program[pc], 1);

      registers[dest_reg] = val;
    }
    else if (strncmp("jnz", program[pc], 3) == 0) {
      int64_t x = calc_val(registers, program[pc], 1);
      int jump_val = (int) calc_val(registers, program[pc], 2);
      
      if (x != 0) {
        pc += jump_val;
        continue;
      }
    }
    else if (strncmp("tgl", program[pc], 3) == 0) {
      int reg = program[pc][4] - 'a';
      int reg_val = registers[reg];
      int instr_to_tgl = pc + reg_val;
      if (instr_to_tgl >= 0 && instr_to_tgl < lc) {
        toggle(program[instr_to_tgl]);
      }
    }

    ++pc;
  }
 
  lines_free(program, lc);

  return registers[0];
}

void p1(void)
{
  printf("P1: %lld\n", exec_vm(7));
}

int main(void)
{
  p1();
}
