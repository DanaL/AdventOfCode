#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<ctype.h>

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
  char **program = NULL;
  FILE *fp = fopen("inputs/day12.txt", "r");
      
  char buffer[50];
  int num_of_lines = 0;
  while (fgets(buffer, sizeof buffer, fp) != NULL) {
    size_t bc = strlen(buffer);
    if (buffer[bc - 1] == '\n')
      buffer[bc - 1] = '\0';
    
    program = realloc(program, ++num_of_lines * sizeof(char*));
    program[num_of_lines - 1] = malloc((bc + 1) * sizeof(char));
    strcpy(program[num_of_lines - 1], buffer);
  }
  fclose(fp);

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