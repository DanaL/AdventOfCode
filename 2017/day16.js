const fs = require('fs').promises;

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').filter(l => l.length > 0);

    return lines;
}

let rotate = (dancers, n) => {
	tmp = [];
	for (let j = 0; j < n; j++)
		tmp.push(dancers.pop());
	tmp.reverse();
	return tmp.concat(dancers);
}

let dance = (dancers, steps) => {
	for (step of steps) {
		switch (step[0]) {
			case "s":
				var m = step.match(/s(\d+)/).map(Number);
				dancers = rotate(dancers, m[1]);
				break;
			case "x":
				var m = step.match(/x(\d+)\/(\d+)/).map(Number);
				var tmp = dancers[m[1]];
				dancers[m[1]] = dancers[m[2]];
				dancers[m[2]] = tmp;
				break;
			case "p":
				var l1 = step[1];
				var l2 = step[3];
				for (var a=0; dancers[a] != l1; a++);
				for (var b=0; dancers[b] != l2; b++);
				var tmp = dancers[a];
				dancers[a] = dancers[b];
				dancers[b] = tmp;
				break;
		}
	}

	return dancers;
}

/* Look for where the dances cycle and then figure out
	which one will be the billionth ieration */
let dance_dance_dance = (dancers, steps) => {
	let seen = new Set();
	seen.add(dancers.join(""));
	let dances = ["abcdefghijklmnop"];
	while (true) {
		dancers = dance(dancers, steps);
		if (seen.has(dancers.join("")))
			break;
		seen.add(dancers.join(""));
		dances.push(dancers.join(""));
	}

	console.log("Q2: " + dances[1000000000 % 60]);
}

let main = async () => {
    const lines = await readInput("dance_steps.txt");
	//let steps = "s1,x3/4,pe/b".split(",");
	let dancers = Array.from("abcdefghijklmnop");
	const steps = lines[0].split(",");

	dancers = dance(dancers, steps);
	console.log("Q1: " + dancers.join(""));
	dance_dance_dance(Array.from("abcdefghijklmnop"), steps);
}

main();
