#include <stdio.h>
#include <stdbool.h>
#include <stdlib.h>
#include <string.h>

#define LINE_W 100

struct bot {
  int id;
  int val1;
  int val2;
};


struct rule {
  int bot_id;
  int lo_dest;
  int lo_val;
  int hi_dest;
  int hi_val;
};

int get_val(char *s, char *word)
{
  char *v = s + strlen(word) + 1;
  char *w = v + 1;
  while (*w != ' ' && *w != '\0')
    ++w;
  int len = w - v;
  char buffer[100];
  strncpy(buffer, v, len);
  buffer[len] = '\0';

  int val;
  sscanf(buffer, "%d", &val);

  return val;
}

int find_max_val(char *s, char *word)
{
  char *sub_str = s;
  char *w;
  int max = -1;

  while ((w = strstr(sub_str, word)) != NULL) {
    if (w != NULL) {
      int val = get_val(w, word);
      if (val > max)
        max = val;
    }
    sub_str = w + 1;
  }
  
  return max;
}

bool contains(char *s, char *word, int target)
{
  char *sub_str = s;
  char *w;

  while ((w = strstr(sub_str, word)) != NULL) {
    if (w != NULL) {
      int val = get_val(w, word);
      if (val == target)
        return true;
    }
    sub_str = w + 1;
  }

  return false;
}

int main(void)
{
  struct rule *rules = NULL;
  int rule_count = 0;

  char **lines = NULL;
  int lc = 0;

  char buffer[LINE_W];
  FILE *fp = fopen("inputs/day10.txt", "r");
  while (fgets(buffer, sizeof buffer, fp) != NULL) {
    int len = strlen(buffer);
    if (buffer[len - 1] == '\n') {
      buffer[len - 1] = '\0';
    }

    lines = realloc(lines, sizeof(char *) * (lc + 1));

    char *s = malloc(sizeof(char) * (len + 1));
    strcpy(s, buffer);
    lines[lc] = s;
    ++lc;
    //strcpy(&lines[lc - 1], buffer);
    //lines[lc - 1][len] = '\0';
    /*
    if (strncmp("value", buffer, 5) == 0) {
      int v1, id;
      sscanf(buffer, "value %d goes to bot %d", &v1, &id);
      struct bot b = { .val1 = v1, .id = id, .val2 = 0 };
      bots = realloc(bots, sizeof *bots * ++bot_count);
      bots[bot_count - 1] = b;
    }
    else {
      int bot_id, lo, hi;
      sscanf(buffer, "bot %d gives low to bot %d and high to bot %d", &bot_id, &lo, &hi);
      struct rule r = { .bot_id = bot_id, .lo = lo, .hi = hi };
      rules = realloc(rules, sizeof *rules * ++rule_count);
      rules[rule_count - 1] = r;
      printf("flag: %s %d %d %d\n", buffer, bot_id, lo, hi);
    }*/
  }
  fclose(fp);

  int max_bot = 0, max_output = 0;
  for (int i = 0; i < lc; i++) {
    int max = find_max_val(lines[i], "bot"); 
    if (max > max_bot)
      max_bot = max;
    max = find_max_val(lines[i], "output");
    if (max > max_output)
      max_output = max;
  }

  int *outputs = calloc(max_output + 1, sizeof(int));
  struct bot *bots = calloc(max_bot + 1, sizeof(struct bot));
  for (int i = 0; i < max_bot; i++) {
    bots[i].id = i;
    bots[i].val1 = -1;
    bots[i].val2 = -1;
  }

  for (int j = 0; j < lc; j++) {
    
  }

  // end-of-program cleanup
  for (int i = 0; i < lc; i++)
    free(lines[i]);
  free(lines);
  free(bots);
  free(outputs);
}
