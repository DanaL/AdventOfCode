def fetch_input():
  w = 0        
  s = ""
  with open("inputs/day12.txt") as f:
    for line in f.readlines():
      w = len(line)
      s += line.strip()
  return s, w
def to_idx(r, c, w):
    return r * w + c

def in_bounds(r, c, w):
    return 0 <= r < w and 0 <= c < w

def count_borders(grid, r, c, w):
    borders = 0
    adj = [(-1, 0), (1, 0), (0, -1), (0, 1)]
    i = to_idx(r, c, w)
    ch = grid[i]
    for dr, dc in adj:
        nr, nc = r + dr, c + dc
        if not in_bounds(nr, nc, w) or grid[to_idx(nr, nc, w)] != ch:
            borders += 1
    return borders

def floodfill(grid, r, c, w):
  ch = grid[to_idx(r, c, w)]
  q = [(r, c)]
  region = {(r, c)}
  visited = set()
  while q:
    cr, cc = q.pop(0)
    if (cr, cc) in visited:
      continue
    
    visited.add((cr, cc))
    for dr, dc in [(-1, 0), (1, 0), (0, -1), (0, 1)]:
        nr, nc = cr + dr, cc + dc
        if in_bounds(nr, nc, w) and grid[to_idx(nr, nc, w)] == ch:
          region.add((nr, nc))
          q.append((nr, nc))

  return region

def cost_of_region(region, grid, w):
  cost = 0
  for r, c in region:
    cost += count_borders(grid, r, c, w)

  return len(region) * cost

def p1(): 
  grid, w = fetch_input()
  
  total_cost = 0
  visited = set()
  for r in range(w):
    for c in range(w):
       if (r, c) not in visited:
          region = floodfill(grid, r, c, w)
          visited.update(region)
          total_cost += cost_of_region(region, grid, w)
  print("P1:", total_cost)
  
p1()

def count_sides(fences):
  count = 0
  
  sides_dict = {}
  for l, _ in fences:
    sides_dict[l] = []

  for l, n in fences:
    sides_dict[l].append(n)

  for l in sides_dict:
    sides_dict[l].sort()
    sides = [sides_dict[l][0]]

    for i in range(1, len(sides_dict[l])):
      if sides_dict[l][i] == sides[-1] + 1:
        sides[-1] = sides_dict[l][i]
      else:
        sides.append(sides_dict[l][i])

    count += len(sides)

  return count

def count_region_sides(region, grid, w):
  north = set()
  south = set()
  east = set()
  west = set()

  sq = next(iter(region))
  ch = grid[to_idx(sq[0], sq[1], w)]
  
  for r, c in region:
    if not in_bounds(r - 1, c, w) or grid[to_idx(r - 1, c, w)] != ch:
       north.add((r, c))
    if not in_bounds(r + 1, c, w) or grid[to_idx(r + 1, c, w)] != ch:
       south.add((r,  c))
    if not in_bounds(r, c - 1, w) or grid[to_idx(r, c - 1, w)] != ch:
       west.add((c, r))
    if not in_bounds(r, c + 1, w) or grid[to_idx(r, c + 1, w)] != ch:
       east.add((c, r))

  return count_sides(north) + count_sides(south) + count_sides(east) + count_sides(west)

def p2():
  grid, w = fetch_input()
  
  cost = 0
  visited = set()
  for r in range(w):
    for c in range(w):
       if (r, c) not in visited:
          region = floodfill(grid, r, c, w)
          visited.update(region)
          
          sides =count_region_sides(region, grid, w)
          cost += sides * len(region)
  print("P2:", cost)
  
p2()

