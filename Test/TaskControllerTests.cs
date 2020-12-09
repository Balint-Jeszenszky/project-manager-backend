// <copyright file="TaskControllerTests.cs" company="BME VIK AUT Témalaboratóruim">
// Copyright (c) BME VIK AUT Témalaboratóruim. All rights reserved.
// </copyright>

namespace Project_manager_backend.Test
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Project_manager_backend.Controllers;

    /// <summary>
    /// Task coontroller test.
    /// </summary>
    [TestClass]
    public class TaskControllerTests
    {
        /// <summary>
        /// Should get an existing task by ID.
        /// </summary>
        /// <returns>Nothing.</returns>
        [TestMethod]
        public async Task GetExistingTaskByID()
        {
            var options = new DbContextOptionsBuilder<Models.ProjectManagerDBContext>()
            .UseInMemoryDatabase(databaseName: "ProjectManagerDB")
            .Options;

            var task = new Models.Task()
            {
                ID = 42,
                TaskgroupID = 2,
                Name = "test",
                Description = "teeeeest",
                Priority = 1,
                Deadline = new System.DateTime(2020, 11, 29, 18, 30, 52),
            };

            using (var context = new Models.ProjectManagerDBContext(options))
            {
                context.Tasks.Add(task);
                context.SaveChanges();
            }

            using (var context = new Models.ProjectManagerDBContext(options))
            {
                var controller = new TaskController(context);

                var result = await controller.GetTask(42);

                result.Result.Should().BeOfType<Microsoft.AspNetCore.Mvc.OkObjectResult>();

                var resultData = (Microsoft.AspNetCore.Mvc.OkObjectResult)result.Result;

                Assert.AreEqual(((Models.Task)resultData.Value).ID, task.ID);
                Assert.AreEqual(((Models.Task)resultData.Value).TaskgroupID, task.TaskgroupID);
                Assert.AreEqual(((Models.Task)resultData.Value).Name, task.Name);
                Assert.AreEqual(((Models.Task)resultData.Value).Description, task.Description);
                Assert.AreEqual(((Models.Task)resultData.Value).Priority, task.Priority);
                Assert.AreEqual(((Models.Task)resultData.Value).Deadline, task.Deadline);
            }
        }

        /// <summary>
        /// Should get a not existing task by ID.
        /// </summary>
        /// <returns>Nothing.</returns>
        [TestMethod]
        public async Task GetNotExistingTaskByID()
        {
            var options = new DbContextOptionsBuilder<Models.ProjectManagerDBContext>()
            .UseInMemoryDatabase(databaseName: "ProjectManagerDB")
            .Options;

            using var context = new Models.ProjectManagerDBContext(options);
            var controller = new TaskController(context);

            var result = await controller.GetTask(12);

            result.Result.Should().BeOfType<Microsoft.AspNetCore.Mvc.NotFoundResult>();
        }

        /// <summary>
        /// Should get a list of tasks by taskgroupID.
        /// </summary>
        /// <returns>Nothing.</returns>
        [TestMethod]
        public async Task GetTasksByTaskgroupID()
        {
            var options = new DbContextOptionsBuilder<Models.ProjectManagerDBContext>()
            .UseInMemoryDatabase(databaseName: "ProjectManagerDB")
            .Options;

            using (var context = new Models.ProjectManagerDBContext(options))
            {
                context.Tasks.Add(new Models.Task() { TaskgroupID = 5 });
                context.Tasks.Add(new Models.Task() { TaskgroupID = 3 });
                context.Tasks.Add(new Models.Task() { TaskgroupID = 5 });
                context.SaveChanges();
            }

            using (var context = new Models.ProjectManagerDBContext(options))
            {
                var controller = new TaskController(context);

                var result = await controller.GetGroup(5);

                result.Result.Should().BeOfType<Microsoft.AspNetCore.Mvc.OkObjectResult>();

                var resultData = (Microsoft.AspNetCore.Mvc.OkObjectResult)result.Result;

                Assert.AreEqual(((List<Models.Task>)resultData.Value).Count, 2);
            }
        }

        /// <summary>
        /// Should create a task.
        /// </summary>
        /// <returns>Nothing.</returns>
        [TestMethod]
        public async Task CreateTask()
        {
            var options = new DbContextOptionsBuilder<Models.ProjectManagerDBContext>()
            .UseInMemoryDatabase(databaseName: "ProjectManagerDB")
            .Options;

            using var context = new Models.ProjectManagerDBContext(options);
            var controller = new TaskController(context);

            var result = await controller.PostTask(new Models.Task() { ID = 72, Name = "originalname" });

            result.Result.Should().BeOfType<Microsoft.AspNetCore.Mvc.CreatedAtActionResult>();

            Assert.AreEqual("originalname", (await context.Tasks.FindAsync(72)).Name);
        }

        /// <summary>
        /// Should not update a task.
        /// </summary>
        /// <returns>Nothing.</returns>
        [TestMethod]
        public async Task TryUpdateTaskWithWrongID()
        {
            var options = new DbContextOptionsBuilder<Models.ProjectManagerDBContext>()
            .UseInMemoryDatabase(databaseName: "ProjectManagerDB")
            .Options;

            var task = new Models.Task() { ID = 76 };

            using (var context = new Models.ProjectManagerDBContext(options))
            {
                context.Tasks.Add(task);
                context.SaveChanges();
            }

            using (var context = new Models.ProjectManagerDBContext(options))
            {
                var controller = new TaskController(context);

                var result = await controller.UpdateTask(72, task) as Microsoft.AspNetCore.Mvc.BadRequestResult;
                Assert.IsNotNull(result);
                Assert.AreEqual(400, result.StatusCode);
            }
        }

        /// <summary>
        /// Should update task priority.
        /// </summary>
        /// <returns>Nothing.</returns>
        [TestMethod]
        public async Task UpdateTaskPriority()
        {
            var options = new DbContextOptionsBuilder<Models.ProjectManagerDBContext>()
            .UseInMemoryDatabase(databaseName: "ProjectManagerDB")
            .Options;

            using (var context = new Models.ProjectManagerDBContext(options))
            {
                context.Tasks.Add(new Models.Task() { ID = 100, Priority = 3, TaskgroupID = 9 });
                context.Tasks.Add(new Models.Task() { ID = 101, Priority = 2, TaskgroupID = 9 });
                context.SaveChanges();
            }

            using (var context = new Models.ProjectManagerDBContext(options))
            {
                var controller = new TaskController(context);

                var result = await controller.UpdateTask(100, new Models.Task() { ID = 100, Priority = 2, TaskgroupID = 9 }) as Microsoft.AspNetCore.Mvc.NoContentResult;
                Assert.IsNotNull(result);
                Assert.AreEqual(204, result.StatusCode);

                Assert.AreEqual(2, (await context.Tasks.FindAsync(100)).Priority);
                Assert.AreEqual(3, (await context.Tasks.FindAsync(101)).Priority);
            }
        }

        /// <summary>
        /// Should update task data.
        /// </summary>
        /// <returns>Nothing.</returns>
        [TestMethod]
        public async Task UpdateTaskData()
        {
            var options = new DbContextOptionsBuilder<Models.ProjectManagerDBContext>()
            .UseInMemoryDatabase(databaseName: "ProjectManagerDB")
            .Options;

            using (var context = new Models.ProjectManagerDBContext(options))
            {
                context.Tasks.Add(new Models.Task()
                {
                    ID = 128,
                    Priority = 3,
                    TaskgroupID = 9,
                    Name = "originalname",
                    Description = "originaldesc",
                    Deadline = new System.DateTime(2020, 12, 09, 18, 30, 52),
                });
                context.SaveChanges();
            }

            using (var context = new Models.ProjectManagerDBContext(options))
            {
                var controller = new TaskController(context);

                var result = await controller.UpdateTask(128, new Models.Task()
                {
                    ID = 128,
                    Priority = 3,
                    TaskgroupID = 9,
                    Name = "newname",
                    Description = "newdesc",
                    Deadline = new System.DateTime(2020, 12, 09, 18, 50, 52),
                }) as Microsoft.AspNetCore.Mvc.NoContentResult;

                Assert.IsNotNull(result);
                Assert.AreEqual(204, result.StatusCode);

                var updatedTask = await context.Tasks.FindAsync(128);
                Assert.AreEqual("newname", updatedTask.Name);
                Assert.AreEqual("newdesc", updatedTask.Description);
                Assert.AreEqual(new System.DateTime(2020, 12, 09, 18, 50, 52), updatedTask.Deadline);
            }
        }

        /// <summary>
        /// Should update group.
        /// </summary>
        /// <returns>Nothing.</returns>
        [TestMethod]
        public async Task UpdateGroup()
        {
            var options = new DbContextOptionsBuilder<Models.ProjectManagerDBContext>()
            .UseInMemoryDatabase(databaseName: "ProjectManagerDB")
            .Options;

            using (var context = new Models.ProjectManagerDBContext(options))
            {
                context.Tasks.Add(new Models.Task()
                {
                    ID = 192,
                    Priority = 3,
                    TaskgroupID = 9,
                    Name = "originalname",
                    Description = "originaldesc",
                    Deadline = new System.DateTime(2020, 12, 09, 18, 30, 52),
                });
                context.SaveChanges();
            }

            using (var context = new Models.ProjectManagerDBContext(options))
            {
                var controller = new TaskController(context);

                var result = await controller.UpdateTask(192, new Models.Task()
                {
                    ID = 192,
                    Priority = 3,
                    TaskgroupID = 7,
                    Name = "originalname",
                    Description = "originaldesc",
                    Deadline = new System.DateTime(2020, 12, 09, 18, 30, 52),
                }) as Microsoft.AspNetCore.Mvc.NoContentResult;

                Assert.IsNotNull(result);
                Assert.AreEqual(204, result.StatusCode);

                var updatedTask = await context.Tasks.FindAsync(192);
                Assert.AreEqual(7, updatedTask.TaskgroupID);
                Assert.AreEqual(1, updatedTask.Priority);
            }
        }

        /// <summary>
        /// Should delete task.
        /// </summary>
        /// <returns>Nothing.</returns>
        [TestMethod]
        public async Task DeleteTask()
        {
            var options = new DbContextOptionsBuilder<Models.ProjectManagerDBContext>()
            .UseInMemoryDatabase(databaseName: "ProjectManagerDB")
            .Options;

            using (var context = new Models.ProjectManagerDBContext(options))
            {
                context.Tasks.Add(new Models.Task()
                {
                    ID = 256,
                    Priority = 3,
                    TaskgroupID = 9,
                    Name = "originalname",
                    Description = "originaldesc",
                    Deadline = new System.DateTime(2020, 12, 09, 18, 30, 52),
                });
                context.SaveChanges();
            }

            using (var context = new Models.ProjectManagerDBContext(options))
            {
                var controller = new TaskController(context);

                var result = await controller.DeleteProject(256);

                result.Result.Should().BeOfType<Microsoft.AspNetCore.Mvc.OkObjectResult>();

                Assert.AreEqual(null, await context.Tasks.FindAsync(256));
            }
        }

        /// <summary>
        /// Should try delete not existingtask.
        /// </summary>
        /// <returns>Nothing.</returns>
        [TestMethod]
        public async Task DeleteNotExistingTask()
        {
            var options = new DbContextOptionsBuilder<Models.ProjectManagerDBContext>()
            .UseInMemoryDatabase(databaseName: "ProjectManagerDB")
            .Options;

            using (var context = new Models.ProjectManagerDBContext(options))
            {
                var controller = new TaskController(context);

                var result = await controller.DeleteProject(512);

                result.Result.Should().BeOfType<Microsoft.AspNetCore.Mvc.NotFoundResult>();
            }
        }
    }
}