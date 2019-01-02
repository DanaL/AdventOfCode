const fs = require('fs').promises;

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').filter(l => l.length > 0);

    return lines;
}

/*                   +1 0
	  \ n  /	    \ -1 /
	nw +--+ ne +1 -1 +--+ 0 +1
	  /    \     0  /0 0 \ -1
	-+      +-    -+  0   += 
	  \    /   0 -1 \    /-1 +1
	sw +--+ se   +1  +--+   0
	  / s  \        /-1 0\
                      +1

	Using the cube co-ordinates from:
		https://www.redblobgames.com/grids/hexagons/
*/
let move = (loc, dir) => {
	switch (dir) {
		case "n":  
			loc.x +=1; loc.z -= 1;
			break;
		case "s": 
			loc.x -=1; loc.z += 1;
			break;
		case "nw": 
			loc.x +=1; loc.y -= 1;
			break;
		case "sw": 
			loc.y -=1; loc.z += 1;
			break;
		case "se": 
			loc.x -=1; loc.y += 1;
			break;
		case "ne": 
			loc.y +=1; loc.z -= 1;	
			break;
	}
	
	return loc;
}

let hex_distance_from_origin = (a) => {
	return (Math.abs(a.x) + Math.abs(a.y) + Math.abs(a.z)) / 2
}

let walk = (path) => {
	let loc = { x:0, y:0, z:0 };
	let furthest = 0;
	for (let dir of path) {
		loc = move(loc, dir);
		let d = hex_distance_from_origin(loc);
		if (d > furthest)
			furthest = d;
	}	

	console.log(loc);	
	console.log("Q1: " + hex_distance_from_origin(loc));
	console.log("Q2: " + furthest);
}

let main = async () => {
    const lines = await readInput("path.txt");
	let path = lines[0].split(",");

	walk(path);
}

main();
