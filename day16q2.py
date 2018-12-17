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
ignore = (0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 11, 13, 14, 15)
for ex in examples:
    if ex.instruction[0] in ignore:
        continue
    res = test_example(ex)
    if len(res) == 1:
        print(res)





