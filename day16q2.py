class Example:
    def __init__(self):
        self.before = None
        self.after = None
        self.instruction = None

class Machine:
    def __init__(self):
        self.regs = [0, 0, 0, 0]
        self.ops = { 0 : self.ex_gtri, 1 : self.ex_bani, 2 : self.ex_eqrr, 3 : self.ex_gtir,
                     4 : self.ex_eqir, 5 : self.ex_bori, 6 : self.ex_seti, 7 : self.ex_setr,
                     8 : self.ex_addr, 9 : self.ex_borr, 10 : self.ex_muli, 11 : self.ex_banr,
                     12 : self.ex_addi, 13 : self.ex_eqri, 14 : self.ex_mulr,
                     15 : self.ex_gtrr}
        
    def ex_addr(self, a, b, r):
        self.regs[r] = self.regs[a] + self.regs[b]
        
    def ex_addi(self, a, b, r):
        self.regs[r] = self.regs[a] + b
        
    def ex_mulr(self, a, b, r):
        self.regs[r] = self.regs[a] * self.regs[b]
        
    def ex_muli(self, a, b, r):
        self.regs[r] = self.regs[a] * b

    def ex_banr(self, a, b, r):
        self.regs[r] = self.regs[a] & self.regs[b]

    def ex_bani(self, a, b, r):
        self.regs[r] = self.regs[a] & b

    def ex_borr(self, a, b, r):
        self.regs[r] = self.regs[a] | self.regs[b]

    def ex_bori(self, a, b, r):
        self.regs[r] = self.regs[a] | b

    def ex_setr(self, a, b, r):
        self.regs[r] = self.regs[a]

    def ex_seti(self, a, b, r):
        self.regs[r] = a
        
    def ex_gtir(self, a, b, r):
        self.regs[r] = 1 if a > self.regs[b] else 0

    def ex_gtri(self, a, b, r):
        self.regs[r] = 1 if self.regs[a] > b else 0

    def ex_gtrr(self, a, b, r):
        self.regs[r] = 1 if self.regs[a] > self.regs[b] else 0

    def ex_eqir(self, a, b, r):
        self.regs[r] = 1 if a == self.regs[b] else 0

    def ex_eqri(self, a, b, r):
        self.regs[r] = 1 if self.regs[a] == b else 0

    def ex_eqrr(self, a, b, r):
        self.regs[r] = 1 if self.regs[a] == self.regs[b] else 0

    def ex_instr(self, instr):
        op, a, b, r = instr
        if op not in self.ops:
            raise Exception(f"Opcode {op} not defined!")

        self.ops[op](a, b, r)

m = Machine()
with open("program.txt") as file:
    for line in file.readlines():
        instr = [int(c) for c in line.split(" ")]
        m.ex_instr(instr)

print(m.regs)
