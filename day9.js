
/* Advent of Code 2018 Day 9 (https://adventofcode.com/2018/day/9)

This puzzle involved a game where Christmas elves placed marbles in a ring
following paricular rules. The marbles added were numbered sequentially and if
a placed marble was divisble by 23, instead of being added to the ring, that
player would earn points equal to the # of the marble and it would not be added.
Additionally, the 7th marble counter-clockwise to where the marble should have
been placed is removed and those points scored as well.

My first version of the code (with 416 players and nearly 72,000 marbles placed)
used built-in JS lists. But that was going to be way too inefficenct for part
2, which called for 10x as many marbles. So instead I went with a doubly-linked
list (a circular queue). I was gobsmacked at just how much faster the linked
list version was over the array version was.

I didn't formally definte a link node class, just a function that returns an
anonymous object with references to the next and previous nodes.

*/

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
