using System;
using System.Net.Http;
using Lacuna.Api.Services;
using Lacuna.Domain.Entities;
using Lacuna.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Lacuna.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHttpClient("ApiHttpClient", c => {
                c.BaseAddress = new Uri(Configuration.GetValue<string>("LacunaApi:Url"));
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddScoped<IApiService, ApiService>(sp => new ApiService(Configuration.GetSection("LacunaApi:Endpoints"), sp.GetRequiredService<IHttpClientFactory>()));
            services.AddScoped<ITokenAnalyser, TokenAnalyser>(sp => new TokenAnalyser(sp.GetRequiredService<IApiService>()));

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lacuna Challenge Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lacuna Challenge API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
