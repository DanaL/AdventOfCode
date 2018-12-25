class DSNode:
    def __init__(self, value):
        self.parent = self
        self.rank = 0
        self.value = value
 
def find(node):
    if node.parent == node:
        return node
    else:
        _n = node.parent
        while _n.parent != _n:
            _n = _n.parent
        node.parent = _n
        return _n
        
def union(n1, n2):
    _root1 = find(n1)
    _root2 = find(n2)
    if _root1.rank > _root2.rank:
        _root2.parent = _root1
    elif _root1.rank < _root2.rank:
        _root1.parent = _root2
    else:
        _root2.parent = _root1
        _root1.rank += 1

def manhattan(pt1, pt2):
    return abs(pt1[0] - pt2[0]) + abs(pt1[1] - pt2[1]) + abs(pt1[2] - pt2[2]) + abs(pt1[3] - pt2[3])

pts = set()
with open("4d_pts.txt", "r") as f:
    for line in f.readlines():
        p = tuple([int(n) for n in line.strip().split(",")])
        node = DSNode(p)
        for pt in pts:
            if node.value != pt.value and manhattan(node.value, pt.value) <= 3:
                union(node, pt)
        pts.add(node)

constellations = set()
for pt in pts:
    v = find(pt).value
    if not v in constellations:
        constellations.add(v)
print(len(constellations))



