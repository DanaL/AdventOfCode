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
    let pt = { row:parseInt(pieces[1]), col:parseInt(pieces[0]), tallies:0 };
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
    check each point's distance. Award that winner a tally. Point with the
    highest tally is the winner */
var max_d = manhattan_d(north_limit, west_limit, south_limit, east_limit);
for (let row = north_limit; row <= south_limit; row++) {
    for (let col = west_limit; col <= east_limit; col++) {
        let smallest = max_d;
        let winner = null;
        for (let pt of points) {
            let md = manhattan_d(pt.row, pt.col, row, col);

            /* If there is a tie, neither point gets the tally */
            if (md == smallest) {
                winner = null;
            }
            else if (md < smallest) {
                smallest = md;
                winner = pt;
            }
        }

        if (winner !== null)
            winner.tallies++;
    }
}

var winner = null;
for (let pt of points) {
    if (winner == null || pt.tallies > winner.tallies)
        winner = pt;
}

console.log(`${winner.row}, ${winner.col}: ${winner.tallies}`);
