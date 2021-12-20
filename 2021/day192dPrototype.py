
# A prototype to solve day 19 with a smaller set of points in 2D instead
# of 3D. (To help (hopefully) cement my understanding)
scannerA = [(1,1), (6,9), (4,7), (6,3), (7,7), (8,5), (-2,0), (4,-5)]
scannerB = [(6,3), (3,4), (4,7), (5,5), (3,1), (-2,-4)]
scannerC = [(-4,4), (-5,6), (-7,5), (1,1), (1,-1), (-3,0)]

def rotate90(pt):
	return (pt[1] * -1, pt[0] * 1)

def rotate180(pt):
	return (pt[0] * -1, pt[1] * -1)

def rotate270(pt):
	return (pt[1] * 1, pt[0] * -1)

# generate all the rotations for each set of points
# in scanners B and C
rotations = []
rotations.append([rotate90(x) for x in scannerB])
rotations.append([rotate180(x) for x in scannerB])
rotations.append([rotate270(x) for x in scannerB])
rotations.append([rotate90(x) for x in scannerC])
rotations.append([rotate180(x) for x in scannerC])
rotations.append([rotate270(x) for x in scannerC])

def cmp_to_rotation(pta, rotation, fixedPts, orig):
	print(rotation)
	for j in range(len(rotation)):
		transpose = (pta[0] - rotation[j][0], pta[1] - rotation[j][1])
		matches = []
		for k in range(len(rotation)):
			t = (rotation[k][0] + transpose[0], rotation[k][1] + transpose[1])
			if t in fixedPts:
				matches.append((t, rotation[k], orig[k], k, transpose))
		if len(matches) > 2:
			for match in matches:
				r = rotate90(match[1])
				print("Match:")
				print("   t:", match[0])
				print("    :", r)

print(rotate90((10,5)))
print(rotate180((10,5)))
print(rotate270((10,5)))
#cmp_to_rotation((6,3), rotations[5], scannerB, scannerC)
#cmp_to_rotation((6,3), rotations[5], scannerB, scannerC)


