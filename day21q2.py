
results = set()
last_repeat = None
r0 = 3909249
r1 = 0
r2 = 0
r3 = 0
r5 = 0

while True:
    # E
    r2 = r3 | 65536
    r3 = 1397714

    while True:
        r5 = r2 & 255
        r3 += r5
        r3 &= 16777215
        r3 *= 65899
        r3 &= 16777215

        if r2 < 256:
            break

        r5 = 0
        while (r5 + 1) * 256 <= r2:
            r5 += 1
        r2 = r5

        if r0 == r3:
            print("FLAG!!")
    if r3 not in results:
        results.add(r3)
        last_result = r3
    else:
        print(len(results))
        print(last_result)
        break
