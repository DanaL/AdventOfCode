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

struct range extract_range(size_t *pos, char *s)
{
    struct range r;
    size_t start = *pos, dash = 0;
    
    while (s[*pos] != '\0' && s[*pos] != ',')
    {
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

bool repeated_digits(unsigned long long n)
{
    char buffer[BUFF_LEN];
    snprintf(buffer, sizeof(buffer), "%llu", n);

    size_t len = strlen(buffer);
    if (len % 2 != 0)
        return false;
        
    char *a = buffer;
    char *b = buffer + len / 2;

    for (size_t j = 0; j < len / 2; j++) {
        if (*a != *b)
            return false;
        ++a;
        ++b;
    }

    return true;
}

unsigned long long check_range(struct range r)
{
    unsigned long long total = 0;
    for (unsigned long long n = r.lo; n <= r.hi; n++)
    {
        if (repeated_digits(n))
            total += n;
    }

    return total;
}

int main(void)
{
    char *data = fetch_input("data/day02.txt");

    printf("%s\n", data);

    unsigned long long total = 0;
    size_t pos = 0;
    size_t len = strlen(data);
    while (pos < len)
    {
        struct range r = extract_range(&pos, data);
        total += check_range(r);
    }

    printf("P1: %llu\n", total);

    free(data);

    return 0;
}