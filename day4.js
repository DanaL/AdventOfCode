function parse_schedule_line(line) {
    const split_at = line.indexOf(']');
    const timestamp = line.slice(1, split_at);
    const action = line.substr(split_at + 2);
    return { ts:timestamp, action:action.trim() };
}

var fs = require("fs");
var schedule = [];
fs.readFileSync("schedule.txt").toString().split("\n").forEach(s => {
    schedule.push(parse_schedule_line(s));
});

schedule.sort(function(a, b){
    if (a.ts > b.ts) return 1;
    if (a.ts == b.ts) return 0;
    return -1;
});

var guards = [];
var sleep_totals = [];
var sleepiest = { total:0 };
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
    var timestamp = schedule[j].ts;
    var action = schedule[j].action;
    if (action.startsWith("Guard")) {
        var octo = action.indexOf('#') + 1
        var guard_num = parseInt(action.slice(octo, action.indexOf(' ', octo + 1)));
        if (!(guard_num in guards)) {
            guards[guard_num] = Array(60).fill(0);
            sleep_totals[guard_num] = 0;
        }
    }
    else if (action == "falls asleep") {
        var start_mins = parseInt(timestamp.substr(timestamp.indexOf(":") + 1));
    }
    else if (action == "wakes up") {
        var end_mins = parseInt(timestamp.substr(timestamp.indexOf(":") + 1));
        for (let j = start_mins; j < end_mins; j++)
            guards[guard_num][j]++;

        mins = end_mins - start_mins;
        sleep_totals[guard_num] += mins;
        if (sleep_totals[guard_num] > sleepiest.total) {
            sleepiest.total = sleep_totals[guard_num];
            sleepiest.guard = guard_num;
        }
    }
}

sg = sleepiest.guard;
most = 0;
minute = 0;
for (j = 0; j < 120; j++) {
    if (guards[sg][j] > most) {
        minute = j;
        most = guards[sg][j];
    }
}
console.log("Q1 answer: " + minute + " " + sg);

most = 0;
sleepy_guard_num = 0;
sleepiest_minute = 0;
for (let g in guards) {
    for (let j = 0; j < 120; j++) {
        if (guards[g][j] > most) {
            most = guards[g][j];
            sleepy_guard_num = g;
            sleepiest_minute = j;
        }
    }
}

console.log("Q2 answer: " + sleepy_guard_num + " " + sleepiest_minute);
