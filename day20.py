class Node:
    def __init__(self):
        self.row = 0
        self.col = 0
        self.exits = { "N":None, "S":None, "E":None, "W":None }


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

    #                 111111 11112222 22222233 33333333
    #      01234567 89012345 67890123 45678901 23456789
    # s = "#######\n#.#.#.#\n#######\n#.#.#.#\n#######\n
    for coord in coords:
        row = (coord[0] - top + 1) * 2 - 1
        col = (coord[1] - left + 1) * 2 -1
        index = row * (width + 1) + col
        carte = carte[:index] + "." + carte[index+1:]
        n = nodes[coord]
        #if n.exits["N"] != None: carte = carte[:index-width+1] + "-" + carte[index-width+2:]
        if n.exits["E"] != None: carte =carte[:index+1] + "|" + carte[index+2:]
        if n.exits["S"] != None: carte =carte[:index+width+1] + "-" + carte[index+width+2:]
    print(carte)

# (-1, 0) = 9  = 1 * 8 + 1 
# (-1, 1) = 11 = 1 * 8 + 3
# (-1, 2) = 13 = 1 * 8 + 5
# ( 0, 0) = 25 = 3 * 8 + 1
# ( 0, 1) = 27 = 3 * 8 + 3
# ( 0, 2) = 29 = 3 * 8 + 5

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

    for c in re[1:]:
        if c in "NESW":
            next_row, next_col = curr.row + deltas[c][0], curr.col + deltas[c][1]
            coord = (next_row, next_col)
            nn = nodes[coord] if coord in nodes else Node()
            nn.row = next_row
            nn.col = next_col
            curr.exits[c] = nn
            nn.exits[opposite_dir(c)] = curr
            nodes[coord] = nn
            curr = nn
        elif c == "$":
            break

    dump_map(nodes)
    
    return start



test_re0 = "^EENWW$"
test_re1 = "^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$"
test_re2 = "^WNE$"
test_re3 = "^NNNNNESSSSSENNNNN$"

build_graph(test_re3)
