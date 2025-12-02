#include <stdio.h>
#include <stdbool.h>
#include <stdlib.h>
#include <string.h>

#define BUFF_LEN 50

struct range {
    unsigned long long lo;
    unsigned long long hi;
};

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

struct range extract_range(size_t *pos, const char *s)
{
    struct range r;
    size_t start = *pos, dash = 0;
    
    while (s[*pos] != '\0' && s[*pos] != ',') {
        if (s[*pos] == '-')
            dash = *pos;
        ++(*pos);
    }

    char buff[BUFF_LEN];
    size_t n = dash - start;
    memcpy(buff, s + start, n);
    buff[n] = '\0';
    r.lo = strtoull(buff, NULL, 10);

    n = *pos - dash - 1;
    memcpy(buff, s + dash + 1, n);
    buff[n] = '\0';
    r.hi = strtoull(buff, NULL, 10);
    
    if (s[*pos] == ',')
        ++(*pos); // bump pos past the comma/newline char

    return r;
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

unsigned long long p1_check(struct range r)
{
    unsigned long long total = 0;
    char buffer[BUFF_LEN];

    for (unsigned long long n = r.lo; n <= r.hi; n++) {
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

unsigned long long p2_check(struct range r)
{
    unsigned long long total = 0;
    char buffer[BUFF_LEN];

    for (unsigned long long n = r.lo; n <= r.hi; n++) {
        snprintf(buffer, sizeof(buffer), "%llu", n);
        size_t len = strlen(buffer);
        for (size_t seg_len = 1; seg_len < len; seg_len++) {
            size_t num_of_segs = len / seg_len;
            if (len % num_of_segs != 0 || len % seg_len != 0)
                continue;
            size_t seg_len = len / num_of_segs;
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
    size_t pos = 0;
    size_t len = strlen(data);
    while (pos < len) {
        struct range r = extract_range(&pos, data);
        p1 += p1_check(r);
        p2 += p2_check(r);
    }

    printf("P1: %llu\n", p1);
    printf("P2: %llu\n", p2);

    free(data);

    return 0;
}