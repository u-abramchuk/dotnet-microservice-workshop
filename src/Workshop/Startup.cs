﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace Workshop
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks(checks =>
            {
                checks.AddCheck("graceful-shutdown", () => GracefulShutdown.SigtermReceived ? HealthCheckResult.Unhealthy("Shutting down") : HealthCheckResult.Healthy("OK"), TimeSpan.FromMilliseconds(100));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            appLifetime.ShutdownGracefully(Configuration.GetValue<int?>("GRACEFUL_SHUTDOWN_TIMEOUT") ?? 2);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
                    {
                        await context.Response.WriteAsync("Hello World!");
                    });
        }
    }
}
