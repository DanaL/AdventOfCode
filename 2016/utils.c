#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdint.h>
#include <limits.h>

#include "utils.h"

char **read_all_lines(const char *filename, size_t *line_count)
{
  char **lines = NULL;
  FILE *fp = fopen(filename, "r");
  char buffer[100];
  *line_count = 0;
  while (fgets(buffer, sizeof buffer, fp) != NULL) {
    size_t bc = strlen(buffer);
    if (buffer[bc - 1] == '\n')
      buffer[bc - 1] = '\0';
    
    lines = realloc(lines, ++(*line_count) * sizeof(char*));
    lines[*line_count - 1] = malloc((bc + 1) * sizeof(char));
    strcpy(lines[*line_count - 1], buffer);
  }

  fclose(fp);

  return lines;
}

void lines_free(char **lines, size_t line_count)
{
  for (size_t j = 0; j < line_count; j++)
    free(lines[j]);
  free(lines);
}

uint8_t *pad(const uint8_t *txt, size_t *padded_len)
{
  size_t len = strlen((char*)txt);
  size_t mod_len = len % 64 + 1;
  size_t padding_length = mod_len <= 56 ? 56 - mod_len : 64 - (mod_len - 56);
  
  *padded_len = len + 1 + padding_length + 8;
  uint8_t *padded = calloc(*padded_len, sizeof(uint8_t));
  memcpy(padded, txt, len);
  padded[len] = 0x80;
  
  uint64_t og_len_in_bits = (uint64_t)len * 8;
  
  // Calc representation of original len in bits in little endian
  // If the length of the text to be hashed can't be represented in 64 bits,
  // we'd use the 64 least significant bits but I'm not going to worry about
  // that here.
  int len_bit_count = 0;
  uint8_t bytes[8];
  while (og_len_in_bits > 0) {
    bytes[len_bit_count++] = og_len_in_bits & 255;
    og_len_in_bits = og_len_in_bits >> 8;
  }

  // Copy the byte representation into the last 8 bytes
  int i = *padded_len - 8;
  for (size_t j = 0; j < len_bit_count; j++) {
    padded[i++] = bytes[j];
  }

  return padded;
}

uint32_t left_rotate(uint32_t x, uint32_t shift) 
{
  uint32_t left = x << shift;
  uint32_t right = x >> (32 - shift);

  return left | right;
}

uint32_t to_32b_word(uint32_t a, uint32_t b, uint32_t c, uint32_t d) 
{
  uint32_t word = 0;
  word += a << 24;
  word += b << 16;
  word += c << 8;
  word += d;

  return word;
}

// 01111011 10000010 00001111 10011001
// 153 15 130 123
char *word_to_bytes(uint32_t x, char *s)
{
  uint8_t a = (x & 255);
  uint8_t b = (x & 65280) >> 8;
  uint8_t c = (x & 16711680) >> 16;
  uint8_t d = (x & 4278190080) >> 24;

  sprintf(s, "%02x%02x%02x%02x", a, b, c, d);
  s[8] = '\0';
  
  return s;
}

char *md5(const char *txt)
{
  uint32_t s[64] = 
    { 7, 12, 17, 22,  7, 12, 17, 22, 7, 12, 17, 22,  7, 12, 17, 22,
      5,  9, 14, 20,  5,  9, 14, 20, 5,  9, 14, 20,  5,  9, 14, 20,
      4, 11, 16, 23,  4, 11, 16, 23, 4, 11, 16, 23,  4, 11, 16, 23,
      6, 10, 15, 21,  6, 10, 15, 21, 6, 10, 15, 21,  6, 10, 15, 21 };

  uint32_t k[64] = {
    0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee,
    0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501,
    0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be,
    0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821,
    0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa,
    0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8,
    0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed,
    0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a,
    0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c,
    0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70,
    0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05,
    0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665,
    0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039,
    0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1,
    0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1,
    0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391
  };

  uint32_t a0 = 0x67452301;
  uint32_t b0 = 0xefcdab89;
  uint32_t c0 = 0x98badcfe;
  uint32_t d0 = 0x10325476;

  size_t padded_len = 0;
  uint8_t *padded = pad((const uint8_t *)txt, &padded_len);

  // Proceed in 512 bit (64 byte for my purposes) chunks
  int offset = 0;
  while (offset < padded_len) {
    uint32_t a = a0, b = b0, c = c0, d = d0;

    // Convert the 512 bit chunk into 16, 32-bit words
    uint32_t words[16];
    for (size_t j = 0; j < 64; j += 4) {
      uint32_t word = to_32b_word(padded[offset+j+3], padded[offset+j+2], padded[offset+j+1], padded[offset+j]);
      words[j / 4] = word; 
    }

    for (int i = 0; i < 64; i++) {
      uint32_t f, g;

      if (i < 16) {
        f = (b & c) | ((~b) & d);
        g = i;
      }
      else if (i < 32) {
        f = (d & b) | ((~d) & c);
        g = (5 * i + 1) % 16;
      }
      else if (i < 48) {
        f = b ^ c ^ d;
        g = (3 * i + 5) % 16;
      }
      else {
        f = c ^ (b | (~d));
        g = (7 * i) % 16;
      }

      f = f + a + k[i] + words[g];
      a = d;
      d = c;
      c = b;
      b = b + left_rotate(f, s[i]);
    }
   
    a0 += a;
    b0 += b;
    c0 += c;
    d0 += d;

    offset += 64;
  }

  char *hex_str = malloc(33);
  char hex_bytes[9];
  offset = 0;
  uint32_t bytes[] = { a0, b0, c0, d0 };
  for (size_t j = 0; j < 4; j++) {
    word_to_bytes(bytes[j], hex_bytes);
    strcpy(hex_str + offset, hex_bytes);
    offset += 8;
  }
  free(padded);

  return hex_str;
}

// heap implementation

#define HT_SIZE 1000
#define HEAP_PARENT(i) ((i) - 1) / 2

struct heap *heap_new(void)
{
  struct heap *h = malloc(sizeof(struct heap));
  h->num_of_elts = 0;
  h->table_size = HT_SIZE;
  h->table = calloc(HT_SIZE, sizeof(void*));

  return h;
}

void min_heap_push(struct heap *h, void *item, int (*priority)(const void *))
{
  if (h->num_of_elts == h->table_size) {
    // need to expand the table
    h->table_size += HT_SIZE;
    h->table = realloc(h->table, h->table_size * sizeof(void*));
    printf("FLAG\n");
  }

  size_t i = h->num_of_elts;
  h->table[i] = item;
  int p = (*priority)(item);
  
  while (i > 0 && p < (*priority)(h->table[HEAP_PARENT(i)])) {
    size_t parent = HEAP_PARENT(i);
    struct state *tmp = h->table[parent];
    h->table[parent] = item;
    h->table[i] = tmp;
    i = parent;
  }

  ++h->num_of_elts;
}

void min_heapify(struct heap *h, size_t i, int (*priority)(const void *))
{
  int left_child_priority = INT_MAX;
  size_t left_child_i = 2 * i + 1;
  if (left_child_i < h->num_of_elts) {
    left_child_priority = (*priority)(h->table[left_child_i]);
  }
  int right_child_priority = INT_MAX;
  size_t right_child_i = 2 * i + 2;
  if (right_child_i < h->num_of_elts) {
    right_child_priority = (*priority)(h->table[right_child_i]);
  }

  int p = (*priority)(h->table[i]);
  if (p > left_child_priority || p > right_child_priority) {
    if (left_child_priority < right_child_priority) {
      // swap i and left and reheapify left branch
      struct state *tmp = h->table[left_child_i];
      h->table[left_child_i] = h->table[i];
      h->table[i] = tmp;
      min_heapify(h, left_child_i, priority);
    }
    else {
      // swap i and right and reheapify right branch
      struct state *tmp = h->table[right_child_i];
      h->table[right_child_i] = h->table[i];
      h->table[i] = tmp;
      min_heapify(h, right_child_i, priority);
    }
  }
}

void *min_heap_pop(struct heap *h, int (*priority)(const void *))
{
  void *result = h->table[0];
  --h->num_of_elts;

  h->table[0] = h->table[h->num_of_elts];
  h->table[h->num_of_elts] = NULL;

  if (h->num_of_elts > 0)
    min_heapify(h, 0, priority);

  return result;
}