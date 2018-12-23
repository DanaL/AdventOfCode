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
    int a, b, c;
} instr;

typedef struct machine {
    int regs[6];
    int instr_ptr;
    int instr_val;
} machine;

struct instr parse_instr(char *line) {
    int a, b, c;
    char op[10];
    instr in;

    sscanf(line, "%s %d %d %d", op, &in.a, &in.b, &in.c);
    if (strcmp(op, "addi") == 0)
        in.opcode = ADDI;
    else if (strcmp(op, "addr") == 0)
        in.opcode = ADDR;
    else if (strcmp(op, "bani") == 0)
        in.opcode = BANI;
    else if (strcmp(op, "banr") == 0)
        in.opcode = BANR;
    else if (strcmp(op, "bori") == 0)
        in.opcode = BORI;
    else if (strcmp(op, "borr") == 0)
        in.opcode = BORR;
    else if (strcmp(op, "eqir") == 0)
        in.opcode = EQIR;
    else if (strcmp(op, "eqri") == 0)
        in.opcode = EQRI;
    else if (strcmp(op, "eqrr") == 0)
        in.opcode = EQRR;
    else if (strcmp(op, "gtir") == 0)
        in.opcode = GTIR;
    else if (strcmp(op, "gtri") == 0)
        in.opcode = GTRI;
    else if (strcmp(op, "gtrr") == 0)
        in.opcode = GTRR;
    else if (strcmp(op, "muli") == 0)
        in.opcode = MULI;
    else if (strcmp(op, "mulr") == 0)
        in.opcode = MULR;
    else if (strcmp(op, "seti") == 0)
        in.opcode = SETI;
    else if (strcmp(op, "setr") == 0)
        in.opcode = SETR;
    else {
        printf("Unknown opcode: %s\n", op);
        exit(0);
    }

    return in;
}

void execute_instr(machine *m, instr in) {
    switch (in.opcode) {
        case ADDI:
            m->regs[in.c] = m->regs[in.a] + in.b;
            break;
        case ADDR:
            m->regs[in.c] = m->regs[in.a] + m->regs[in.b];
            break;
        case MULR:
            m->regs[in.c] = m->regs[in.a] * m->regs[in.b];
            break;
        case MULI:
            m->regs[in.c] = m->regs[in.a] * in.b;
            break;
        case BANR:
            m->regs[in.c] = m->regs[in.a] & m->regs[in.b];
            break;
        case BANI:
            m->regs[in.c] = m->regs[in.a] & in.b;
            break;
        case BORR:
            m->regs[in.c] = m->regs[in.a] | m->regs[in.b];
            break;
        case BORI:
            m->regs[in.c] = m->regs[in.a] | in.b;
            break;
        case SETR:
            m->regs[in.c] = m->regs[in.a];
            break;
        case SETI:
            m->regs[in.c] = in.a;
            break;
        case GTIR:
            m->regs[in.c] = in.a > m->regs[in.b] ? 1 : 0;
            break;
        case GTRI:
            m->regs[in.c] = m->regs[in.a] > in.b ? 1 : 0;
            break;
        case GTRR:
            m->regs[in.c] = m->regs[in.a] > m->regs[in.b] ? 1 : 0;
            break;
        case EQIR:
            m->regs[in.c] = in.a == m->regs[in.b] ? 1 : 0;
            break;
        case EQRI:
            m->regs[in.c] = m->regs[in.a] == in.b ? 1 : 0;
            break;
        case EQRR:
            m->regs[in.c] = m->regs[in.a] == m->regs[in.b] ? 1 : 0;
            break;
        default:
            printf("Unknown opcode: %d\n", in.opcode);
            exit(-1);
    }
}

void execute_program(machine *m, const instr prog[], int prog_length, int verbose) {
    unsigned int executed = 0;
    instr curr;
    while (m->instr_val < prog_length) {
        curr = prog[m->instr_val];
        if (m->instr_val == 28) {
            printf("Instr 28: %d\n", m->regs[3]);
            printf("   %d %d %d %d\n", curr.opcode, curr.a, curr.b, curr.c);
        }

        m->regs[m->instr_ptr] = m->instr_val;
        execute_instr(m, curr);
        m->instr_val = m->regs[m->instr_ptr];
        ++m->instr_val;
        ++executed;
        if (verbose && executed % 100000) {
            printf("Curr state:\n");
            printf("   %d %d %d %d %d %d\n", m->regs[0], m->regs[1], m->regs[2], m->regs[3],
                m->regs[4], m->regs[5]);
        }
    }
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

    m.regs[0] = 3909249;
    execute_program(&m, prog, prog_length, 0);
    printf("%d\n", m.regs[0]);

    return 0;
}
