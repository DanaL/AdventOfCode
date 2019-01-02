const fs = require('fs').promises;
require("../DisjointSet");

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').filter(l => l.length > 0);

    return lines;
}

let parse_line = (line) => {
    const m = line.split(" <-> ");
    return { prog_id:parseInt(m[0]), connected:m[1].split(", ").map(Number) };
}

let group = (progs) => {
    /* Use Union/Find to divide the programs into sets that are connected by
        their communication pipes */
    let nodes = {};
    for (p of progs) {
        if (!(p.prog_id in nodes))
            nodes[p.prog_id] = DSNode(p.prog_id);
        for (n of p.connected) {
            if (!(n in nodes))
                nodes[n] = DSNode(n);
            nodes[p.prog_id].union(nodes[n]);
        }
    }

    /*  Q1 wants to know (effectively) how many items are in the same set as program ID 0
        Q2 is simply count how many distinct sets of programs there are. */
    let count = 0;
    let p0 = nodes[0].find();
    let sets = new Set();
    for (n in nodes) {
        let parent = nodes[n].find();
        if (p0 === parent)
            ++count;
        if (!sets.has(parent.value))
            sets.add(parent.value);
    }

    console.log("Q1: " + count);
    console.log("Q2: " + sets.size);
}

let main = async () => {
    const lines = await readInput("day12input.txt");
	const progs = lines.map(parse_line);

    group(progs);
}

main();
