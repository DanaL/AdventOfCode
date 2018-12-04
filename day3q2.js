function parse_swatch(swatch) {
    const amp = swatch.indexOf('@');
    const colon = swatch.indexOf(':');
    var cds = [];
    swatch.slice(amp + 2, colon).split(',').forEach(c => cds.push(parseInt(c)));
    var dims = [];
    const size = swatch.substr(colon + 1).split('x').forEach(d => dims.push(parseInt(d)));

    return { name: swatch.slice(0, amp - 1), coords:cds, dimensions:dims};
}

var fs = require("fs");
const swatches = fs.readFileSync("swatches.txt").toString().split("\n");

var sets = [];
var visited = [];
swatches.forEach(s => {
    var swatch = parse_swatch(s);

    /* loop over each co-ord in the swatch */
    const row = swatch.coords[1] + 1;
    const col = swatch.coords[0] + 1;
    const w = swatch.dimensions[0];
    const h = swatch.dimensions[1];
    for (let j = row; j < row + h; j++) {
        for (let k = col; k < col + w; k++) {
            var coords = `${j}x${k}`;
            if (!(swatch.name in sets))
                sets[swatch.name] = false;

            if (coords in visited) {
                sets[visited[coords]] = true;
                sets[swatch.name] = true;;
            }
            else {
                visited[coords] = swatch.name;
            }
        }
    }
});

for (set in sets) {
    if (!sets[set]) {
        console.log(set);
        break;
    }
}
