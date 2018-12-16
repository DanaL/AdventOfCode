from collections import deque

class Agent:
    def __init__(self, ch, r, c):
        self.ch = ch
        self.hp = 200
        self.row = r
        self.col = c

class Sqr:
    def __init__(self, tile, occ):
        self.tile = tile
        self.occ = occ;

    def clear(self):
        return self.occ == None and self.tile == '.'

class Vertex:
    def __init__(self, r, c, d):
        self.row = r
        self.col = c
        self.path = None
        self.distance = d

def dump_agents(agents):
    s = ""
    for a in agents:
        s += f"{a.ch}: {a.hp} r={a.row} c={a.col}, \n"
    print(s[0:-2])
    
def dump_map(cave):
    for row in cave:
        print("".join([c.tile if c.occ == None else c.occ.ch for c in row]))

# Search for all squares that are in-range of enemies
def find_enemies(cave, agents, agent):
    in_range = []
    for e in agents:
        if e.ch != agent.ch:
            if cave[e.row-1][e.col].clear():
                in_range.append((e.row-1, e.col))
            if cave[e.row+1][e.col].clear():
                in_range.append((e.row+1, e.col))
            if cave[e.row][e.col-1].clear():
                in_range.append((e.row, e.col-1))
            if cave[e.row][e.col+1].clear():
                in_range.append((e.row, e.col+1))
    return in_range

def pick_move(cave, agents, agent):
    vertexes = shortest_path(cave, agent.row, agent.col)
    enemy_sqs = find_enemies(cave, agents, agent)
    shortest = Vertex(-1, -1, 10_000)
    for sq in enemy_sqs:
        if not sq in vertexes:
            continue # unreachacle square
        v = vertexes[sq]
        if v.distance < shortest.distance:
            shortest = v
        elif v.distance == shortest.distance:
            # They want us to solve ties with 'reading' order: top-to-bottom, left-to-right
            if v.row < shortest.row or (v.row == shortest.row and v.col < shortest.col):
                shortest = v

    if shortest.distance == 10_000:
        print("No viable move found!")
    else:
        goal = vertexes[(shortest.row, shortest.col)]
    
        while goal.distance > 0:
            cave[goal.row][goal.col].tile = "*"
            goal = goal.path
        #dump_map(cave)
        
def crappy_pq(q, agent):
    loc = 0
    while loc < len(q):
        if agent.row < q[loc].row or (agent.row == q[loc].row and agent.col < q[loc].col):
            break
        loc += 1
    q.insert(loc, agent)

# Unweighted Shortest Path algorithm from Data Structures & Algorithm Analysis In C++ 2nd Ed
# by Mark Allen Weiss (pages 335-339)
#
# This will find the shortest path from start to every other reachacble square in the cave
def shortest_path(cave, start_r, start_c):
    s = Vertex(start_r, start_c, 0)
    q = deque([s])
    vertexes = {(start_r, start_c): s}
    while len(q) > 0:
        v = q.popleft()
        d = vertexes[(v.row, v.col)].distance + 1
        
        if not (v.row - 1, v.col) in vertexes and cave[v.row - 1][v.col].clear():
            w = Vertex(v.row - 1, v.col, d)
            w.path = v
            vertexes[(w.row, w.col)] = w
            q.append(w)
        if not (v.row + 1, v.col) in vertexes and cave[v.row + 1][v.col].clear():
            w = Vertex(v.row + 1, v.col, d)
            w.path = v
            vertexes[(w.row, w.col)] = w
            q.append(w)
        if not (v.row, v.col - 1) in vertexes and cave[v.row][v.col - 1].clear():
            w = Vertex(v.row, v.col - 1, d)
            w.path = v
            vertexes[(w.row, w.col)] = w
            q.append(w)
        if not (v.row, v.col + 1) in vertexes and cave[v.row][v.col + 1].clear():
            w = Vertex(v.row, v.col + 1, d)
            w.path = v
            vertexes[(w.row, w.col)] = w
            q.append(w)
    return vertexes
     
agents = []
cave = []
with open("cave.txt") as file:
    lines = file.readlines()

for r in range(len(lines)):
    row = []
    for c in range(len(lines[r])):
        if lines[r][c] in ('#', '.'):
            row.append(Sqr(lines[r][c], None))
        elif lines[r][c] in ('G', 'E'):
            agent = Agent(lines[r][c], r, c)
            crappy_pq(agents, agent)
            row.append(Sqr('.', agent))
    cave.append(row)

dump_map(cave)
#dump_agents(agents)
print(agents[1].row, agents[1].col)
pick_move(cave, agents, agents[1])
pick_move(cave, agents, agents[-1])
pick_move(cave, agents, agents[-2])
pick_move(cave, agents, agents[-3])
dump_map(cave)

