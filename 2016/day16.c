#include <stdio.h>
#include <stdlib.h>
#include <string.h>

char *iterate(const char *a, size_t len)
{
  size_t b_len = 2 * len + 2;

  char *result = malloc(b_len * sizeof(char));
  memcpy(result, a, len);
  result[len] = '0';

  for (size_t j = 0; j < len; j++) {
    result[b_len - 2 - j] = a[j] == '0' ? '1' : '0';
  }

  result[b_len - 1] = '\0';

  return result;
}

void p1(const char *initial, size_t data_len)
{
  char data[1000];
  strcpy(data, initial);
  data[5] = '\0';
  size_t len = strlen(data);

  while (len < data_len) {
    char *b = iterate(data, len);
    len = strlen(b);
    strcpy(data, b);
    free(b);
  }

  data[data_len] = '\0';
  //printf("%s\n", data);

  char checksum[1000];
  memcpy(checksum, data, data_len);
  char tmp[1000];
  size_t checksum_len = data_len;
  while (checksum_len % 2 == 0) {
    printf("%zu\n", checksum_len);
    for (int j = 0; j < checksum_len/2; j++) {
      char bit = checksum[2*j] == checksum[2*j+1] ? '1' : '0';
      tmp[j] = bit;
    }
    checksum_len /= 2;
    memcpy(checksum, tmp, checksum_len);
  }
  checksum[checksum_len] = '\0';

  printf("%s\n", checksum);
}

int main(void)
{
  p1("10111011111001111", 272);
}
