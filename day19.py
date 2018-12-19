class Example:
    def __init__(self):
        self.before = None
        self.after = None
        self.instruction = None

class Machine:
    def __init__(self):
        self.regs = [0, 0, 0, 0, 0, 0]
        self.instr_ptr = 0
        self.instr_val = 0
        self.ops = { "gtri" : self.ex_gtri, "bani" : self.ex_bani, "eqrr" : self.ex_eqrr, "gtir" : self.ex_gtir,
                     "eqir" : self.ex_eqir, "bori" : self.ex_bori, "seti" : self.ex_seti, "setr" : self.ex_setr,
                     "addr" : self.ex_addr, "borr" : self.ex_borr, "muli" : self.ex_muli, "banr" : self.ex_banr,
                     "addi" : self.ex_addi, "eqri" : self.ex_eqri, "mulr" : self.ex_mulr,
                     "gtrr" : self.ex_gtrr}
        
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

        print(instr, m.instr_val, m.regs,)
        m.regs[m.instr_ptr] = m.instr_val
        self.ops[op](a, b, r)
        m.instr_val = m.regs[m.instr_ptr]
        m.instr_val += 1
        print("                   ",m.regs)
        
    def execute_program(self, program):
        while m.instr_val < len(program):
            self.ex_instr(program[m.instr_val])
                    
m = Machine()
program = []
with open("program2.txt") as file:
    for line in file.readlines():
        if line[0:3] == "#ip":
            m.instr_ptr = int(line[line.find(" "):])
            m.instr_val = 0
        else:
            pieces = line.strip().split(" ")
            instr = [pieces[0]]
            instr.extend([int(c) for c in pieces[1:]])
            program.append(instr)


m.execute_program(program)

print(m.regs)
