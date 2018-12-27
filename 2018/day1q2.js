const file = process.argv[2];
var fs = require("fs");
var freqs = [];
fs.readFileSync(file).toString().split("\n").forEach((f) => {
    if (f.length > 0 && !isNaN(f))
        freqs.push(parseInt(f));
});

/* Loop over our array of frequencies until we've found a repeated frequency */
var curr_freq = 0, j = 0;
var frequencies_seen = { 0 : true };
while (true) {
    curr_freq += freqs[j];
    if (curr_freq in frequencies_seen) {
        console.log("First repeated freqency: " + curr_freq);
        break;
    }
    frequencies_seen[curr_freq] = true;

    if (++j == freqs.length)
        j = 0;
}
