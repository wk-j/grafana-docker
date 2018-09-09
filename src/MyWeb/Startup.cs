using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Configuration;
using App.Metrics.Extensions.Reporting.InfluxDB;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using App.Metrics.Reporting.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MyWeb {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {

            var uri = new Uri("http://localhost:8086");
            var database = "dotnet";

            services.AddMetrics(options => {
                options.WithGlobalTags((globalTags, info) => {
                    globalTags.Add("app", info.EntryAssemblyName);
                    globalTags.Add("env", "state");
                });
            })
            .AddHealthChecks()
            .AddReporting(factory => {
                factory.AddInfluxDb(new InfluxDBReporterSettings {
                    InfluxDbSettings = new InfluxDBSettings(database, uri),
                    ReportInterval = TimeSpan.FromSeconds(5)
                });
            })
            .AddMetricsMiddleware(options => options.IgnoredHttpStatusCodes = new[] { 404 });

            services.AddMvc(options =>
                options.AddMetricsResourceFilter()
            ).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMetrics();
            app.UseMetricsReporting(lifetime);
            app.UseMvc();
        }
    }
}
