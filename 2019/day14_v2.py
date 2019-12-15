from collections import defaultdict
import math
import re

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
	print("Producing:", chem, amt)
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
#print("CXFTF", produce("CXFTF", 68, leftovers, recipes))
#print(leftovers);
#print("VJHF", produce("VJHF", 46, leftovers, recipes))
#print("MNCFX", produce("MNCFX", 6, leftovers, recipes))
#print("GNMV", produce("GNMV", 25, leftovers, recipes))
#print("STKFG",produce("STKFG", 53, leftovers, recipes))
#print("HVMC", produce("HVMC", 81, leftovers, recipes))
#print("")
#leftovers = defaultdict(int)
print(produce("FUEL", 1, leftovers, recipes))

