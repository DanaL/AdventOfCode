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
        for (size_t p = 0; p < seg_len; p++) {
            if (s[p] != s[seg * seg_len + p])
                return false;
        }
    }

    return true;
}

unsigned long long p1_check(unsigned long long a, unsigned long long b)
{
    unsigned long long total = 0;
    char buffer[BUFF_LEN];

    for (unsigned long long n = a; n <= b; n++) {
        snprintf(buffer, sizeof(buffer), "%llu", n);
        size_t len = strlen(buffer);
        if (len % 2 != 0 )
            continue;
        size_t seg_len = len / 2;
        if (repeated_digits(buffer, seg_len, 2)) {
            total += n;
        }
    }

    return total;
}

unsigned long long p2_check(unsigned long long a, unsigned long long b)
{
    unsigned long long total = 0;
    char buffer[BUFF_LEN];

    for (unsigned long long n = a; n <= b; n++) {
        snprintf(buffer, sizeof(buffer), "%llu", n);
        size_t len = strlen(buffer);
        for (size_t seg_len = 1; seg_len < len; seg_len++) {
            if (len % seg_len != 0)
                continue;
            size_t num_of_segs = len / seg_len;
            if (repeated_digits(buffer, seg_len, num_of_segs)) {
                total += n;
                break;
            }
        }
    }

    return total;
}

int main(void)
{
    char *data = fetch_input("data/day02.txt");

    unsigned long long p1 = 0, p2 = 0;
    
    char *ptr = data;
    unsigned long long lo, hi;
    while (sscanf(ptr, "%llu-%llu", &lo, &hi) == 2) {
        p1 += p1_check(lo, hi);
        p2 += p2_check(lo, hi);

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