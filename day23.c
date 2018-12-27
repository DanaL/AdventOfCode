#include <limits.h>
#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#define NUM_OF_BOTS 1000

typedef struct coord {
    long x, y, z, r;
} coord;


typedef struct searchbox {
	long x, y, z;
	long side_length;
	int points_covered;
} searchbox;

long manhattan(long x0, long y0, long z0, long x1, long y1, long z1) {
    return labs(x0 - x1) + labs(y0 - y1) + labs(z0 - z1);
}

long distance_range(int a, int lo, int hi) {
	if (a < lo) return lo - a;
	if (a > hi) return a - hi;
	return 0;
}

void bots_covered_by_box(searchbox *box, coord bots[], int num_of_bots) {
	int covered = 0;
	long x0 = box->x, x1 = box->x + box->side_length;	
	long y0 = box->y, y1 = box->y + box->side_length;	
	long z0 = box->z, z1 = box->z + box->side_length;

	for (int j = 0; j < num_of_bots; j++) {
		long d = distance_range(bots[j].x, x0, x1);
		d += distance_range(bots[j].y, y0, y1);
		d += distance_range(bots[j].z, z0, z1);
		if (d <= bots[j].r)
			++covered;	
	}

	box->points_covered = covered;	
}

/* Simple priority queue with a heapq implementation, ripped straight out of my
 * university algorithms text */
#define HEAP_LENGTH NUM_OF_BOTS * NUM_OF_BOTS
typedef struct heapq {
	searchbox *heap[HEAP_LENGTH];
	int heap_size;
} heapq;

/* Comparison function for the heap operations. A searchbox has higher priority
	if it covers more bots. In the event of a tie, the box that is closer to the origin
	wins */
int cmp_searchboxes(searchbox *a, searchbox *b) {
	if (a->points_covered > b->points_covered)
		return 1;
	else if (a->points_covered == b->points_covered) {
		long ma = manhattan(a->x, a->y, a->z, 0, 0, 0);
		long mb = manhattan(b->x, b->y, b->z, 0, 0, 0);
	
		return ma < mb;	
	}

	return 0;
}

void heap_push(heapq *q, searchbox *box) {
	/* need to check if the heap is full */
	if (q->heap_size >= HEAP_LENGTH) {
		puts("Oops...heap exceeded max size");
		exit(0);
	}

	int hole = ++q->heap_size;
	
	/* Percolate up */
	for (; hole > 1 && cmp_searchboxes(box, q->heap[hole/2]); hole /= 2)
		q->heap[hole] = q->heap[hole/2];

	q->heap[hole] = box;	
}

void heap_percolate_down(heapq *q, int hole) {
	int child;
	searchbox *tmp = q->heap[hole];

	for (; hole * 2 <= q->heap_size; hole = child) {
		child = hole * 2;
		if (child != q->heap_size && cmp_searchboxes(q->heap[child+1],q->heap[child]))
			++child;	
		
		if (cmp_searchboxes(q->heap[child], tmp))
			q->heap[hole] = q->heap[child];
		else
			break;
	}

	q->heap[hole] = tmp;
}

searchbox* heap_pop(heapq *q) {
	/* should check if the heap is empty */
	searchbox *result = q->heap[1];
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

long rand_coord(long min, long max) {
    /* Straight out of the comp.lang.c faq */
    long r = min + rand() / (RAND_MAX / (max - min + 1) + 1);

    return r;
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

	// And now, the search boxes. I need to start with a cube big enough to cover the full
	// set of points, then split it into smaller and smaller boxes until I find a box whose
	// side length is 1 and which covers the most bots. Search boxes are stored as an (x, y)
	// point and a side-length. The (x, y) point are the "bottom-left" corner of the box.

	/* Initial, giant box */ 
	searchbox *box = malloc(sizeof(searchbox));
	box->x = r.min_x;
	box->y = r.min_y;
	box->z = r.min_z;
	long xrange = labs(r.max_x - r.min_x);
	long yrange = labs(r.max_y - r.min_y);

	/* We need the side length to be a power of 2 in order to ensure we can always evenly divide
		the searchbox */	
	box->side_length = 1;
	while (box->x + box->side_length < r.max_x || box->y + box->side_length < r.max_y || box->z + box->side_length < r.max_z)
		box->side_length *= 2;
	bots_covered_by_box(box, bots, num_of_bots);
	
	heapq q;
	q.heap_size = 0;
	heap_push(&q, box);

	while (q.heap_size > 0) {
		box = heap_pop(&q);

		if (box->side_length == 1) {
			/* We are done! By definition, the first box we pop off the heap that
				has side length of 1 is the point which covers the most bots */
			printf("Q2: Bots covered: %d (%ld %ld %ld)\n", box->points_covered, box->x, box->y, box->z);
			printf("        Distance from origin: %ld\n", manhattan(box->x, box->y, box->z, 0, 0, 0));
			free(box);
			break;
		}

		long new_side_length = box->side_length / 2;

		/* Now create the eight sub-boxes */
		searchbox *nb;

		nb = malloc(sizeof(searchbox));
		nb->x = box->x; nb->y = box->y; nb->z = box->z;
		nb->side_length = new_side_length;
		bots_covered_by_box(nb, bots, num_of_bots);
		heap_push(&q, nb);

		nb = malloc(sizeof(searchbox));
		nb->x = box->x + new_side_length - 1; nb->y = box->y; nb->z = box->z;
		nb->side_length = new_side_length;
		bots_covered_by_box(nb, bots, num_of_bots);
		heap_push(&q, nb);

		nb = malloc(sizeof(searchbox));
		nb->x = box->x; nb->y = box->y + new_side_length - 1; nb->z = box->z;
		nb->side_length = new_side_length;
		bots_covered_by_box(nb, bots, num_of_bots);
		heap_push(&q, nb);

		nb = malloc(sizeof(searchbox));
		nb->x = box->x + new_side_length - 1; nb->y = box->y + new_side_length - 1; nb->z = box->z;
		nb->side_length = new_side_length;
		bots_covered_by_box(nb, bots, num_of_bots);
		heap_push(&q, nb);

		nb = malloc(sizeof(searchbox));
		nb->x = box->x; nb->y = box->y; nb->z = box->z + new_side_length - 1;
		nb->side_length = new_side_length;
		bots_covered_by_box(nb, bots, num_of_bots);
		heap_push(&q, nb);

		nb = malloc(sizeof(searchbox));
		nb->x = box->x + new_side_length - 1; nb->y = box->y; nb->z = box->z + new_side_length - 1;
		nb->side_length = new_side_length;
		bots_covered_by_box(nb, bots, num_of_bots);
		heap_push(&q, nb);

		nb = malloc(sizeof(searchbox));
		nb->x = box->x; nb->y = box->y + new_side_length - 1; nb->z = box->z + new_side_length - 1;
		nb->side_length = new_side_length;
		bots_covered_by_box(nb, bots, num_of_bots);
		heap_push(&q, nb);

		nb = malloc(sizeof(searchbox));
		nb->x = box->x + new_side_length - 1; nb->y = box->y + new_side_length - 1; nb->z = box->z + new_side_length - 1;
		nb->side_length = new_side_length;
		bots_covered_by_box(nb, bots, num_of_bots);
		heap_push(&q, nb);

		free(box);
	}

	/*
	puts("Remaining:\n");
	while (q.heap_size > 0) {
		searchbox *b = heap_pop(&q);
		if (b->points_covered == 977) {
			printf("Points covered: %d (%ld %ld %ld)\n", b->points_covered, b->x, b->y, b->z);
			printf("        Distance from origin: %ld\n", manhattan(b->x, b->y, b->z, 0, 0, 0));
		}
	}
	*/
	/* Free any remaining boxes on the heap */
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
		coord b = bots[j];
        if (manhattan(b.x, b.y, b.z, mx, my, mz) <= widest)
            ++in_range;
    }

    printf("Q1: %d\n", in_range);
}

int main() {
    coord bots[NUM_OF_BOTS];
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

	q1(bots, NUM_OF_BOTS);
	q2(bots, NUM_OF_BOTS);

    return 0;
}
