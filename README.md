# RabbitMqDemo
Demo project with using RabbitMQ

Tasker is the first console project for generating tasks. It's obsolete.

WebApi project is a client to start work with.

POST /api/task/randomactions
Generates random tasks with random arguments.

JSON {"min": 1, "max": 100, "count":5}
min - minimum value of the arg
max - maximum value of the arg
count - count of tasks

POST /api/task/allactions
Generates tasks with your args for all the workers.

JSON { "args": ["10,8", "4,5"] }
args - arguments for tasks

GET /api/task/getresults?actiontype=webapi.random
or
GET /api/task/getresults?actiontype=webapi.all
Gets results from in-memory storage.

Worker projects (1,2,3) execute tasks with delay on different result values. That's just for example of long running tasks.
Worker1 makes addition, Worker2 makes multiplication and Worker3 makes comparison.

Logger prints logs to the console.

TaskerLib and WorkerLib are dlls with common classes for taskers and workers.
