class Node:
    def __init__(self):
        self.row = 0
        self.col = 0
        self.exits = { "N":None, "S":None, "E":None, "W":None }
        self.distance = 0
        

def dump_map(nodes):
    coords = [k for k in nodes.keys()]
    coords.sort()
    top = min([k[0] for k in nodes.keys()])
    bottom = max([k[0] for k in nodes.keys()])
    left = min([k[1] for k in nodes.keys()])
    right = max([k[1] for k in nodes.keys()])
    width = (right - left + 1) * 2 + 1
    carte = "#" * (width) + "\n"
    carte *= ((bottom - top + 1) * 2 + 1)

    for coord in coords:
        row = (coord[0] - top + 1) * 2 - 1
        col = (coord[1] - left + 1) * 2 -1
        index = row * (width + 1) + col
        sq = "X" if coord[0] == 0 and coord[1] == 0 else "."
        carte = carte[:index] + sq + carte[index+1:]
        n = nodes[coord]
        if n.exits["E"] != None: carte =carte[:index+1] + "|" + carte[index+2:]
        if n.exits["S"] != None: carte =carte[:index+width+1] + "-" + carte[index+width+2:]
    print(carte)

def opposite_dir(d):
    if d == "N": return "S"
    elif d == "E": return "W"
    elif d == "S": return "N"
    else: return "E"
    
def build_graph(re):
    deltas = { "N":(-1,0), "E":(0,1), "S":(1,0), "W":(0,-1) }
    start = Node()
    nodes = { (0,0):start }
    curr = start
    junctions = []
    
    for c in re[1:]:
        if c in "NESW":
            next_row, next_col = curr.row + deltas[c][0], curr.col + deltas[c][1]
            coord = (next_row, next_col)
            if coord in nodes:
                nn = nodes[coord]
            else:
                nn = Node()
                nn.distance = curr.distance + 1
            nn.row = next_row
            nn.col = next_col
            curr.exits[c] = nn
            nn.exits[opposite_dir(c)] = curr
            nodes[coord] = nn
            curr = nn
        elif c == "(":
            junctions.append((curr.row, curr.col))
        elif c == "|":
            curr = nodes[junctions[-1]]
        elif c == ")":
            junctions.pop()
        elif c == "$":
            break

    dump_map(nodes)
    
    return nodes

with open("floorplan.txt", "r") as f:
    s = f.read().strip()

nodes = build_graph(s)
print("Q1:", max([nodes[k].distance for k in nodes]))
print("Q2:", len([k for k in nodes if nodes[k].distance >= 1000]))
