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
        
    def ex_addr(self, instr):
        output_reg = instr[3]
        reg_a = instr[1]
        reg_b = instr[2]
        self.regs[output_reg] = self.regs[reg_a] + self.regs[reg_b]
        
    def ex_addi(self, instr):
        output_reg = instr[3]
        reg_a = instr[1]
        val_b = instr[2]
        self.regs[output_reg] = self.regs[reg_a] + val_b
        
    def ex_mulr(self, instr):
        output_reg = instr[3]
        reg_a = instr[1]
        reg_b = instr[2]
        self.regs[output_reg] = self.regs[reg_a] * self.regs[reg_b]
        
    def ex_muli(self, instr):
        output_reg = instr[3]
        reg_a = instr[1]
        val_b = instr[2]
        self.regs[output_reg] = self.regs[reg_a] * val_b

    def ex_banr(self, instr):
        output_reg = instr[3]
        reg_a = instr[1]
        reg_b = instr[2]
        self.regs[output_reg] = self.regs[reg_a] & self.regs[reg_b]

    def ex_bani(self, instr):
        output_reg = instr[3]
        reg_a = instr[1]
        val_b = instr[2]
        self.regs[output_reg] = self.regs[reg_a] & val_b

    def ex_borr(self, instr):
        output_reg = instr[3]
        reg_a = instr[1]
        reg_b = instr[2]
        self.regs[output_reg] = self.regs[reg_a] | self.regs[reg_b]

    def ex_bori(self, instr):
        output_reg = instr[3]
        reg_a = instr[1]
        val_b = instr[2]
        self.regs[output_reg] = self.regs[reg_a] | val_b

    def ex_setr(self, instr):
        output_reg = instr[3]
        reg_a = instr[1]
        self.regs[output_reg] = self.regs[reg_a]

    def ex_seti(self, instr):
        output_reg = instr[3]
        val_a = instr[1]
        self.regs[output_reg] = val_a
        
    def ex_gtir(self, instr):
        output_reg = instr[3]
        val_a = instr[1]
        reg_b = instr[2]
        self.regs[output_reg] = 1 if val_a > self.regs[reg_b] else 0

    def ex_gtri(self, instr):
        output_reg = instr[3]
        reg_a = instr[1]
        val_b = instr[2]
        self.regs[output_reg] = 1 if self.regs[reg_a] > val_b else 0

    def ex_gtrr(self, instr):
        output_reg = instr[3]
        reg_a = instr[1]
        reg_b = instr[2]
        self.regs[output_reg] = 1 if self.regs[reg_a] > self.regs[reg_b] else 0

    def ex_eqir(self, instr):
        output_reg = instr[3]
        val_a = instr[1]
        reg_b = instr[2]
        self.regs[output_reg] = 1 if val_a == self.regs[reg_b] else 0

    def ex_eqri(self, instr):
        output_reg = instr[3]
        reg_a = instr[1]
        val_b = instr[2]
        self.regs[output_reg] = 1 if self.regs[reg_a] == val_b else 0

    def ex_eqrr(self, instr):
        output_reg = instr[3]
        reg_a = instr[1]
        reg_b = instr[2]
        self.regs[output_reg] = 1 if self.regs[reg_a] == self.regs[reg_b] else 0

    def ex_instr(self, instr):
        if instr[0] not in self.ops:
            raise Exception(f"Opcode {instr[0]} not defined!")

        self.ops[instr[0]](instr)

m = Machine()
with open("program.txt") as file:
    for line in file.readlines():
        instr = [int(c) for c in line.split(" ")]
        m.ex_instr(instr)

print(m.regs)
