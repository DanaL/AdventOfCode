


var acc = (function() {
    var total = 0;

    return {
        accumulate: function(s) {
            total += parseInt(s);
        },
        get: function() {
            console.log("Final frequency: " + total);
        }
    };
}());

var fs = require('fs');
const file = process.argv[2];
const readline = require('readline');
const rl = readline.createInterface({
    input: fs.createReadStream(file),
    crlfDelay: Infinity
});

rl.on('close', acc.get);
rl.on('line', acc.accumulate);
