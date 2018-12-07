
function get_available_tasks(tasks) {
    var avail = Object.keys(tasks).filter(s => tasks[s].length == 0);
    return avail.sort();
}

function get_available_worker(workers) {
    for (w of workers) {
        if (w.task === '')
            return w;
    }

    return null;
}

function work_remaining(tasks, workers) {
    return Object.keys(tasks).length > 0 ||
        workers.filter(w => w.task !== '').length > 0;
}

const tasks = [];
var fs = require("fs");
fs.readFileSync("instructions.txt").toString().split("\n").forEach(s => {
    let step = s.substr(s.indexOf("step") + 5, 1);
    let depends = s.substr(5, 1);
    if (!(step in tasks))
        tasks[step] = [];
    if (!(depends in tasks))
        tasks[depends] = [];
    tasks[step].push(depends);
});

const workers = []
const num_of_workers = 5;
for (let j = 0; j < num_of_workers; j++)
    workers[j] = { task:'', finished_at:Number.MAX_SAFE_INTEGER };

var time = 0;
const base_cost = 60;
while (work_remaining(tasks, workers)) {
    const ns = get_available_tasks(tasks);
    //console.log(time + ': Tasks: ' + ns);
    for (task of ns) {
        w = get_available_worker(workers);
        if (w !== null) {
            //console.log("   Assign task " + task);
            delete tasks[task];
            w.task = task;
            w.finished_at = time + base_cost + task.charCodeAt(0) - 64;
        }
    }

    ++time;

    workers.forEach(w => {
        if (w.finished_at == time) {
            // Remove the completed tasks from the depencies lists
            //console.log("   Finished: " + w.task);
            for (let d in tasks) {
                tasks[d] = tasks[d].filter(t => t != w.task);
            }

            w.task = '';
        }
    });

    //if (time > 10) break;
}

console.log(time);
