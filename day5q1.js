function react(ch1, ch2) {
    return ch1 != ch2 && ch1.toLowerCase() == ch2.toLowerCase();
}

const fs = require("fs");
const polymer = fs.readFileSync("polymer.txt", "utf8");

const stack = [];
for (j = 0; j < polymer.length; j++) {
    const last = stack.length - 1;
    if (stack.length == 0 || !react(stack[last], polymer[j]))
        stack.push(polymer[j]);
    else
        stack.pop();
}

console.log(stack.length);
