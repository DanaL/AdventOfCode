
clay = set()
deepest_y = 0
with open("clay_scan_sm.txt", "r") as file:
    for line in file.readlines():
        l, r = line.strip().split(",")
        rng_s = int(r[r.find("=")+1:r.find(".")])
        rng_e = int(r[r.find("..")+2:])
        lv = int(l[2:])

        if l[0] == "x":
            for y in range(rng_s, rng_e+1):
                clay.update([(lv, y)])
                if y > deepest_y: deepest_y = y
        else:
            if lv > deepest_y: deepest_y = lv
            for x in range(rng_s, rng_e+1):
                clay.update([(x, lv)])
print(clay)
        
