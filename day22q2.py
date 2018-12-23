import sys
from enum import Enum
from heapq import *

class Vertex:
    def __init__(self):
        self.adj = []
        self.known = False
        self.distance = sys.maxsize
        self.prev = None
        
class ToolState(Enum):
    TORCH = 0
    GEAR = 1
    NONE = 2

    def __lt__(self, other):
        return True
    
moves = [(0, -1), (1, 0), (0, 1), (-1, 0)]

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
                    if v not in vertexes:
                        vertexes[v] = Vertex()
                    vertexes[v].adj.extend(calc_edges(cave[nr][nc], state, nr, nc))

def dijkstra(vertexes, initial, goal):
    q = [[0, initial]]
    shortest = sys.maxsize
    prev_node = None
    
    while q:
        n = heappop(q)
        v = vertexes[n[1]]
        v.known = True

        #print(n[1])
        for edge in v.adj:
            # adjacencies stored as (ToolState, distance, coord)
            key = (edge[0], edge[2])
            kv = vertexes[key]
            cost = v.distance + edge[1]
            
            if cost < kv.distance:
                kv.distance = cost
                kv.prev = n[1]
                #print("   ", key, cost)
            if kv.known: continue
            kv.known = True

            if edge[2] == goal:
                print("Cost:", n[1], cost, edge)
            if edge[2] == goal and cost < shortest:
                shortest = cost
                vertexes[key].prev = n[1]
                prev_node = key
            heappush(q, [cost, key])
            
            """
            
            
            
            
            kv.prev = n[1]
            
            if edge[2] == goal and cost < shortest:
                print("Goal?", edge, goal)
                print("  ", n)
                prev_node = k
                shortest = cost
                vertexes[k].prev = n[1]
            if kv.distance > shortest:
                # Don't bother continuing if a path is going to
                # be bigger than the currently known shortest path
                continue

            heap_item = [cost, k]
            
            """
    return (shortest, prev_node)

def dump_map(cave):
    for r in cave:
        print(r)
        
def dump_map_with_path(cave, vertexes, v, goal):
    path = set()
    nodes = []
    while v != None:
        path.add(v[1])
        node = vertexes[v]
        nodes.append(v)
        v = node.prev
    nodes.reverse()

    for n in nodes:
        print(n, vertexes[n].distance)


    
    for r in range(len(cave)):
        s = ""
        for c in range(len(cave[r])):
            if (c, r) == goal:
                s += 'X'
            elif (c, r) == (0, 0):
                s += 'M'
            elif (c, r) in path:
                s += '*'
            else:
                s += cave[r][c]
        print(s)
        
with open("rescue_cave_sm.txt", "r") as f:
    cave = [line.strip() for line in f.readlines()]

cave = ['.=|', '.=.', '=|.']
tx = 2
ty = 2

initial = (ToolState.TORCH, (0, 0))
vertexes = {}
gen_adjacencies(cave, vertexes)

v = vertexes[initial]
v.distance = 0

res = dijkstra(vertexes, initial, (tx, ty))
print("Shortest:", res[0], res[1])
dump_map(cave)
print("")
dump_map_with_path(cave, vertexes, res[1], (tx, ty))
#for edge in vertexes[(ToolState.GEAR, (1, 0))]:
#    print(edge)

#for e in vertexes[(ToolState.TORCH, (0, 0))].adj:
#    print(e)
#print("")
#for e in vertexes[(ToolState.NONE, (1, 0))].adj:
#    print(e)
