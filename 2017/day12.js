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

let q1 = (progs) => {
    let nodes = {};

    /* Use Union/Find to divide the programs into sets that are connected by
        their communication pipes */
    for (p of progs) {
        if (!(p.prog_id in nodes))
            nodes[p.prog_id] = DSNode(p.prog_id);
        for (n of p.connected) {
            if (!(n in nodes))
                nodes[n] = DSNode(n);
            nodes[p.prog_id].union(nodes[n]);
        }
    }

    let count = 0;
    let p0 = nodes[0].find();
    for (n in nodes) {
        if (p0 === nodes[n].find())
            ++count;
    }

    console.log("Q1: " + count);
}

let main = async () => {
    const lines = await readInput("day12input.txt");
	const progs = lines.map(parse_line);

    q1(progs);
}

main();
