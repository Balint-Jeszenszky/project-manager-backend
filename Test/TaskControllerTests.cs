// <copyright file="TaskControllerTests.cs" company="BME VIK AUT Témalaboratóruim">
// Copyright (c) BME VIK AUT Témalaboratóruim. All rights reserved.
// </copyright>

namespace Project_manager_backend.Test
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Xunit;

    /// <summary>
    /// Task coontroller test.
    /// </summary>
    public class TaskControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskControllerTests"/> class.
        /// </summary>
        /// <param name="fixture">Testfixture.</param>
        public TaskControllerTests(TestServerFixture fixture)
        {
            this.fixture = fixture;
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <returns>Nothing.</returns>
        [Fact]
        public async Task CreateTask()
        {
            var task = new Models.Task();
            task.TaskgroupID = 0;
            task.Name = "test";
            task.Description = "teeeeest";
            task.Priority = 1;
            task.Deadline = new System.DateTime(2020, 11, 29, 18, 30, 52);

            var myContent = JsonConvert.SerializeObject(task);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = this.fixture.Client.PostAsync("api/Tasks", byteContent).Result;

            response.EnsureSuccessStatusCode();

            var responseStrong = await response.Content.ReadAsStringAsync();

            responseStrong.Should().Contain("taskgroupID\":0,\"name\":\"test\",\"description\":\"teeeeest\",\"priority\":1,\"deadline\":\"2020-11-29T15:30:52");
        }
    }
}