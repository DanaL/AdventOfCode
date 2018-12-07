
function get_available_tasks(steps) {
    var avail = Object.keys(steps).filter(s => steps[s].length == 0);
    return avail.sort();
}

function get_available_worker(workers) {
    for (w of workers) {
        if (w.task === '')
            return w;
    }

    return null;
}

const steps = [];
var fs = require("fs");
fs.readFileSync("instructions_sm.txt").toString().split("\n").forEach(s => {
    let step = s.substr(s.indexOf("step") + 5, 1);
    let depends = s.substr(5, 1);
    if (!(step in steps))
        steps[step] = [];
    if (!(depends in steps))
        steps[depends] = [];
    steps[step].push(depends);
});

const workers = []
const num_of_workers = 2;
for (let j = 0; j < 5; j++)
    workers[j] = { task:'', finished_at:Number.MAX_SAFE_INTEGER };

var time = 0;
const base_cost = 0;
while (Object.keys(steps).length > 0) {
    const ns = get_available_tasks(steps);
    console.log(time + ': Tasks: ' + ns);
    for (task of ns) {
        w = get_available_worker(workers);
        if (w !== null) {
            delete steps[ns];
            w.task = task;
            w.finished_at = time + base_cost + task.charCodeAt(0) - 65;
        }
    }

    ++time;

    workers.forEach(w => {
        if (w.finished_at <= time) {
            // Remove the completed tasks from the depencies lists
            console.log("Finished: " + w.task);
            for (let d in steps) {
                steps[d] = steps[d].filter(t => t != w.task);
            }

            w.task = '';
        }
    });

    if (time > 10) break;
}
console.log(workers);
