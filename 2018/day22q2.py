import sys
from enum import Enum
from heapq import *

class ToolState(Enum):
    TORCH = 0
    GEAR = 1
    NONE = 2

    def __lt__(self, other):
        return True
    
moves = [(0, -1), (1, 0), (0, 1), (-1, 0)]

erosion = { }
def g_index(x, y):
    global erosion
    if x == 0 and y == 0: return 0
    if x == 13 and y == 704: return 0
    if y == 0: return x * 16807
    if x == 0: return y * 48271

    e1 = e_lvl_from_gi(x-1,y)
    e2 = e_lvl_from_gi(x, y-1)
    return e1 * e2

def e_lvl_from_gi(x, y):
    global erosion
    if (x, y) in erosion:
        return erosion[(x, y)]

    e = (g_index(x, y) + 9465) % 20183
    erosion[(x, y)] = e
    return e

def get_ch(x, y):
    global erosion
    e = e_lvl_from_gi(x, y)        
    em = e % 3
    if em == 0: return "."
    elif em == 1: return "="
    elif em == 2: return "|"
    
def get_adj(q, visited, cost, tool, coord):
    global moves
    for mv in moves:
        new_coord = (coord[0] + mv[0], coord[1] + mv[1])
        if new_coord[0] < 0 or new_coord[1] < 0: continue
        sq = get_ch(new_coord[0], new_coord[1])
        if sq == "." and tool == ToolState.NONE: continue
        if sq == "|" and tool == ToolState.GEAR: continue
        if sq == "=" and tool == ToolState.TORCH: continue
        if (tool, new_coord) in visited and visited[(tool, new_coord)] <= cost: continue

        visited[(tool, new_coord)] = cost
        heappush(q, [cost, tool, new_coord])
        
def find_path(goal, initial):
    visited = { initial:0 }
    q = [(0, initial[0], initial[1])] # cost, tool, co-ord

    while q:
        cost, tool, coord = heappop(q)
        sq = get_ch(coord[0], coord[1])
        if (tool, coord) == (ToolState.TORCH, goal):
            return cost
        
        get_adj(q, visited, cost + 1, tool, coord)

        if sq == "." and tool == ToolState.TORCH:
            get_adj(q, visited, cost + 8, ToolState.GEAR, coord)
        elif sq == "." and tool == ToolState.GEAR:
            get_adj(q, visited, cost + 8, ToolState.TORCH, coord)
        elif sq == "|" and tool == ToolState.NONE:
            get_adj(q, visited, cost + 8, ToolState.TORCH, coord)
        elif sq == "|" and tool == ToolState.TORCH:
            get_adj(q, visited, cost + 8, ToolState.NONE, coord)
        elif sq == "=" and tool == ToolState.GEAR:
            get_adj(q, visited, cost + 8, ToolState.NONE, coord)
        elif sq == "=" and tool == ToolState.NONE:
            get_adj(q, visited, cost + 8, ToolState.GEAR, coord)
        

tx = 13
ty = 704

initial = (ToolState.TORCH, (0, 0))

result = find_path((tx, ty), initial)
print(result)

