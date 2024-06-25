#include <stdio.h>

#define D1(n) ((n + 1) % 13 == 0)
#define D2(n) ((n + 10) % 19 == 0)
#define D3(n) ((n + 2) % 3 == 0)
#define D4(n) ((n + 1) % 7 == 0)
#define D5(n) ((n + 3) % 5 == 0)
#define D6(n) ((n + 5) % 17 == 0)
#define D7(n) ((n) % 11 == 0)

void p1(void)
{
  int n = 0;
  while (1) {
    if (D1(n+1) && D2(n+2) && D3(n+3) && D4(n+4) && D5(n+5) && D6(n+6)) {
      printf("P1: %d\n", n);
      break;
    }
    ++n;
  }
}

void p2(void)
{
  int n = 0;
  while (1) {
    if (D1(n+1) && D2(n+2) && D3(n+3) && D4(n+4) && D5(n+5) && D6(n+6) && D7(n+7)) {
      printf("P2: %d\n", n);
      break;
    }
    ++n;
  }
}

int main(void)
{
  p1();
  p2();
}
