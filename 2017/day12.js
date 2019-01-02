const fs = require('fs').promises;

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').filter(l => l.length > 0);

    return lines;
}

let parse_line = (line) => {
    const m = line.split(" <-> ");
    return { prog_id:parseInt(m[0]), connected:m[1].split(", ").map(Number) };
}

let main = async () => {
    const lines = await readInput("day12input.txt");
	const progs = lines.map(parse_line);

    console.log(progs);
}

main();
