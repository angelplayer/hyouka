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
// using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using System.IO;
using NJsonSchema;
using NSwag.AspNetCore;
using System.Reflection;

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
      services.AddCors(option =>
      {
        option.AddPolicy("api", x =>
        x.AllowAnyOrigin().AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials());
      });

      services.AddMediatR();
      services.AddAutoMapper(GetType().Assembly);
      services.AddScoped<IPasswordHasher, PasswordHaser>();
      services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
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
      app.UseStaticFiles();
      app.UseFileServer();
      app.UseCors("api");
      // app.UseHttpsRedirection();
      app.UseMvc();
      app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, settings =>
        {
          settings.GeneratorSettings.DefaultPropertyNameHandling =
              PropertyNameHandling.CamelCase;
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
