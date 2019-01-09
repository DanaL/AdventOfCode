const fs = require('fs').promises;
const util = require("./common.js");

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
						a:{x:parseInt(m[7]), y:parseInt(m[8]), z:parseInt(m[9])},
						distance:0, prev_distance:0, collided:false });
	}

	return parsed;
}

let update_velocity = (particle) => {
	particle.v.x += particle.a.x; 
	particle.v.y += particle.a.y; 
	particle.v.z += particle.a.z; 
}

let update_pos = (particle) => {
	particle.p.x += particle.v.x; 
	particle.p.y += particle.v.y; 
	particle.p.z += particle.v.z; 
}

let update_distance = (pt) => {
	const d = dis_from_origin(pt);
	pt.prev_distance = pt.distance;
	pt.distance = d;
}

let dis_from_origin = (pt) => Math.abs(pt.p.x) + Math.abs(pt.p.y) + Math.abs(pt.p.z);

/* Don't actually need to simulate Q1. Over the long haul, the point closest to the origin will be 
	the one with the slowest acceleration */
let q1 = (particles) => {
	let lowest_acceleration = 999999;
	for (let p = 0; p < particles.length; p++) {
		let acc = Math.abs(particles[p].a.x) + Math.abs(particles[p].a.y) + Math.abs(particles[p].a.z);
		if (acc < lowest_acceleration) {
			lowest_acceleration = acc;
			var slowest = p;
		}
	}

	console.log("Q1: " + slowest);
}

let detect_collisions = (particles) => {
	const positions = { };
	for (let pt of particles) {
		const coord = `${pt.p.x},${pt.p.y},${pt.p.z}`;
		if (!(coord in positions))
			positions[coord] = [];
		positions[coord].push(pt);
	}

	for (let pos in positions) {
		if (positions[pos].length > 1)
			positions[pos].map(pt => pt.collided = true);
	}
}

let q2 = (particles) => {
	particles.map(update_distance);
	particles.map(pt => pt.prev_distance = pt.distance);

	/* Loop over the particles, updating their positions and looking for collisions.
		Proposed stopping condition: when all points are moving away from the origin. (Ie., all points 
		have distances that are greater than their previous distance. Not sure if that's a valid way
		to test that there will be no more collisions. */
	do {
		let alive = particles.filter(p => !p.collided);
		for (let pt of alive) {
			update_velocity(pt);
			update_pos(pt);
			update_distance(pt);
		}

		detect_collisions(alive);
	} while (particles.filter(p => !p.collided && p.distance < p.prev_distance).length > 0);

	console.log("Q2: " + particles.filter(p => !p.collided).length);
}

let main = async () => {
    const lines = await readInput("particles.txt");
	const parsed = parse(lines);

	q1(util.copy_obj(parsed));
	q2(util.copy_obj(parsed));
}

main();
