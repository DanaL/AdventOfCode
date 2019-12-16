from collections import defaultdict
import math
import re

# I'll clean this shit up when I rewrite this in Rust D:
class Recipe:
	def __init__(self, c, p, i):
		self.chem = c
		self.product = int(p)
		self.ingredients = i

	def __repr__(self):
		s = "Chem: " + self.chem + " (" + str(self.product) + ")"
		s += "\n   Ingredients: " + " ".join([i[1] + " (" + i[0] + ")" for i in self.ingredients])
		s += "\n"
		return s

#with open("test14.txt") as file:
#	lines = file.readlines()
with open("./inputs/day14.txt") as file:
	lines = file.readlines()

fuel = None
recipes = {}
for line in lines:
	r = re.findall("(\d+) ([A-Z]+)", line)
	recipe = Recipe(r[-1][1],r[-1][0], r[:-1])
	recipes[recipe.chem] = recipe

def only_ore(recipe):
	for i in recipe.ingredients:
		if i[1] != "ORE": return False
	return True

def produce(chem, amt, leftovers, recipes):
	if chem in leftovers and leftovers[chem] > 0:
		if leftovers[chem] >= amt:
			# We can use the leftovers, don't need more ore
			leftovers[chem] -= amt
			return 0
		else:
			amt -= leftovers[chem]
			leftovers[chem] = 0
	
	recipe = recipes[chem]
	num_of_runs = math.ceil(amt / recipe.product)
	produced = num_of_runs * recipe.product
	if produced > amt:
		leftovers[recipe.chem] += produced - amt

	if only_ore(recipe):
		return num_of_runs * int(recipe.ingredients[0][0])
	else:
		ore = 0
		for ing in recipe.ingredients:
			ing_amt = num_of_runs * int(ing[0])	
			ore += produce(ing[1], ing_amt, leftovers, recipes)
		return ore
	
leftovers = defaultdict(int)
print("Q1:", produce("FUEL", 1, leftovers, recipes))

# Now for Q2...
total_ore = 1000000000000
leftovers = defaultdict(int)
fuel_produced = 0

hi = 5_785_000
lo = 5_100_000
mid = 0
while lo <= hi:
	leftovers = defaultdict(int)	
	mid = (hi + lo) // 2
	ore = produce("FUEL", mid, leftovers, recipes)
	if total_ore - ore < 0: #too high
		hi = mid - 1
	else:
		lo = mid + 1

leftovers = defaultdict(int)
print("Q2:", mid)

