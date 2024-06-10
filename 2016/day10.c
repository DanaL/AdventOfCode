#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define BOT 0
#define OUTPUT 1
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

int main(void)
{
  struct bot *bots = NULL;
  int bot_count = 0;
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

  for (int i = 0; i < lc; i++)
    printf("%s\n", lines[i]);
  /*
  for (int i = 0; i < bot_count; i++)
      printf("bot %d:\n  val 1: %d\n  val 2: %d\n", bots[i].id, bots[i].val1, bots[i].val2);

  for (int i = 0; i < rule_count; i++)
    printf("bot %d gives low to %d and hi to %d\n", rules[i].bot_id, rules[i].lo, rules[i].hi);

  free(bots);
  free(rules);
  */

  for (int i = 0; i < lc; i++)
    free(lines[i]);
  free(lines);
}
