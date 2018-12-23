def manhattan(a, b):
    return abs(a[0] - b[0]) + abs(a[1] - b[1]) + abs(a[2] - b[2])

widest = 0
strongest = None
nanobots = []
with open("nanobots.txt", "r") as f:
    for line in f.readlines():
        coords = [int(n) for n in line[line.find("<")+1:line.find(">")].split(",")]
        r = int(line[line.find("r=")+2:].strip())
        nanobots.append(coords)
        if r > widest:
            strongest = coords
            widest = r

bx, by, bz = strongest
in_range = 0
for x, y, z in nanobots:
    if abs(x - bx) + abs(y - by) + abs(z - bz) <= widest:
        in_range += 1
    
print(in_range)
