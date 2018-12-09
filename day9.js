
const num_of_elves = parseInt(process.argv[2]);
const last_marble = parseInt(process.argv[3]);

const scores = Array(num_of_elves).fill(0);
const ring = [0];
var curr_marble = 1;
var pos = 0;

while (curr_marble <= last_marble) {
    for (let p = 0; p < num_of_elves && curr_marble <= last_marble; p++) {
        if (curr_marble % 23 == 0) {
            scores[p] += curr_marble;
            pos -= 7;
            if (pos < 0)
                pos += ring.length;
            else
                pos %= ring.length;
            scores[p] += ring[pos];
            ring.splice(pos, 1);
        }
        else {
            pos = (pos + 1) % ring.length + 1;
            ring.splice(pos, 0, curr_marble);
        }

        ++curr_marble;
    }
}

console.log(scores.reduce((a,b) => Math.max(a, b)));
