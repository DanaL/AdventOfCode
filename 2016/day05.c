#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdint.h>

void fuck_off(uint8_t *arr)
{
  for (int j = 0; j < 64; j++)
    printf("%u ", arr[j]);
  printf("\n");
}

uint8_t *pad(uint8_t *txt, int *padded_len)
{
  int len = strlen(txt);
  *padded_len = len + (64 - len % 64);

  uint8_t *padded = calloc(*padded_len, sizeof(uint8_t));
  fuck_off(padded);
  memcpy(padded, txt, len);
  fuck_off(padded);
  padded[len] = 0x80;
  fuck_off(padded);

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
  int i = *padded_len - 8;
  for (int j = 0; j < len_bit_count; j++) {
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

int main(void)
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
    
  printf("before: %u %u %u %u\n", a0, b0, c0, d0);
  char *txt = "Hello, world?0";
  int padded_len = 0;
  uint8_t *padded = pad(txt, &padded_len);

  for (int j = 0; j < 64; j++)
    printf("%02d %u\n", j, padded[j]);

  // Proceed in 512 bit (64 byte for my purposes) chunks
  int offset = 0;
  while (offset < padded_len) {
    uint32_t a = a0, b = b0, c = c0, d = d0;

    printf("%lu %lu %lu %lu\n", a0, b0, c0, d0);
    // Convert the 512 bit chunk into 16, 32-bit words
    uint32_t words[16];
    for (int j = 0; j < 64; j += 4) {
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


  printf("after %u %u %u %u\n", a0, b0, c0, d0);

  free(padded);
}
