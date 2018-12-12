/* Advent of Code 2018 Day 10 (https://adventofcode.com/2018/day/10)

This puzzle had us reading a message in the sky. Lights were scattered about
a grid and we were given their starting position and velocity (speed and
direction, which remain constant thank goodness). All we had to do was iterate
until the message was visible and read the message. The trick was figuring out
when you should stop iterating.

I first thought about trying to detect when a bunch of lights were all lined up
but a better way is to realize the points converge on each other and then move
apart again. So, when reading in the co-ordinates, I save the furthest each
north, south, east, and west co-ord. Those give me the bounds of a rectangle
that contains all the points. After each iteration I calculate the frame again.
As soon as it starts getting bigger, I know I'm done and I back up one
iteration and print the result.

*/

const bounded_area = (pts) => {
    var top = Number.MAX_SAFE_INTEGER;
    var bottom = Number.MIN_SAFE_INTEGER;
    var left = Number.MAX_SAFE_INTEGER;
    var right = Number.MIN_SAFE_INTEGER;

    for (let pt of pts) {
	if (pt.px < left) left = pt.px;
	if (pt.px > right) right = pt.px;
	if (pt.py < top) top = pt.py;
	if (pt.py > bottom) bottom = pt.py;
    }

    return { top:top, bottom:bottom, left:left, right:right};
};

const dump_area = (pts) => {
    var ba = bounded_area(pts);
    var grid = [];

    for (let r = ba.top - 1; r <= ba.bottom + 1; r++)
	for (let c = ba.left - 1; c <= ba.right + 1; c++)
	    grid[`${r},${c}`] = '.';

    for (let p of pts) {
	let key = `${p.py},${p.px}`;
	if (key in grid)
	    grid[key] = '#';
    }

    for (let r = ba.top - 1; r <= ba.bottom + 1; r++) {
	var line = '';
	for (let c = ba.left - 1; c <= ba.right + 1; c++) {
	    let key = `${r},${c}`;
	    line += grid[key];
	}
	console.log(line);
    }
};

const iterate = (pts) => {
    for (let pt of pts) {
	pt.px += pt.vx;
	pt.py += pt.vy;
    }
}

const reverse = (pts) => {
    for (let pt of pts) {
	pt.px -= pt.vx;
	pt.py -= pt.vy;
    }
}

var fs = require("fs");
var pts = fs.readFileSync("points.txt").toString().trim().split("\n").map(line => {
    var lb = line.indexOf("<") + 1;
    var rb = line.indexOf(">", lb);
    const coords =line.slice(lb, rb).split(",").map(p => parseInt(p));
    lb = line.indexOf("<", rb) + 1;
    rb = line.indexOf(">", lb);
    const v =line.slice(lb, rb).split(",").map(p => parseInt(p));

    return { px:coords[0], py:coords[1], vx:v[0], vy:v[1]};
});

var prev_area = Number.MAX_SAFE_INTEGER;
var seconds = 1;
while (true) {
    ++seconds;
    iterate(pts);
    var ba = bounded_area(pts);
    let area = (ba.bottom - ba.top) * (ba.right - ba.left);
    if (area >= prev_area) {
	reverse(pts);
	break;
    }
    prev_area = area;
}

console.log("Seconds needed: " + (seconds - 2));
dump_area(pts);
