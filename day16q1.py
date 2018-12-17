class Example:
    def __init__(self):
        self.before = None
        self.after = None
        self.instruction = None

def ex_addr(ex):
    output_reg = ex.instruction[3]
    reg_a = ex.instruction[1]
    reg_b = ex.instruction[2]
    after = list(ex.before)
    after[output_reg] = ex.before[reg_a] + ex.before[reg_b]
    return tuple(after)

def ex_addi(ex):
    output_reg = ex.instruction[3]
    reg_a = ex.instruction[1]
    val_b = ex.instruction[2]
    after = list(ex.before)
    after[output_reg] = ex.before[reg_a] + val_b
    return tuple(after)

def ex_mulr(ex):
    output_reg = ex.instruction[3]
    reg_a = ex.instruction[1]
    reg_b = ex.instruction[2]
    after = list(ex.before)
    after[output_reg] = ex.before[reg_a] * ex.before[reg_b]
    return tuple(after)

def ex_muli(ex):
    output_reg = ex.instruction[3]
    reg_a = ex.instruction[1]
    val_b = ex.instruction[2]
    after = list(ex.before)
    after[output_reg] = ex.before[reg_a] * val_b
    return tuple(after)

def ex_banr(ex):
    output_reg = ex.instruction[3]
    reg_a = ex.instruction[1]
    reg_b = ex.instruction[2]
    after = list(ex.before)
    after[output_reg] = ex.before[reg_a] & ex.before[reg_b]
    return tuple(after)

def ex_bani(ex):
    output_reg = ex.instruction[3]
    reg_a = ex.instruction[1]
    val_b = ex.instruction[2]
    after = list(ex.before)
    after[output_reg] = ex.before[reg_a] & val_b
    return tuple(after)

def ex_borr(ex):
    output_reg = ex.instruction[3]
    reg_a = ex.instruction[1]
    reg_b = ex.instruction[2]
    after = list(ex.before)
    after[output_reg] = ex.before[reg_a] | ex.before[reg_b]
    return tuple(after)

def ex_bori(ex):
    output_reg = ex.instruction[3]
    reg_a = ex.instruction[1]
    val_b = ex.instruction[2]
    after = list(ex.before)
    after[output_reg] = ex.before[reg_a] | val_b
    return tuple(after)

def ex_setr(ex):
    output_reg = ex.instruction[3]
    reg_a = ex.instruction[1]
    after = list(ex.before)
    after[output_reg] = ex.before[reg_a]
    return tuple(after)

def ex_seti(ex):
    output_reg = ex.instruction[3]
    val_a = ex.instruction[1]
    after = list(ex.before)
    after[output_reg] = val_a
    return tuple(after)

def ex_gtir(ex):
    output_reg = ex.instruction[3]
    val_a = ex.instruction[1]
    reg_b = ex.instruction[2]
    after = list(ex.before)
    after[output_reg] = 1 if val_a > ex.before[reg_b] else 0
    return tuple(after)

def ex_gtri(ex):
    output_reg = ex.instruction[3]
    reg_a = ex.instruction[1]
    val_b = ex.instruction[2]
    after = list(ex.before)
    after[output_reg] = 1 if ex.before[reg_a] > val_b else 0
    return tuple(after)

def ex_gtrr(ex):
    output_reg = ex.instruction[3]
    reg_a = ex.instruction[1]
    reg_b = ex.instruction[2]
    after = list(ex.before)
    after[output_reg] = 1 if ex.before[reg_a] > ex.before[reg_b] else 0
    return tuple(after)

def ex_eqir(ex):
    output_reg = ex.instruction[3]
    val_a = ex.instruction[1]
    reg_b = ex.instruction[2]
    after = list(ex.before)
    after[output_reg] = 1 if val_a == ex.before[reg_b] else 0
    return tuple(after)

def ex_eqri(ex):
    output_reg = ex.instruction[3]
    reg_a = ex.instruction[1]
    val_b = ex.instruction[2]
    after = list(ex.before)
    after[output_reg] = 1 if ex.before[reg_a] == val_b else 0
    return tuple(after)

def ex_eqrr(ex):
    output_reg = ex.instruction[3]
    reg_a = ex.instruction[1]
    reg_b = ex.instruction[2]
    after = list(ex.before)
    after[output_reg] = 1 if ex.before[reg_a] == ex.before[reg_b] else 0
    return tuple(after)

def test_example(ex):
    matches = 0
    
    if ex_addr(ex) == ex.after: matches +=1
    if ex_addi(ex) == ex.after: matches += 1
    if ex_mulr(ex) == ex.after: matches += 1
    if ex_muli(ex) == ex.after: matches += 1
    if ex_banr(ex) == ex.after: matches += 1
    if ex_bani(ex) == ex.after: matches += 1
    if ex_borr(ex) == ex.after: matches += 1
    if ex_bori(ex) == ex.after: matches += 1
    if ex_setr(ex) == ex.after: matches += 1
    if ex_seti(ex) == ex.after: matches += 1
    if ex_gtri(ex) == ex.after: matches += 1
    if ex_gtir(ex) == ex.after: matches += 1
    if ex_gtrr(ex) == ex.after: matches += 1
    if ex_eqri(ex) == ex.after: matches += 1
    if ex_eqir(ex) == ex.after: matches += 1
    if ex_eqrr(ex) == ex.after: matches += 1

    return matches

examples = []
with open("opcodes.txt") as file:
    lines = file.readlines()

    for j in range(len(lines)):
        if lines[j][0:3] == "Bef":
            ob = lines[j].find("[")
            eb = lines[j].find("]")
            ex = Example()
            ex.before = tuple([int(i) for i in lines[j][ob+1:eb].split(",")]);
            ex.instruction = tuple([int(i) for i in lines[j+1].split(" ")]);
            ex.after = tuple([int(i) for i in lines[j+2][ob+1:eb].split(",")]);
            examples.append(ex)

count = 0
for ex in examples:
    if test_example(ex) >= 3:
        count += 1
print(count)



