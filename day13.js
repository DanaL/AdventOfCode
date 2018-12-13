
const map = [];

var fs = require("fs");
const lines = fs.readFileSync("tracks.txt").toString().split("\n").map(l => {
    var sqs = l.split("");
    map.push(sqs);
});

for (let j = 0; j < 50; j++) {
    let s = "";
    for (let k = 0; k < 100; k++) {
	s += map[j][k];
    }
    console.log(s);
}
	    
	

