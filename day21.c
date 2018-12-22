#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define ADDI 12
#define ADDR 8
#define BANI 1
#define BANR 11
#define BORI 5
#define BORR 9
#define EQIR 4
#define EQRI 13
#define EQRR 2
#define GTIR 3
#define GTRI 0
#define GTRR 15
#define MULI 10
#define MULR 14
#define SETI 6
#define SETR 7

typedef struct instr {
    int opcode;
    int regs[3];
} instr;

typedef struct machine {
    int regs[6];
    int instr_ptr;
    int instr_val;
} machine;

struct instr parse_instr(char *line) {
    char i[5];
    i[4] = '\0';
    instr in;

    memcpy(i, line, 4);

    if (strcmp(i, "addi") == 0)
        in.opcode = ADDI;
    else if (strcmp(i, "addr") == 0)
        in.opcode = ADDR;
    else if (strcmp(i, "bani") == 0)
        in.opcode = BANI;
    else if (strcmp(i, "banr") == 0)
        in.opcode = BANR;
    else if (strcmp(i, "bori") == 0)
        in.opcode = BORI;
    else if (strcmp(i, "borr") == 0)
        in.opcode = BORR;
    else if (strcmp(i, "eqir") == 0)
        in.opcode = EQIR;
    else if (strcmp(i, "eqri") == 0)
        in.opcode = EQRI;
    else if (strcmp(i, "eqrr") == 0)
        in.opcode = EQRR;
    else if (strcmp(i, "gtir") == 0)
        in.opcode = GTIR;
    else if (strcmp(i, "gtri") == 0)
        in.opcode = GTRI;
    else if (strcmp(i, "gtrr") == 0)
        in.opcode = GTRR;
    else if (strcmp(i, "muli") == 0)
        in.opcode = MULI;
    else if (strcmp(i, "mulr") == 0)
        in.opcode = MULR;
    else if (strcmp(i, "seti") == 0)
        in.opcode = SETI;
    else if (strcmp(i, "setr") == 0)
        in.opcode = SETR;
    else {
        printf("Unknown opcode: %s\n", i);
        exit(0);
    }

    return in;
}

int main(int argc, char **argv) {
    FILE *f = fopen("program_day21.txt", "r");
    char buffer[1024];
    size_t nread;
    char *line = NULL;
    int c;
    instr prog[100];
    int prog_length = 0;

    machine m;
    m.instr_ptr = 4;
    m.instr_val = 0;
    m.regs[0] = 0;
    m.regs[1] = 0;
    m.regs[2] = 0;
    m.regs[3] = 0;
    m.regs[4] = 0;
    m.regs[5] = 0;

    if (f) {
        while (fgets(buffer, 1024, f)) {
            line = (char *) malloc(strlen(buffer));
            for (c = 0; c < strlen(buffer) && buffer[c] != '\n'; c++)
                line[c] = buffer[c];
            line[c] = '\0';

            if (line[0] == '#')
                continue;
            prog[prog_length++] = parse_instr(line);
        }
    }

    for (int j = 0; j < prog_length; j++)
        printf("%d\n", prog[j].opcode);
    return 0;
}
