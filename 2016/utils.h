#ifndef UTILS_H
#define UTILS_H

char *md5(const char *txt);

char **read_all_lines(const char *filename, size_t *line_count);
void lines_free(char **lines, size_t line_count);

// my heap implementation
struct heap {
  void **table;
  size_t table_size;
  size_t num_of_elts;
};

struct heap *heap_new(void);
void min_heap_push(struct heap *h, void *item, int (*priority)(const void *));
void *min_heap_pop(struct heap *h, int (*priority)(const void *));

#endif
