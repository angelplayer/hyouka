using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hyouka_api.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace hyouka_api
{
    public class Startup
    {

        public const string DATABASE_FILE = "hyouka.db";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR();

            services.AddEntityFrameworkSqlite().AddDbContext<HyoukaContext>();
            services.AddEntityFrameworkSqlite().AddDbContext<HyoukaContext>();
            services.AddSwaggerGen(x =>
            {
                x.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                x.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                  { "Bearer", new string[] { } }
                });

                x.SwaggerDoc("v1", new Info { Title = "RealWorld API", Version = "v1" });
                x.CustomSchemaIds(y => y.FullName);
                x.DocInclusionPredicate((version, apiDescription) => true);
                x.TagActionsBy(y => y.GroupName);
            });

            services
          .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
          .AddJsonOptions(opt =>
          {
              opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
          });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger(c =>
                {
                    c.RouteTemplate = "swagger/{documentName}/swagger.json";
                });
            // Enable middleware to serve swagger-ui assets(HTML, JS, CSS etc.)
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "RealWorld API V1");
            });
        }
    }
}
