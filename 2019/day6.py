def walk_graph(nodes, curr, depth):
	steps = depth 
	for n in nodes[curr]:
		steps += walk_graph(nodes, n, depth+1) 
	return steps

with open("./inputs/day6.txt") as file:
	lines = file.readlines()

#lines = "COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L".split("\n");
#lines = "COM)B\nB)C\nC)D".split("\n")

# Part One, generate a graph of the nodes and then walk through each path
# counting the steps. An up-tree would be been better, I think, especially
# after having seen Part Two
nodes = { }
for line in lines:
	p = line.strip().split(")")
	if not p[0] in nodes:
		nodes[p[0]] = []
	if not p[1] in nodes:
		nodes[p[1]] = []
	nodes[p[0]].append(p[1])
print("Q1: ", walk_graph(nodes, "COM", 0))

# For Part Two, an up-tree felt way easier. Then, just walk up the tree 
# starting from YOU and SAN, recording the count it takes to visit each
# node. After that it's just finding the intersections of the sets of
# keys, summing each of their distances and finding the smallest
def upward_path(uptree, node):
	path = { node:0 }
	s = 0
	while node in uptree:
		node = uptree[node]
		s += 1
		path[node] = s
	return path

#lines = "COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L\nK)YOU\nI)SAN".split("\n");

uptree = {}
for line in lines:
	p = line.strip().split(")")
	uptree[p[1]] = p[0]

me = upward_path(uptree, "YOU")
santa = upward_path(uptree, "SAN")
shortest = 1_000_000
for k in set(me.keys()).intersection(santa.keys()):
	s = me[k] + santa[k]
	shortest = s if s < shortest else shortest
print("Q2: ", shortest - 2)
	
