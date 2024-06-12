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
    bots[i].id = i;
    bots[i].val1 = -1;
    bots[i].val2 = -1;
  }

  while (true) {
    int processed = 0;
    
    for (int j = 0; j < lc; j++) {
      if (rules[j].processed)
        continue;
      ++processed;
      //printf("%s\n", rules[j].txt);
      if (strncmp("value", rules[j].txt, 5) == 0) {
        int v, id;
        sscanf(rules[j].txt, "value %d goes to bot %d", &v, &id);
        assign_val_to_bot(&bots[id], v);
        rules[j].processed = true;
      }
      else {
        int bot_id = get_val(rules[j].txt, "bot");
        struct bot *bot = &bots[bot_id];
        
        // if the bot doesn't have both its inputs yet, skip it
        if (bot->val1 == -1 || bot->val2 == -1)
          continue;

        int hi, lo;
        if (bot->val1 > bot->val2) {
          hi = bot->val1;
          lo = bot->val2;
        }
        else {
          hi = bot->val2;
          lo = bot->val1;
        }

        // determine low target
        char *sub_s = strstr(rules[j].txt, "low to ") + 7;
        if (strncmp(sub_s, "bot", 3) == 0) {
          // low target is a bot
          int low_bot_id = get_val(sub_s, "bot");
          assign_val_to_bot(&bots[low_bot_id], lo);
        }
        else {
          // low target is output
          int low_output_id = get_val(sub_s, "output");
          outputs[low_output_id] = lo;          
        }

        // now the high target
        sub_s = strstr(sub_s, "high to ") + 8;
        if (strncmp(sub_s, "bot", 3) == 0) {
          // low target is a bot
          int hi_bot_id = get_val(sub_s, "bot");
          assign_val_to_bot(&bots[hi_bot_id], hi);          
        }
        else {
          // low target is output
          int hi_output_id = get_val(sub_s, "output");
          outputs[hi_output_id] = hi;          
        }
      
        rules[j].processed = true;
      }
    }

    if (processed == 0)
      break;
  }

  int t1 = 17, t2 = 61;
  for (int j = 0; j < num_of_bots; j++) {
    if ((bots[j].val1 == t1 && bots[j].val2 == t2) || (bots[j].val1 == t2 && bots[j].val2 == t1)) {
      printf("P1: %d\n", j);
      break;
    } 
  }

  printf("P2: %d\n", outputs[0] * outputs[1] * outputs[2]);

  // end-of-program cleanup
  for (int i = 0; i < lc; i++)
    free(rules[i].txt);
  free(rules);
  free(bots);
  free(outputs);
}