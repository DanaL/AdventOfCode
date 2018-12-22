from collections import defaultdict
from enum import Enum

class ToolState(Enum):
    TORCH = 0
    GEAR = 1
    NONE = 2

moves = [(0, -1), (1, 0), (0, 1), (-1, 0)]

class Terrains(Enum):
    ROCK = "."
    WATER = "="
    NARROW = "|"

def calc_edges(next_sq, state, nr, nc):
    edges = []
    if state == ToolState.TORCH:
        if next_sq == "=":
            # Using a torch, have to switch to move to water
            edges.append((ToolState.NONE, 8, (nc, nr)))
            edges.append((ToolState.GEAR, 8, (nc, nr)))
        elif next_sq == ".":
            edges.append((ToolState.TORCH, 1, (nc ,nr)))
            edges.append((ToolState.GEAR, 8, (nc, nr)))
        elif next_sq == "|":
            edges.append((ToolState.TORCH, 1, (nc, nr)))
            edges.append((ToolState.NONE, 8, (nc, nr)))
    elif state == ToolState.GEAR:
        if next_sq == "=":
            edges.append((ToolState.GEAR, 1, (nc, nr)))
            edges.append((ToolState.NONE, 8, (nc, nr)))
        elif next_sq == "|":
            edges.append((ToolState.TORCH, 8, (nc, nr)))
            edges.append((ToolState.NONE, 8, (nc, nr)))
        elif next_sq == ".":
            edges.append((ToolState.GEAR, 1, (nc, nr)))
            edges.append((ToolState.TORCH, 8, (nc, nr)))
    elif state == ToolState.NONE:
        if next_sq == "=":
            edges.append((ToolState.NONE, 1, (nc, nr)))
            edges.append((ToolState.GEAR, 8, (nc, nr)))
        elif next_sq == ".":
            edges.append((ToolState.GEAR, 8, (nc, nr)))
            edges.append((ToolState.TORCH, 8, (nc, nr)))
        elif next_sq == "|":
            edges.append((ToolState.NONE, 1, (nc, nr)))
            edges.append((ToolState.TORCH, 8, (nc, nr)))

    return edges

def in_bounds(x, y, depth, width):
    if x < 0 or y < 0 or x >= width or y >= depth:
        return False
    return True
                        
def gen_adjacencies(cave, vertexes):
    global moves
    depth = len(cave)
    width = len(cave[0])
    
    for r in range(len(cave)):
        for c in range(len(cave[r])):
            # For each of the terrain types, we can be in two tool states.
            # Each of the two tool states can have two vertexes to the adjoining squares
            sq = cave[r][c]
            if sq == ".":
                states = (ToolState.GEAR, ToolState.TORCH)
            elif sq == "|":
                states = (ToolState.NONE, ToolState.TORCH)
            elif sq == "=":
                states = (ToolState.NONE, ToolState.GEAR)

            for state in states:
                v = (state, (c, r))
                for mv in moves:
                    nr, nc = r+mv[1], c+mv[0]
                    if not in_bounds(nc, nr, depth, width): continue
                    vertexes[v].extend(calc_edges(cave[nr][nc], state, nr, nc))
                              
with open("rescue_cave_sm.txt", "r") as f:
    cave = [line.strip() for line in f.readlines()]

cave = [".|", "=."]
tx = 10
ty = 10

initial = (ToolState.TORCH, (0, 0))
vertexes = defaultdict(list)
gen_adjacencies(cave, vertexes)
for k in vertexes:
    print(k)
    for e in vertexes[k]:
        print("    ", e)


#for edge in vertexes[(ToolState.GEAR, (1, 0))]:
#    print(edge)
