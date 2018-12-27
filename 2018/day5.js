function react(ch1, ch2) {
    return ch1 != ch2 && ch1.toLowerCase() == ch2.toLowerCase();
}

function do_reaction(polymer, skip) {
    const stack = [];
    for (let p of polymer) {
        if (p.toLowerCase() == skip)
            continue;

        const last = stack.length - 1;
        if (stack.length == 0 || !react(stack[last], p))
            stack.push(p);
        else
            stack.pop();
    }

    return stack.length;
}

const fs = require("fs");
const polymer = fs.readFileSync("polymer.txt", "utf8");

console.log("Answer to q1: " + do_reaction(polymer, ""));

var smallest = polymer.length + 1;
for (let ch of "abcdefghijklmnopqrstuvwxyz") {
    let len = do_reaction(polymer, ch);
    if (len < smallest)
        smallest = len;
}

console.log("Answer to q2: " + smallest);
