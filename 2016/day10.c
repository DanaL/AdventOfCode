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
  bool processed;
  char *txt;
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

void assign_val_to_bot(struct bot *bot, int val)
{
  if (bot->val1 == -1)
    bot->val1 = val;
  else
    bot->val2 = val;
}

int main(void)
{
  struct rule *rules = NULL;
  int lc = 0;
  char buffer[LINE_W];
  FILE *fp = fopen("inputs/day10.txt", "r");
  while (fgets(buffer, sizeof buffer, fp) != NULL) {
    int len = strlen(buffer);
    if (buffer[len - 1] == '\n') {
      buffer[len - 1] = '\0';
    }

    char *s = malloc(sizeof(char) * (len + 1));
    strcpy(s, buffer);

    rules = realloc(rules, sizeof(struct rule) * (lc +1));
    rules[lc].processed = false;
    rules[lc].txt = s;
    ++lc;
  }
  fclose(fp);

  int max_bot = 0, max_output = 0;
  for (int i = 0; i < lc; i++) {
    int max = find_max_val(rules[i].txt, "bot"); 
    if (max > max_bot)
      max_bot = max;
    max = find_max_val(rules[i].txt, "output");
    if (max > max_output)
      max_output = max;
  }
  int num_of_bots = max_bot + 1;
  int num_of_outputs = max_output + 1;

  int *outputs = calloc(num_of_outputs, sizeof(int));
  struct bot *bots = malloc(num_of_bots * sizeof(struct bot));
  for (int i = 0; i < num_of_bots; i++) {
    bots[i].id = i + 1;
    bots[i].val1 = -1;
    bots[i].val2 = -1;
  }

  while (true) {    
    for (int j = 0; j < lc; j++) {
      printf("%s\n", rules[j].txt);
      if (strncmp("value", rules[j].txt, 5) == 0) {
        int v, id;
        sscanf(rules[j].txt, "value %d goes to bot %d", &v, &id);
        assign_val_to_bot(&bots[id - 1], v);
      }

    }
    break;
  }

  printf("\n");
  for (int j = 0; j < num_of_bots; j++)
    printf("bot %d: %d %d\n", bots[j].id, bots[j].val1, bots[j].val2);
  
  // end-of-program cleanup
  for (int i = 0; i < lc; i++)
    free(rules[i].txt);
  free(rules);
  free(bots);
  free(outputs);
}
