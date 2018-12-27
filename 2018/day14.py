
elf1, elf2 = 0, 1
rs = [3, 7]
recipes = 793031
seq = [int(ch) for ch in str(recipes)]
q1 = q2 = False

while not(q1 and q2):
    score = rs[elf1] + rs[elf2]
    rs.extend(divmod(score, 10) if score > 9 else [score])

    elf1 = (elf1 + rs[elf1] + 1) % len(rs)
    elf2 = (elf2 + rs[elf2] + 1) % len(rs)

    if not q1 and len(rs) > 10 + recipes:
        print("Q1: ", "".join([str(r) for r in rs[recipes:recipes+10]]))
        q1 = True
    if not q2 and rs[-len(seq):] == seq:
        print("Q2: ", len(rs) - len(seq))
        q2 = True
    # We can add either 1 or 2 recipes to the scoreboard, depending on the
    # results of the last experiment
    if not q2 and rs[-len(seq)-1:-1] == seq:
        print("Q2: ", len(rs) - len(seq) - 1)
        q2 = True
