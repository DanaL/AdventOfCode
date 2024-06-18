#ifndef UTILS_H
#define UTILS_H

char *md5(const char *txt);
char **read_all_lines(const char *filename, size_t *line_count);
void lines_free(char **lines, size_t line_count);

#endif
