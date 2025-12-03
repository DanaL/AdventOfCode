#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define BUFF_SIZE 128
#define BATTERY_SIZE 12

int calc_p1_joltage(const char *s, size_t len)
{
    char a = '\0', b = '\0';
    for (size_t j = 0; j < len; j++)
    {
        if (s[j] > a && j < len - 1)
        {
            a = s[j];
            b = '\0';
        }
        else if (s[j] > b)
        {
            b = s[j];
        }
    }

    return (a - '0') * 10 + (b - '0');
}

unsigned long long calc_p2_joltage(const char *s, size_t len)
{
    char battery[BATTERY_SIZE + 1];
    battery[BATTERY_SIZE] = '\0';
    
    size_t start_pos = 0;
    for (size_t digit = 0; digit < BATTERY_SIZE; digit++) {
        size_t remaining = BATTERY_SIZE - digit - 1;
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
    unsigned int p1_joltage = 0;
    unsigned long long p2_joltage = 0;
    while (fgets(buffer, BUFF_SIZE, fp))
    {
        size_t n = strlen(buffer) - 1;
        p1_joltage += calc_p1_joltage(buffer, n);
        p2_joltage += calc_p2_joltage(buffer, n);
    }

    printf("P1: %u\n", p1_joltage);
    printf("P2: %llu\n", p2_joltage);
    
    fclose(fp);

    return 0;
}
