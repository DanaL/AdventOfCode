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

sum_tree = (head) => {
    var sum = head.metadata.reduce((acc, curr) => acc + curr);
    for (let child of head.children)
        sum += sum_tree(child);

    return sum;
}

var fs = require("fs");
nodes = fs.readFileSync("license.txt").toString().split(" ").map(s => parseInt(s)).reverse();

head = parse(nodes);

console.log(sum_tree(head));
