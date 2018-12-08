parse = (nodes) => {
    let num_children = nodes.pop();
    let num_metadata = nodes.pop();
    let n = { children:[], metadata:[] };

    for (let j = 0; j < num_children; j++)
        n.children.push(parse(nodes));
    for (let k = 0; k < num_metadata; k++)
        n.metadata.push(nodes.pop());

    return n;
}

sum_metadata = (head) => {
    var sum = head.metadata.reduce((acc, curr) => acc + curr);
    for (let child of head.children)
        sum += sum_metadata(child);

    return sum;
}

sum_child_values = (head) => {
    if (head.children.length == 0)
        return head.metadata.reduce((acc, curr) => acc + curr);

    var sum = 0;
    for (let m of head.metadata) {
        if (m <= head.children.length)
            sum += sum_child_values(head.children[m - 1]);
    }

    return sum;
}

var fs = require("fs");
nodes = fs.readFileSync("license.txt").toString().split(" ").map(s => parseInt(s)).reverse();

head = parse(nodes);

console.log("Q1: " + sum_metadata(head));
console.log("Q2: " + sum_child_values(head));
