from collections import deque

class NoFoes(Exception):
    pass

class Agent:
    def __init__(self, elf, r, c):
        self.elf = elf
        self.hp = 200
        self.row = r
        self.col = c

    def is_enemy(self, agent):
        return agent != None and self.elf != agent.elf
    
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
    
def dump_map(cave):
    for row in cave:
        s = ""
        a = []
        for sq in row:
            s+= sq.tile if sq.occ == None else "E" if sq.occ.elf else "G"
            if sq.occ != None:
                a.append(sq.occ)
        if len(a) > 0:
            s += "  "
            for m in a:
                s += ("E" if m.elf else "G") + "(" + str(m.hp) + "), "
            s = s[0:-2]
        print(s)

# Find open sqaures adjacent to enemies
def find_enemies(cave, foes, agent):
    in_range = []
    for e in foes:        
        if cave[e.row-1][e.col].clear():
            in_range.append((e.row-1, e.col))
        if cave[e.row+1][e.col].clear():
            in_range.append((e.row+1, e.col))
        if cave[e.row][e.col-1].clear():
            in_range.append((e.row, e.col-1))
        if cave[e.row][e.col+1].clear():
            in_range.append((e.row, e.col+1))
    return in_range

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
        if not (v.row + 1, v.col) in vertexes and cave[v.row + 1][v.col].clear():
            w = Vertex(v.row + 1, v.col, d)
            w.path = v
            vertexes[(w.row, w.col)] = w
            q.append(w)
    return vertexes

def pick_movement(cave, agents, agent):
    foes = [f for f in agents if f.hp > 0 and f.elf != agent.elf]
    if len(foes) == 0:
        raise NoFoes()
    vertexes = shortest_path(cave, agent.row, agent.col)
    enemy_sqs = find_enemies(cave, foes, agent)
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
        return None
    else:
        goal = vertexes[(shortest.row, shortest.col)]
        while goal.path.path != None:
            goal = goal.path

        return goal

def attack(cave, victim):
    victim.hp -= 3
    if victim.hp <= 0:
        cave[victim.row][victim.col].occ = None
        
def evaluate_target(curr, new_v):
    if curr == None:
        return new_v
    elif new_v.hp < curr.hp:
        return new_v
    elif new_v.hp == curr.hp:
        if new_v.row < curr.row or (new_v.row == curr.row and new_v.col < curr.col):
            return new_v
    return curr

def pick_victim(cave, agent):
    # Try to find adjacent opponent with the fewest HP. Ties broken
    # in reading order
    victim = None
    if agent.is_enemy(cave[agent.row - 1][agent.col].occ):
        victim = evaluate_target(victim, cave[agent.row - 1][agent.col].occ)
    if agent.is_enemy(cave[agent.row][agent.col - 1].occ):
        victim = evaluate_target(victim, cave[agent.row][agent.col - 1].occ)
    if agent.is_enemy(cave[agent.row][agent.col + 1].occ):
        victim = evaluate_target(victim, cave[agent.row][agent.col + 1].occ)
    if agent.is_enemy(cave[agent.row + 1][agent.col].occ):
        victim = evaluate_target(victim, cave[agent.row + 1][agent.col].occ)
    return victim
        
def action(cave, agents, agent):
    if agent.hp <= 0:
        return
    
    # Agent will check to see if it is adjacent to an opponent, and if so attack
    # them. Otherwise, move toward the nearest opponent and failing that, do nothing.
    victim = pick_victim(cave, agent)
    if victim != None:
        print("Attack target at:", victim.row, victim.col, victim.elf)
        attack(cave, victim)
    else:
        mv = pick_movement(cave, agents, agent)
        if mv != None:
            print("Move to:", mv.row, mv.col)
            cave[agent.row][agent.col].occ = None
            agent.row = mv.row
            agent.col = mv.col
            cave[agent.row][agent.col].occ = agent

            # After moving, check to see if we can now attack someone
            victim = pick_victim(cave, agent)
            if victim != None:
                print("Attack target at:", victim.row, victim.col, victim.elf)
                attack(cave, victim)

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
            agent = Agent(lines[r][c] == 'E', r, c)
            agents.append(agent)
            row.append(Sqr('.', agent))
    cave.append(row)
    
turn = 0
while True:
    try:
        for agent in agents:
            action(cave, agents, agent)

        agents = [a for a in agents if a.hp > 0]                     
        agents = sorted(agents, key=lambda a:(a.row, a.col))
        turn += 1
        print("After turn:", turn)
        dump_map(cave)
    except NoFoes:
        print("Final score:", sum([a.hp for a in agents if a.hp > 0], 0) * turn, "after", turn)
        dump_map(cave)
        print(len(agents))
        break

