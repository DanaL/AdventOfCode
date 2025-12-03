#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define BUFF_SIZE 128
#define MAX_BATTERY_SIZE 12

unsigned long long calc_joltage(const char *s, size_t len, size_t battery_size)
{
    char battery[MAX_BATTERY_SIZE + 1];
    battery[battery_size] = '\0';

    size_t start_pos = 0;
    for (size_t digit = 0; digit < battery_size; digit++) {
        size_t remaining = battery_size - digit - 1;
        size_t search_len = len - remaining;

        char biggest = '0';
        size_t biggest_pos = start_pos;
        for (size_t j = start_pos; j < search_len; j++) {
            if (s[j] > biggest) {
                biggest = s[j];
                biggest_pos = j;
            }
        }

        battery[digit] = biggest;
        start_pos = biggest_pos + 1;
    }
    
    unsigned long long joltage = strtoull(battery, NULL, 10);
    
    return joltage;
}

int main(void)
{
    FILE *fp;
    char buffer[BUFF_SIZE];

    fp = fopen("data/day03.txt", "r");
    unsigned long long p1_joltage = 0, p2_joltage = 0;    
    while (fgets(buffer, BUFF_SIZE, fp))
    {
        size_t n = strlen(buffer) - 1;
        p1_joltage += calc_joltage(buffer, n, 2);
        p2_joltage += calc_joltage(buffer, n, 12);
    }

    printf("P1: %u\n", p1_joltage);
    printf("P2: %llu\n", p2_joltage);
    
    fclose(fp);

    return 0;
}
