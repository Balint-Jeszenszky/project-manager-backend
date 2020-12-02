// <copyright file="TestServerFixture.cs" company="BME VIK AUT Témalaboratóruim">
// Copyright (c) BME VIK AUT Témalaboratóruim. All rights reserved.
// </copyright>

namespace Project_manager_backend.Test
{
    using System;
    using System.IO;
    using System.Net.Http;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.PlatformAbstractions;
    using Project_manager_backend;

    /// <summary>
    /// Testfixture for tests.
    /// </summary>
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer testServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestServerFixture"/> class.
        /// </summary>
        public TestServerFixture()
        {
            var builder = new WebHostBuilder()
                   .UseContentRoot(this.GetContentRootPath())
                   .UseEnvironment("Development")
                   .UseStartup<Startup>();  // Uses Start up class from your API Host project to configure the test server

            this.testServer = new TestServer(builder);
            this.Client = this.testServer.CreateClient();
        }

        /// <summary>
        /// Gets testClient.
        /// </summary>
        public HttpClient Client { get; }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Client.Dispose();
            this.testServer.Dispose();
        }

        private string GetContentRootPath()
        {
            var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
            var relativePathToHostProject = @"..\..\..\..\..\..\Product.CommandService";
            return Path.Combine(testProjectPath, relativePathToHostProject);
        }
    }
}