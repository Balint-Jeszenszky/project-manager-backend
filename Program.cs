// <copyright file="Program.cs" company="BME VIK AUT Témalaboratóruim">
// Copyright (c) BME VIK AUT Témalaboratóruim. All rights reserved.
// </copyright>

namespace Project_manager_backend
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Main class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main. Everything starts here.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// IhostBuilder.
        /// </summary>
        /// <param name="args">Host.CreateDefaultBuilder args.</param>
        /// <returns>IDK its auto generated.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
