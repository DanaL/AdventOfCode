
def dump_scan(scan):
    for row in scan:
        print(row)

def replace_ch_at_index(s, ch, index):
    return s[:index] + ch + s[index+1:]

clay = set()
top = 10_000
bottom = 0
left = 10_000
right = 0
with open("clay_scan_sm.txt", "r") as file:
    for line in file.readlines():
        l, r = line.strip().split(",")
        rng_s = int(r[r.find("=")+1:r.find(".")])
        rng_e = int(r[r.find("..")+2:])
        lv = int(l[2:])

        if l[0] == "x":
            if lv < left: left = lv
            if lv > right: right = lv
            for y in range(rng_s, rng_e+1):
                clay.update([(lv, y)])
                if y > bottom: bottom = y
                if y < top: top = y
        else:
            if lv > bottom: bottom = lv
            if lv < top: top = lv
            for x in range(rng_s, rng_e+1):
                clay.update([(x, lv)])
                if x > right : right = x
                if x < left : left = x
left -= 1
right += 1
width = right - left + 1
col_of_spring = 500

scan = []
for row in range(bottom + 1):
    scan.append('.' * width)
scan[0] = replace_ch_at_index(scan[0], "+", col_of_spring - left)
for sq in clay:
    scan[sq[1]] = replace_ch_at_index(scan[sq[1]], "#", sq[0] - left)
dump_scan(scan)
