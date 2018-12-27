
function get_next(steps) {
    var avail = Object.keys(steps).filter(s => steps[s].length == 0);
    return avail.sort()[0];
}

const steps = [];
var fs = require("fs");
fs.readFileSync("instructions.txt").toString().split("\n").forEach(s => {
    let step = s.substr(s.indexOf("step") + 5, 1);
    let depends = s.substr(5, 1);
    if (!(step in steps))
        steps[step] = [];
    if (!(depends in steps))
        steps[depends] = [];
    steps[step].push(depends);
});

var order = [];
while (Object.keys(steps).length > 0) {
    const ns = get_next(steps);
    order.push(ns);
    delete steps[ns];
    for (let d in steps) {
        steps[d] = steps[d].filter(c => c != ns);
    }
}
console.log(order.join(""));
