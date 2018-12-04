function parse_schedule_line(line) {
    const split_at = line.indexOf(']');
    const timestamp = line.slice(1, split_at);
    const action = line.substr(split_at + 2);
    return { ts:timestamp, action:action.trim() };
}

var fs = require("fs");
var schedule = [];
fs.readFileSync("schedule_sm.txt").toString().split("\n").forEach(s => {
    schedule.push(parse_schedule_line(s));
});

schedule.sort(function(a, b){
    if (a.ts > b.ts) return 1;
    if (a.ts == b.ts) return 0;
    return -1;
});

var guards = [];
for (let j = 0; j < schedule.length; j++) {
    /*
        Simple state machine:
            - if we get a Guard line, extract their ID and check if they are in
                our list yet
            - if we get a falls asleep line, set the start time
            - if we get a wakes up line, record the minutes the guard was asleep
        I am assuming correct data, otherwise I would check the current start
        vs the expected state. (Ie., we should never have a Guard line followed
        immediately by a Wakes Up line. Or two Falls Asleep lines in a row, etc)
    */
    var timestamp = schedule[j].timestamp;
    var action = schedule[j].action;
    if (action.startsWith("Guard")) {
        var octo = action.indexOf('#') + 1
        var guard_num = parseInt(action.slice(octo, action.indexOf(' ', octo + 1)));
        if (!(guard_num in guards)) {
            guards[guard_num] = Array(60).fill(0);
        }
    }
    else if (action == "falls asleep") {
        console.log("flag " + guard_num);
    }
}
