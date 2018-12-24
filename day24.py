class Group:
    def __init__(self, type):
        self.weaknesses = set()
        self.immunities = set()
        self.target = None
        self.type = type

def find_weaknesses(block, g):
    if len(block) < 2: return
    block = block.replace(",", "").replace(";", "")
    weak = False
    for word in block.split(" "):
        if word == "to": continue
        if word == "weak":
            weak = True
            continue
        if word == "immune":
            weak = False
            continue
        if weak:
            g.weaknesses.add(word)
        else:
            g.immunities.add(word)

def calc_actual_attack(tori, uke):
    ep = tori.effective_power
    if tori.dmg_type in uke.immunities:
        ep = 0
    if tori.dmg_type in uke.weaknesses:
        ep *= 2
    return ep

def pick_target(tori, ukes, prev_targets):
    best = 0
    targets = []
    for u in ukes:
        if u in prev_targets: continue # target can only be chosen once per round
        aa = calc_actual_attack(tori, u)
        if aa == 0: continue # or aa < u.hp: continue
        if aa > best:
            best = aa
            targets = [u]
        elif aa == best:
            targets.append(u)
    if not targets:
        tori.target = None
    else:
        if len(targets) > 1:
            targets = sorted(targets, key=lambda t: (t.effective_power, t.init), reverse=True)
        tori.target = targets[0]
        prev_targets.add(tori.target)

def dump_group(g):
    s = f"{g.type} {g.id}: {g.hp} per {g.units}. {g.dmg} of {g.dmg_type}. init: {g.init}"
    print(s)
    print("  ", g.weaknesses)
    print("  ", g.immunities)

def fight(immunes, infections):
    x = 1
    while True:
        print("Round:", x)
        prev_targets = set()
        # Step 1, pick targets
        for tori in sorted(immunes, key=lambda t: (t.effective_power, t.init), reverse=True):
            pick_target(tori, infections, prev_targets)
        for tori in sorted(infections, key=lambda t: (t.effective_power, t.init), reverse=True):
            pick_target(tori, immunes, prev_targets)

        # Step 2, do the attacks
        fighters = sorted(immunes + infections, key=lambda t: t.init, reverse=True)
        for tori in fighters:
            if tori.target == None: continue
            if tori.units <= 0: continue
            aa = calc_actual_attack(tori, tori.target)
            killed = aa // tori.target.hp
            units_before = tori.target.units
            tori.target.units -= killed
            print(f"{tori.type} {tori.id} attacks {tori.target.type} {tori.target.id} for {aa}, {killed} of {units_before} killed.")
            tori.target.effective_power = tori.target.dmg * tori.target.units

        # after round, remove dead groups, check effective power
        immunes = [i for i in immunes if i.units > 0]
        infections = [i for i in infections if i.units > 0]
        for i in immunes+infections:
            i.effective_power = i.units * i.dmg
        x += 1
        if not immunes or not infections: break

    print(sum([i.units for i in immunes + infections if i.units > 0]))

immunes = []
infections = []
x = 0
with open("armies.txt", "r") as f:
    imm = False
    for line in f.readlines():
        line = line.strip()
        if line.strip() == "": continue
        if line == "Immune System:":
            imm = True
            continue
        if line == "Infection:":
            x = 0
            imm = False
            continue
        pieces = line.split(" ")
        g = Group("Immume" if imm else "Infection")
        g.id = x
        g.units = int(pieces[0])
        g.hp = int(pieces[4])
        g.init = int(pieces[-1])

        # find attack type/dmg
        j = line.find("attack that does ")
        k = line.find(" ", j + 17)
        g.dmg = int(line[j+17:k])
        l = line.find(" ", k+1)
        g.dmg_type = line[k+1:l]
        g.effective_power = g.units * g.dmg
        if line.find("(") > 0:
            find_weaknesses(line[line.find("(")+1:line.find(")")], g)
        if imm: immunes.append(g)
        else: infections.append(g)
        x += 1

fight(immunes, infections)
