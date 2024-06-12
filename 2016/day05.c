#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdint.h>

char *pad(char *txt)
{
  int len = strlen(txt);
  int padded_len = len + (63 - len % 64);

  char *padded = calloc(padded_len, sizeof(char));
  memcpy(padded, txt, len);
  padded[len] = 0x80;

  uint64_t og_len_in_bits = (uint64_t)len * 8;
 
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
  int i = padded_len - 8;
  for (int j = 0; j < len_bit_count; j++) {
    padded[i++] = bytes[j];
  }

  return padded;
}

int main(void)
{
  unsigned int s[64] = 
    { 7, 12, 17, 22,  7, 12, 17, 22, 7, 12, 17, 22,  7, 12, 17, 22,
      5,  9, 14, 20,  5,  9, 14, 20, 5,  9, 14, 20,  5,  9, 14, 20,
      4, 11, 16, 23,  4, 11, 16, 23, 4, 11, 16, 23,  4, 11, 16, 23,
      6, 10, 15, 21,  6, 10, 15, 21, 6, 10, 15, 21,  6, 10, 15, 21 };

  unsigned int k[64] = {
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

  char *txt = "Hello, world?0";
  char *padded = pad(txt);

  free(padded);
}
