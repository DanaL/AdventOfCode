ROCKY = 0
WET = 1
NARROW = 2

def region_ch(rt):
    if rt == ROCKY: return '.'
    elif rt == WET: return '='
    elif rt == NARROW: return '|'
    
def dump_map(carte):
    for row in carte:
        s = "".join([region_ch(c) for c in row])
        print(s)
                    
def e_lvl_from_gi(gi, depth):
    return (gi + depth) % 20183

def region_type(el):
    if el % 3 == 0: return ROCKY
    elif el % 3 == 1: return WET
    elif el % 3 == 2: return NARROW

tx = 13
ty = 704
depth = 9465

#tx = 10
#ty = 10
#depth = 510

# initial setup of geologic indices Y = 0
cave = [[x * 16807 for x in range(0, tx+1)]]

for y in range(1, ty + 1):
    row = [y * 48271]
    for x in range(1, tx + 1):
        row.append(e_lvl_from_gi(row[-1], depth) * e_lvl_from_gi(cave[y-1][x], depth))
    cave.append(row)
cave[ty][tx] = 0

carte = []
for row in cave:
    r = [region_type(e_lvl_from_gi(c, depth)) for c in row]
    carte.append(r)

dump_map(carte)

s = 0
for row in carte:
    s += sum(row)
print(s)


