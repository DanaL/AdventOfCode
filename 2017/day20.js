const fs = require('fs').promises;

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').map(l => l.trim()).filter(l => l.length > 0);

    return lines;
}

let parse = (lines) => {
	const parsed = [];
	for (let line of lines) {
		const m = line.match(/^p=<(-?\d+),(-?\d+),(-?\d+)>, v=<(-?\d+),(-?\d+),(-?\d+)>, a=<(-?\d+),(-?\d+),(-?\d+)>$/);
		parsed.push({ p:{x:parseInt(m[1]), y:parseInt(m[2]), z:parseInt(m[3])},
						v:{x:parseInt(m[4]), y:parseInt(m[5]), z:parseInt(m[6])}, 
						a:{x:parseInt(m[7]), y:parseInt(m[8]), z:parseInt(m[9])} });
	}

	return parsed;
}

let update_velocity = (particle) => {
	particle.v.x += particle.a.x; 
	particle.v.y += particle.a.y; 
	particle.v.z += particle.a.z; 

	return particle;
}

let update_pos = (particle) => {
	particle.p.x += particle.v.x; 
	particle.p.y += particle.v.y; 
	particle.p.z += particle.v.z; 

	return particle;
}

let dis_from_origin = (pt) => Math.abs(pt.p.x) + Math.abs(pt.p.y) + Math.abs(pt.p.z);

let q1 = (particles) => {
	for (let j = 0; j < 1000; j++) {
		for (let p = 0; p < particles.length; p++)
			particles[p] = update_velocity(particles[p]);
		for (let p = 0; p < particles.length; p++)
			particles[p] = update_pos(particles[p]);
	}
	
	let closest = dis_from_origin(particles[0]);
	let closest_index;
	for (let j = 0; j < particles.length; j++) {
		let d = dis_from_origin(particles[j]);
		if (d < closest) {
			closest = d;
			closest_index = j;
		}
	}

	console.log("Q1: " + closest_index + " " + closest);
}

let main = async () => {
    const lines = await readInput("particles.txt");
	const parsed = parse(lines);

	q1(parsed);
}

main();
