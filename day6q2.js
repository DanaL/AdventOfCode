function manhattan_d(pr, pc, qr, qc) {
    return Math.abs(pr - qr) + Math.abs(pc - qc);
}

var points = [];
var north_limit = { row:NaN };
var south_limit = { row:NaN };
var west_limit = { col:NaN };
var east_limit = { col:NaN };
var fs = require("fs");
fs.readFileSync("mpoints.txt").toString().split("\n").forEach(s => {
    let pieces = s.split(',');
    let pt = { row:parseInt(pieces[1]), col:parseInt(pieces[0]) };
    points.push(pt);

    /* Figure out what our limits are */
    if (isNaN(north_limit) || pt.row < north_limit)
        north_limit = pt.row;
    if (isNaN(east_limit) || pt.col > east_limit)
        east_limit = pt.col;
    if (isNaN(south_limit) || pt.row > south_limit)
        south_limit = pt.row;
    if (isNaN(west_limit) || pt.col < west_limit)
        west_limit = pt.col;
});

/* Loop over each point in a search grid (defined by the above limits) and
    count which points are below the threshold */
const threshold = 10000;
var count = 0;
for (let row = north_limit; row <= south_limit; row++) {
    for (let col = west_limit; col <= east_limit; col++) {
        const sum = points.reduce((total, pt) => total + manhattan_d(pt.row, pt.col, row, col), 0);
        if (sum < threshold)
            ++count;
    }
}

console.log(count);
