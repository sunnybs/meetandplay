using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Abstraction.Services.FileService;
using MeetAndPlay.Core.Infrastructure;
using MeetAndPlay.Core.Services;
using MeetAndPlay.Core.Services.FilesService;
using MeetAndPlay.Core.Services.GamesService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace MeetAndPlay.API
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
            services.AddDbContext<MNPContext>(ConfigureDbContext);
            services.AddScoped<IGamesService, GamesService>();
            services.AddScoped<IFilesService, FilesService>();
            services.AddScoped<IPictureRandomizer, PictureRandomizer>();

            services.AddScoped<IPlaceService, PlaceService>();
            
            services.AddHttpContextAccessor();
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "MeetAndPlay.API", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeetAndPlay.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
        
        private void ConfigureDbContext(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(Data.Consts.Infrastructure.MigrationAssembly));
        }
    }
}