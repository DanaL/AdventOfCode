from collections import deque

class Agent:
    def __init__(self, elf, r, c):
        self.elf = elf
        self.hp = 20
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

def dump_agents(agents):
    s = ""
    for a in agents:
        ch = "E" if a.elf else "G"
        s += f"{ch}: {a.hp} r={a.row} c={a.col}, \n"
    print(s[0:-2])
    
def dump_map(cave):
    for row in cave:
        print("".join([c.tile if c.occ == None else "E" if c.occ.elf else "G" for c in row]))

# Search for all squares that are in-range of enemies
def find_enemies(cave, agents, agent):
    in_range = []
    for e in agents:
        if e.hp > 0 and e.is_enemy(agent):
            if cave[e.row-1][e.col].clear():
                in_range.append((e.row-1, e.col))
            if cave[e.row+1][e.col].clear():
                in_range.append((e.row+1, e.col))
            if cave[e.row][e.col-1].clear():
                in_range.append((e.row, e.col-1))
            if cave[e.row][e.col+1].clear():
                in_range.append((e.row, e.col+1))
    return in_range

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

def pick_movement(cave, agents, agent):
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
        return None
    else:
        goal = vertexes[(shortest.row, shortest.col)]
        while goal.path.path != None:
            goal = goal.path

        return goal

def attack(cave, victim):
    victim.hp -= 3
    if victim.hp <= 0:
        if victim.elf:
            print("Elf killed!", victim.row, victim.col)
        else:
            print("Goblin killed!", victim.row, victim.col)
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
    elif agent.is_enemy(cave[agent.row][agent.col - 1].occ):
        victim = evaluate_target(victim, cave[agent.row][agent.col - 1].occ)
    elif agent.is_enemy(cave[agent.row][agent.col + 1].occ):
        victim = evaluate_target(victim, cave[agent.row][agent.col + 1].occ)
    elif agent.is_enemy(cave[agent.row + 1][agent.col].occ):
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

agents = []
cave = []
with open("cave_sm.txt") as file:
    lines = file.readlines()

for r in range(len(lines)):
    row = []
    for c in range(len(lines[r])):
        if lines[r][c] in ('#', '.'):
            row.append(Sqr(lines[r][c], None))
        elif lines[r][c] in ('G', 'E'):
            agent = Agent(lines[r][c] == 'E', r, c)
            crappy_pq(agents, agent)
            row.append(Sqr('.', agent))
    cave.append(row)

dump_map(cave)
turn = 0
while True:
    for agent in agents:
        action(cave, agents, agent)

    new_q = []
    elf_count = 0
    goblin_count = 0
    for agent in agents:
        if agent.hp > 0:
            crappy_pq(new_q, agent)
            if agent.elf:
                elf_count += 1
            else:
                goblin_count += 1

    if elf_count == 0 or goblin_count == 0:
        # Battle is over!
        break

    agents = new_q
    print("Fighters remaining:", len(agents))
    turn += 1
    dump_agents(agents)
    dump_map(cave)
    input()
    
print("Final score:", sum([a.hp for a in agents], 0) * turn)
