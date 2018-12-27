function check_word(word) {
    var letters = {};
    for (let c of word) {
        if (c in letters)
            letters[c]++;
        else
            letters[c] = 1;
    }

    o = { pair : false, triplet : false};
    for (let c in letters) {
        if (letters[c] == 2)
            o.pair = true;
        if (letters[c] == 3)
            o.triplet = true;
    }

    return o;
}

const file = process.argv[2];
var fs = require("fs");
var boxes = fs.readFileSync(file).toString().split("\n");

var doubles = 0, triples = 0;
for (let box of boxes) {
    o = check_word(box);
    if (o.pair) ++doubles;
    if (o.triplet) ++triples;
}

console.log(`Checksum: ${doubles * triples}`);
