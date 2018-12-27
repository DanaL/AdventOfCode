

const file = process.argv[2];
var fs = require("fs");
var freqs = fs.readFileSync(file).toString().split("\n");

var total = 0;
for (var j = 0; j < freqs.length; j++) {
    if (freqs[j].length == 0 || isNaN(freqs[j]))
        continue;
    else
        total += parseInt(freqs[j]);
}
console.log("Final frequency: " + total);
