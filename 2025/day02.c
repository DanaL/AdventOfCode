#include <stdio.h>
#include <stdbool.h>
#include <stdlib.h>
#include <string.h>

#define BUFF_LEN 50

char* fetch_input(const char* filename) {
    FILE* file = fopen(filename, "rb");
    
    fseek(file, 0, SEEK_END);
    long size = ftell(file);
    fseek(file, 0, SEEK_SET);
    
    char* buffer = malloc(size + 1);
    if (!buffer) {
        fprintf(stderr, "Error: Could not allocate %ld bytes for file\n", size + 1);
        fclose(file);
        exit(EXIT_FAILURE);
    }
    
    fread(buffer, 1, size, file);
    buffer[size] = '\0';
    
    fclose(file);

    return buffer;
}

bool repeated_digits(const char *s, size_t seg_len, size_t num_of_segs)
{
    for (size_t seg = 1; seg < num_of_segs; seg++) {
        if (strncmp(s, s + seg * seg_len, seg_len) != 0) {
           return false;
        }
    }

    return true;
}

int main(void)
{
    char buffer[BUFF_LEN];
    char *data = fetch_input("data/day02.txt");

    unsigned long long p1 = 0, p2 = 0;
    unsigned long long lo, hi;
    char *ptr = data;
    while (sscanf(ptr, "%llu-%llu", &lo, &hi) == 2) {
        for (unsigned long long n = lo; n <= hi; n++) {
            snprintf(buffer, sizeof(buffer), "%llu", n);
            size_t len = strlen(buffer);

            // part 1
            if (len % 2 == 0) {
                size_t seg_len = len / 2;
                if (strncmp(buffer, buffer + seg_len, seg_len) == 0)
                    p1 += n;
            }

            // part 2
            for (size_t seg_len = 1; seg_len <= len / 2; seg_len++) {
                if (len % seg_len != 0)
                    continue;
                size_t num_of_segs = len / seg_len;
                if (repeated_digits(buffer, seg_len, num_of_segs)) {
                    p2 += n;
                    break;
                }
            }
        }

        ptr = strchr(ptr, ',');
        if (!ptr)
            break;
        ptr++; // skip over comma
    }

    printf("P1: %llu\n", p1);
    printf("P2: %llu\n", p2);

    free(data);

    return 0;
}