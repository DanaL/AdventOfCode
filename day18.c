#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define OPEN '.'
#define TREES '|'
#define LUMBER_YARD '#'

void free_grid(char **grid, int row_count) {
    for (int j = 0; j < row_count; j++)
        free(grid[j]);
    free(grid);
}

char **copy_grid(char **grid, int row_count) {
    char **new_grid = malloc(row_count * sizeof(char*));
    for (int j = 0; j < row_count; j++) {
        new_grid[j] = malloc(strlen(grid[j]));
        /* Yes, yes, strcpy bad it should suffice for Advent of Code... */
        strcpy(new_grid[j], grid[j]);
    }

    return new_grid;
}

void dump_grid(char **grid, int row_count) {
    for (int j = 0; j < row_count; j++)
        puts(grid[j]);
}

int calc_q1_score(char **grid, int height, int width) {
    int trees = 0, ly = 0;

    for (int r = 0; r < height; r++) {
        for (int c = 0; c < width; c++) {
            if (grid[r][c] == TREES) ++trees;
            if (grid[r][c] == LUMBER_YARD) ++ly;
        }
    }

    return trees * ly;
}

int calc_score(char **grid, int height, int width) {
    int trees = 0, ly = 0;

    for (int r = 0; r < height; r++) {
        for (int c = 0; c < width; c++) {
            if (grid[r][c] == TREES) ++trees;
            if (grid[r][c] == LUMBER_YARD) ++ly;
        }
    }

    return trees * ly;
}

int calc_lumber(char **grid, int height, int width) {
    int trees = 0;

    for (int r = 0; r < height; r++) {
        for (int c = 0; c < width; c++) {
            if (grid[r][c] == TREES) ++trees;
        }
    }

    return trees;
}

int count_adj(char **grid, char target, int r, int c, int height, int width) {
    int count = 0;
    if (r > 0 && c > 0 && grid[r-1][c-1] == target)
        ++count;
    if (r > 0 && grid[r-1][c] == target)
        ++count;
    if (r > 0 && c < width -1 && grid[r-1][c+1] == target)
        ++count;
    if (c > 0 && grid[r][c-1] == target)
        ++count;
    if (c < width - 1 && grid[r][c+1] == target)
        ++count;
    if (r < height - 1 && c > 0 && grid[r+1][c-1] == target)
        ++count;
    if (r < height - 1 && grid[r+1][c] == target)
        ++count;
    if (r < height - 1 && c < width - 1 && grid[r+1][c+1] == target)
        ++count;

    return count;
}

void do_generation(char **grid, char **next_gen, int height, int width) {
    int trees, ly;

    for (int r = 0; r < height; r++) {
        for (int c = 0; c < width; c++) {
            switch (grid[r][c]) {
                case OPEN:
                    if (count_adj(grid, TREES, r, c, height, width) >= 3)
                        next_gen[r][c] = TREES;
                    break;
                case TREES:
                if (count_adj(grid, LUMBER_YARD, r, c, height, width) >= 3)
                    next_gen[r][c] = LUMBER_YARD;
                    break;
                case LUMBER_YARD:
                    trees = count_adj(grid, TREES, r, c, height, width);
                    ly = count_adj(grid, LUMBER_YARD, r, c, height, width);
                    if (trees < 1 || ly < 1)
                        next_gen[r][c] = OPEN;
                    break;
            }
        }
    }
}

int main(int argc, char **argv) {
    /* Read in our file */
    FILE *f = fopen("field.txt", "r");
    char buffer[1024];
    size_t nread;
    char *line = NULL;
    char **grid = NULL;
    int c, row_count = 0;

    if (f) {
        while (fgets(buffer, 1024, f)) {
            ++row_count;
            grid = realloc(grid, row_count * sizeof(char*));
            line = (char *) malloc(strlen(buffer));
            grid[row_count - 1] = line;
            for (c = 0; c < strlen(buffer) && buffer[c] != '\n'; c++)
                line[c] = buffer[c];
            line[c] = '\0';
        }
    }
    fclose(f);

    int num_of_generations = 100;
    char **copy;
    int lumber, prev_lumber;
    for (int j = 0; j < num_of_generations; j++) {
        copy = copy_grid(grid, row_count);
        do_generation(grid, copy, row_count, strlen(grid[0]));
        free_grid(grid, row_count);
        grid = copy;
    }

    dump_grid(grid, row_count);
    
    free_grid(grid, row_count);

    return 0;
}