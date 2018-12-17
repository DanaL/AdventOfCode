
def dump_scan(scan):
    for row in scan:
        print(row)

def replace_ch_at_index(s, ch, index):
    return s[:index] + ch + s[index+1:]

def flow_sideways(scan, r, source_col, left, bottom):
    spill = False
    #scan[r] = replace_ch_at_index(scan[r], "|", source_col - left)

    for j in range(source_col - left + 1, len(scan[r])):
        if scan[r][j] in ("#", "~"): break
        scan[r] = replace_ch_at_index(scan[r], "~", j)
        if r + 1 <= bottom and scan[r+1][j] == ".":
            spill = True
            just_add_water(scan, r + 1, j + left, left, bottom)
            break
    for j in range(source_col-left - 1, -1, -1):
        if scan[r][j] in ("#", "~"): break
        scan[r] = replace_ch_at_index(scan[r], "~", j)
        if r + 1 <= bottom and scan[r+1][j] in ("."):
            spill = True
            just_add_water(scan, r + 1, j + left, left, bottom)
            break
    return spill

def just_add_water(scan, r, source_col, left, bottom):
    # 1. Add the flowing water char to the row
    scan[r] = replace_ch_at_index(scan[r], "|", source_col-left)

    # 2. If we can keep flowing downward, flow downward
    if r == bottom: return True
    if scan[r+1][source_col-left] in ("."):
        spill = just_add_water(scan, r + 1, source_col, left, bottom)
        if not spill:
            return flow_sideways(scan, r, source_col, left, bottom)
    elif scan[r+1][source_col-left] in ("#"):
        return flow_sideways(scan, r, source_col, left, bottom)
    elif scan[r+1][source_col-left] in ("~"):
        return flow_sideways(scan, r + 1, source_col, left, bottom)

    return True

clay = set()
top = 10_000
bottom = 0
left = 10_000
right = 0
with open("clay_scan_sm2.txt", "r") as file:
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

try:
    just_add_water(scan, 1, col_of_spring, left, bottom)
except IndexError:
    pass
#just_add_water(scan, 1, col_of_spring, left, bottom)
#just_add_water(scan, 1, col_of_spring, left, bottom)
#just_add_water(scan, 1, col_of_spring, left, bottom)
#just_add_water(scan, 1, col_of_spring, left, bottom)
dump_scan(scan)
