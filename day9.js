
const new_node = (val) =>  {
    return { val:val, next:null, prev:null };
};

const insert = (curr, val) => {
    nn = new_node(val);
    curr = curr.next;
    nn.next = curr.next;
    nn.prev = curr;
    curr.next = nn;
    nn.next.prev = nn;

    return nn;
};

const find_and_cut = (curr) => {
    curr = curr.prev.prev.prev.prev.prev.prev.prev;
    curr.prev.next = curr.next;
    curr.next.prev = curr.prev;

    return { cut:curr, new_pos:curr.next };
};

const num_of_elves = parseInt(process.argv[2]);
const last_marble = parseInt(process.argv[3]);
const scores = Array(num_of_elves).fill(0);
var curr_marble = 1;

const head = new_node(0);
head.next = head;
head.prev = head;
var curr = head;

for (let curr_marble = 1; curr_marble <= last_marble; curr_marble++) {
    const p = curr_marble % num_of_elves;
    if (curr_marble % 23 == 0) {
        var result = find_and_cut(curr);
        scores[p] += curr_marble + result.cut.val;
        curr = result.new_pos;
    }
    else {
        curr = insert(curr, curr_marble);
    }
}

console.log(scores.reduce((a,b) => Math.max(a, b)));
