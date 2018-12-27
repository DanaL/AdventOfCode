function parse_swatch(swatch) {
    const colon = swatch.indexOf(':');
    var cds = [];
    swatch.slice(swatch.indexOf('@') + 2, colon).split(',').forEach(c => cds.push(parseInt(c)));
    var dims = [];
    const size = swatch.substr(colon + 1).split('x').forEach(d => dims.push(parseInt(d)));

    return { coords:cds, dimensions:dims};
}

function mark_overlays(swatch, overlays) {
    const row = swatch.coords[1] + 1;
    const col = swatch.coords[0] + 1;
    const w = swatch.dimensions[0];
    const h = swatch.dimensions[1];
    for (let j = row; j < row + h; j++) {
        for (let k = col; k < col + w; k++) {
            sq = `${j}x${k}`;
            if (sq in overlays)
                overlays[sq]++;
            else
                overlays[sq] = 1;
        }
    }
}

var fs = require("fs");
const swatches = fs.readFileSync("swatches.txt").toString().split("\n");

var overlays = [];
swatches.forEach(s => mark_overlays(parse_swatch(s), overlays));

var sum = 0;
for (coord in overlays) {
    if (overlays[coord] > 1)
        sum++;
}
console.log(sum);
