#include <limits.h>
#include <stdio.h>
#include <stdlib.h>
#include <time.h>

/* Mucking about with a heapq implementation, ripped straight out of my
 * university algorithms text */
#define HEAP_LENGTH 16384
typedef struct heapq {
	int heap[HEAP_LENGTH];
	int heap_size;
} heapq;

void heap_push(heapq *q, int val) {
	/* need to check if the heap is full */
	int hole = ++q->heap_size;
	
	/* Percolate up */
	for (; hole > 1 && val < q->heap[hole/2]; hole /= 2)
		q->heap[hole] = q->heap[hole/2];
	q->heap[hole] = val;	
}

void heap_percolate_down(heapq *q, int hole) {
	int child;
	int tmp = q->heap[hole];

	for (; hole * 2 <= q->heap_size; hole = child) {
		child = hole * 2;
		if (child != q->heap_size && q->heap[child+1] < q->heap[child])
			++child;
		if (q->heap[child] < tmp)
			q->heap[hole] = q->heap[child];
		else
			break;
	}

	q->heap[hole] = tmp;
}

int heap_pop(heapq *q) {
	/* should check if the heap is empty */
	int result = q->heap[1];
	q->heap[1] = q->heap[q->heap_size--];
	heap_percolate_down(q, 1);

	return result;
}

typedef struct ranges {
    long min_x, max_x;
    long min_y, max_y;
    long min_z, max_z;
} ranges;

typedef struct candidate {
    long x, y, z;
    int nearby;
    int prev;
    int delta;
    int best;
} candidate;

typedef struct coord {
    long x, y, z, r;
} coord;

long rand_coord(long min, long max) {
    /* Straight out of the comp.lang.c faq */
    long r = min + rand() / (RAND_MAX / (max - min + 1) + 1);

    return r;
}

long manhattan(coord a, long x, long y, long z) {
    return labs(a.x - x) + labs(a.y - y) + labs(a.z - z);
}

/* How many bots is this point (x, y, z) within range of? */
int reachable_from(long x, long y, long z, coord bots[], int num_of_bots) {
    int count = 0;

    for (int j = 0; j < num_of_bots; j++) {
        if (manhattan(bots[j], x, y, z) <= bots[j].r)
            ++count;
    }

    return count;
}

void check_candidates(candidate cs[], int num_of_c, coord bots[], int num_of_bots) {
    for (int j = 0; j < num_of_c; j++) {
        cs[j].prev = cs[j].nearby;
        cs[j].nearby = reachable_from(cs[j].x, cs[j].y, cs[j].z, bots, num_of_bots);
        if (cs[j].nearby > cs[j].best)
            cs[j].best = cs[j].nearby;
    }
}

void search_by_neightbours(coord bots[], int num_of_bots) {
    int best = 0, count, best_id;
    long bx, by, bz;
    coord curr;

    for (int j = 0; j < num_of_bots; j++) {
        curr = bots[j];
        count = 0;
        for (int k = 0; k < num_of_bots; k++) {
            /* This calculates how many other bots' curr is within range of
                (as opposed to how many bots are within curr's range) */
            if (j != k && manhattan(bots[k], curr.x, curr.y, curr.z) <= bots[k].r) {
                ++count;
            }
        }
        if (count > best) {
            if (count > 900) printf(">900 %d\n", j);
            best = count;
            best_id = j;
            bx = curr.x;
            by = curr.y;
            bz = curr.z;
        }
        if (count == 0)
            printf("%d has no neighbours :o\n", j);
    }

    printf("Best %d had %d neighbours\n", best_id, best);
    printf("   %ld %ld %ld\n", bx, by, bz);
    printf("   distance from origin: %ld\n", manhattan(bots[best_id], 0, 0, 0));
    printf("   15972003, 44657553, 29285970\n");
    // 89,915,526
    // 76,468,059
                                    //10000000  10000000  10000000
    printf("--> %d\n", reachable_from(15972003, 44657553, 29285970, bots, num_of_bots));
    printf("--> %d\n", reachable_from(15972003, 44657553, 29285970, bots, num_of_bots));
    int deltas[] = { -50000000, 0, 100000000};
    for (int dx = 0; dx < 3; dx++) {
        for (int dy = 0; dy < 3; dy++) {
            for (int dz = 0; dz < 3; dz++) {
                count = reachable_from(bx+dx, by+dy, bz+dz, bots, num_of_bots);
                if (count > 831)
                    printf("%d %d %d %d\n", dx, dy, dz, count);
            }
        }
    }
}

void pure_random_guessing(coord bots[], int num_of_bots, ranges r) {
    int num_of_candidates = 9;
    candidate candidates[num_of_candidates];

    srand(time(0));
    rand_coord(-1000, 1000);
    for (int j = 0; j < num_of_candidates; j++) {
        candidates[j].x = rand_coord(r.min_x, r.max_x) / 2;
        candidates[j].y = rand_coord(r.min_y, r.max_y) / 2;
        candidates[j].z = rand_coord(r.min_z, r.max_z) / 2;
        candidates[j].nearby = 0;
        candidates[j].best = 0;
    }

    check_candidates(candidates, num_of_candidates, bots, num_of_bots);

    for (int n = 0; n < 1000; n++) {
        for (int j = 0; j < num_of_candidates; j++) {
            candidates[j].x += rand_coord(-10000000, 10000000);
            candidates[j].y += rand_coord(-10000000, 10000000);
            candidates[j].z += rand_coord(-10000000, 10000000);
        }

        check_candidates(candidates, num_of_candidates, bots, num_of_bots);
    }

    for (int j = 0; j < num_of_candidates; j++) {
        printf("Best result for %d: %d\n", j, candidates[j].best);
    }
}

void q2(coord bots[], int num_of_bots) {
    ranges r;
    r.min_x = LONG_MAX, r.max_x = LONG_MIN;
    r.min_y = LONG_MAX, r.max_y = LONG_MIN;
    r.min_z = LONG_MAX, r.max_z = LONG_MIN;

    for (int j = 0; j < num_of_bots; j++) {
        if (bots[j].x < r.min_x)
            r.min_x = bots[j].x;
        if (bots[j].x > r.max_x)
            r.max_x = bots[j].x;
        if (bots[j].y < r.min_y)
            r.min_y = bots[j].y;
        if (bots[j].y > r.max_y)
            r.max_y = bots[j].y;
        if (bots[j].z < r.min_z)
            r.min_z = bots[j].z;
        if (bots[j].z > r.max_z)
            r.max_z = bots[j].z;
    }

    //pure_random_guessing(bots, num_of_bots, r);
    search_by_neightbours(bots, num_of_bots);
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

    printf("Q1: %d\n", in_range);
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

    //q1(bots, 1000);
    //q2(bots, 1000);
	heapq q;
	q.heap_size = 0;
	
	srand(time(0));
	for (int j = 0; j < 100; j++) {
		heap_push(&q, rand() % 10000);
	}

	while (q.heap_size > 0) {
		printf("%d\n", heap_pop(&q));
	}	

    return 0;
}
