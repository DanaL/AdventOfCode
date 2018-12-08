
let get_available_tasks = (tasks) => Object.keys(tasks).filter(s => tasks[s].length == 0).sort();

let get_available_worker = (workers) => {
    for (w of workers) {
        if (w.task === '')
            return w;
    }

    return null;
}

let work_remaining = (tasks, workers) =>
    Object.keys(tasks).length > 0 ||
        workers.filter(w => w.task !== '').length > 0;

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
    for (task of get_available_tasks(tasks)) {
        const w = get_available_worker(workers);
        if (w !== null) {
            delete tasks[task];
            w.task = task;
            w.finished_at = time + base_cost + task.charCodeAt(0) - 64;
        }
    }

    ++time;

    workers.forEach(w => {
        if (w.finished_at == time) {
            // Remove the completed tasks from the depencies lists
            for (let d in tasks) {
                tasks[d] = tasks[d].filter(t => t != w.task);
            }

            w.task = '';
        }
    });
}

console.log(time);
