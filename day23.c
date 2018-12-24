#include <limits.h>
#include <stdio.h>
#include <stdlib.h>
#include <time.h>

typedef struct candidate {
    long x, y, z;
    int nearby;
} candidate;

typedef struct coord {
    long x, y, z, r;
} coord;

long manhattan(coord a, long x, long y, long z) {
    return labs(a.x - x) + labs(a.y - y) + labs(a.z - z);
}

int reachable_from(long x, long y, long z, coord bots[], int num_of_bots) {
    int count = 0;

    for (int j = 0; j < num_of_bots; j++) {
        if (manhattan(bots[j], x, y, z) <= bots[j].r)
            ++count;
    }

    return count;
}

long avg_distance(coord a, coord bots[], int num_of_bots) {
    long sum;

    for (int j = 0; j < num_of_bots; j++)
        sum += manhattan(bots[j], a.x, a.y,a.z);

    printf("%ld\n", sum);
    return sum / num_of_bots;
}

void q1(coord bots[], int num_of_bots) {
    long widest = 0;
    coord strongest;

    for (int j = 0; j < num_of_bots; j++) {
        if (bots[j].r > widest) {
            widest = bots[j].r;
            strongest = bots[j];
        }
    }

    long mx = strongest.x, my = strongest.y, mz = strongest.z;
    int in_range = 0;
    for (int j = 0; j < num_of_bots; j++) {
        if (manhattan(bots[j], mx, my, mz) <= widest)
            ++in_range;
    }

    printf("%d\n", in_range);
}

void q2(coord bots[], int num_of_bots) {
    candidate candidates[10];
    long min_x = LONG_MAX, max_x = LONG_MIN;
    long min_y = LONG_MAX, max_y = LONG_MIN;
    long min_z = LONG_MAX, max_z = LONG_MIN;

    for (int j = 0; j < num_of_bots; j++) {
        if (bots[j].x < min_x)
            min_x = bots[j].x;
        if (bots[j].x > max_x)
            max_x = bots[j].x;
        if (bots[j].y < min_y)
            min_y = bots[j].y;
        if (bots[j].y > max_y)
            max_y = bots[j].y;
        if (bots[j].z < min_z)
            min_z = bots[j].z;
        if (bots[j].z > max_z)
            max_z = bots[j].z;
    }

    printf("X range: %ld %ld\n", min_x, max_x);
    printf("Y range: %ld %ld\n", min_y, max_y);
    printf("Z range: %ld %ld\n", min_z, max_z);
    printf("R: %d\n", rand() % (max_x - min_x));
    printf("Nearby: %d\n", reachable_from(-5450833, 49619122, 25583046, bots, 1000));
}

int main() {
    coord bots[1000];
    FILE *f = fopen("nanobots.txt", "r");

    if (f) {
        int j = 0;
        char buffer[1024];
        while (fgets(buffer, 1024, f)) {
            sscanf(buffer, "pos=<%ld,%ld,%ld>, r=%ld", &bots[j].x, &bots[j].y, &bots[j].z, &bots[j].r);
            ++j;
        }
    }
    fclose(f);

    q1(bots, 1000);

    srand(time(NULL));
    for (int j = 0; j < 10; j++)
        printf("%d\n", rand() % 1000000000);

    q2(bots, 1000);

    return 0;
}
