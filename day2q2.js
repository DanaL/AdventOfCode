function same_letters(word1, word2) {
    var result = '';
    for (var j = 0; j < word1.length; j++) {
        if (word1[j] == word2[j])
            result += word1[j];
    }

    return result;
}

var fs = require("fs");
const boxes = fs.readFileSync("boxes.txt").toString().split("\n");

/* Time for a big ol' O(n^2) nested loop I guess */
finished = false;
start = 0;
for (var j = 0; j < boxes.length - 1; j++) {
    for (var k = start; k < boxes.length; k++) {
        const result = same_letters(boxes[j], boxes[k]);
        if (boxes[j].length - result.length == 1) {
            console.log(result);
            finished = true;
            break;
        }
    }

    if (finished) break;
    ++start;
}
