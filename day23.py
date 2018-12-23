
def q1(nanobots):
    widest = 0
    strongest = None

    for x, y, z, r in nanobots:
        if r > widest:
            widest = r
            strongest = [x, y, z, r]
            
    bx, by, bz, br = strongest
    in_range = 0
    for x, y, z, _ in nanobots:
        if abs(x - bx) + abs(y - by) + abs(z - bz) <= widest:
            in_range += 1
    
    print(in_range)

nanobots = []
with open("nanobots.txt", "r") as f:
    for line in f.readlines():
        coords = [int(n) for n in line[line.find("<")+1:line.find(">")].split(",")]
        coords.append(int(line[line.find("r=")+2:].strip()))
        nanobots.append(coords)
        
q1(nanobots)
