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

#i = to_idx(4, 7, w)
#print(grid[i])


