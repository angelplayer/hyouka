using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hyouka_api.Infrastructure;
using hyouka_api.Infrastructure.security;
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
using AutoMapper;
using System.IO;

namespace hyouka_api
{
  public class Startup
  {
    public static string WebRootPath { get; private set; }

    public const string DATABASE_FILE = "hyouka.db";

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddJwt();
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

      // services.AddCors(options => options.AddPolicy("AllowSpecificOrigin",
      //       builder => builder.WithOrigins("http://localhost")));

      services.AddCors();

      // services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
      // {
      //   builder.AllowAnyOrigin()
      //            .AllowAnyMethod()
      //            .AllowAnyHeader();
      // }));


      services.AddMediatR();
      services.AddAutoMapper(GetType().Assembly);
      services.AddScoped<IPasswordHasher, PasswordHaser>();
      services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
      services
      .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
      .AddJsonOptions(opt =>
      {
        opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
      }).AddXmlDataContractSerializerFormatters();
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
      app.UseStaticFiles();
      app.UseFileServer();
      app.UseCors(options => options.WithOrigins("http://localhost")
            .AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()
            .AllowCredentials());
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

      WebRootPath = env.WebRootPath;
    }

    public static string MapPath(string path, string basePath = null)
    {
      if (string.IsNullOrEmpty(basePath))
      {
        basePath = Startup.WebRootPath;
      }
      path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
      return Path.Combine(basePath, path);
    }
  }
}
