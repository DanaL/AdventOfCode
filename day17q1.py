
def dump_scan(scan):
    for r in range(len(scan)):
        print(str(r).rjust(5, ' '), scan[r])

def replace_ch_at_index(s, ch, index):
    return s[:index] + ch + s[index+1:]

def count_water(scan):
    total = 0
    for j in range(5, len(scan)):
        total += sum([1 for ch in scan[j] if ch in ("~", "|")])

    return total

def in_resevoir(scan, r, c):
    sand = 0
    right = False
    for j in range(c, len(scan[r])):
        if scan[r][j] == ".": sand += 1
        if scan[r+1][j] == ".": break
        if scan[r][j] == "#":
            right = True
            break;
    left = False
    for j in range(c, -1, -1):
        if scan[r][j] == ".": sand += 1
        if scan[r+1][j] == ".": break
        if scan[r][j] == "#":
            left = True
            break;

    return right and left and sand > 0

def fill_resevoir(scan, r, source_col, left, bottom, spill_points):
    while in_resevoir(scan, r, source_col - left):
        spill_points.extend(flow_sideways(scan, r, source_col, left, bottom))
        if len(spill_points) > 0: break
        r -= 1
    return r

def pump_water(scan, r, source_col, left, bottom):
    while r <= bottom and scan[r][source_col-left] in (".", "|"):
        scan[r] = replace_ch_at_index(scan[r], "|", source_col-left)
        r += 1

    if r > bottom: return
    
    col = source_col - left
    sq = scan[r][col]
    if sq == "#" or (sq == "~" and in_resevoir(scan, r - 1, col)):
        while True:
            r -= 1
            overflow = False
            # flow right
            for j in range(col, len(scan[r])):
                if scan[r][j] == "#": break
                if scan[r+1][j] in (".", "|"):
                    scan[r] = replace_ch_at_index(scan[r], "|", j)
                    pump_water(scan, r + 1, j + left, left, bottom)
                    overflow = True
                    break
                else:
                    scan[r] = replace_ch_at_index(scan[r], "~", j)
            # flow left
            for j in range(col, -1, -1):
                if scan[r][j] == "#": break
                if scan[r+1][j] in (".", "|"):
                    scan[r] = replace_ch_at_index(scan[r], "|", j)
                    pump_water(scan, r + 1, j + left, left, bottom)
                    overflow = True
                    break
                else:
                    scan[r] = replace_ch_at_index(scan[r], "~", j)
            if overflow: return
        
clay = set()
top = 10_000
bottom = 0
left = 10_000
right = 0
with open("clay_scan.txt", "r") as file:
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

pump_water(scan, 1, col_of_spring, left, bottom)

#dump_scan(scan)
print(top, bottom)
print(count_water(scan))
