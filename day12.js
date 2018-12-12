const apply_rules = (rules, ca) => {
    var next_ca = "".padStart(ca.length, ".");
    for (let j = 0; j < ca.length - 5; j++) {
        for (r of rules) {
            if (ca.substr(j, 5) == r.rule)
                next_ca = next_ca.substr(0,j + 2) + r.res + next_ca.substr(j + 3);
        }
    }

    return next_ca;
};

const count_score = (ca, offset) => {
    var score = 0;
    for (let j = 0; j < ca.length; j++)
        score += ca[j] == "#" ? j - offset : 0;

    return score;
}

var fs = require("fs");
const lines = fs.readFileSync("plants.txt").toString().split("\n");

const initial_state = lines[0].substr(15).trim();
rules = lines.slice(2).map(l => {
    return {rule:l.slice(0, 5), res:l.slice(9, 10)};
});

const padding = ".........................";
var ca = "....." + initial_state + padding + "................................................................................................................";

for (let j = 0; j < 131; j ++) {
    ca = apply_rules(rules, ca);
    let a = ca.substr(ca.indexOf('#')).padEnd(ca.length, '.');
    let s = `${ca} ${j + 1} ${ca.split("").filter(c => c == "#").length} ${count_score(ca, 5)}`;
    console.log(s);
}
