using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CurrieTechnologies.Razor.SweetAlert2;
using IdentityServer4;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Abstraction.Services.FileService;
using MeetAndPlay.Core.Infrastructure;
using MeetAndPlay.Core.Services;
using MeetAndPlay.Core.Services.FilesService;
using MeetAndPlay.Core.Services.GamesService;
using MeetAndPlay.Web.Infrastructure;
using MeetAndPlay.Web.Mapper;
using MeetAndPlay.Web.Middlewares;
using MeetAndPlay.Web.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MeetAndPlay.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MeetAndPlay.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddHttpContextAccessor();

            services.AddOptions();
            services.AddSweetAlert2(options =>
            {
                options.Theme = SweetAlertTheme.Bootstrap4;
            });

            services.AddDbContext<MNPContext>(ConfigureDbContext, ServiceLifetime.Transient);
            
            var authSection = Configuration.GetSection("Auth");
            var authOptions = Configuration.GetSection("Auth").Get<Auth>();
            services.Configure<Auth>(authSection);

            var apiSection = Configuration.GetSection("ApiInfo");
            var apiInfo = apiSection.Get<ApiInfo>();
            services.Configure<ApiInfo>(apiSection);
            services.AddHttpClient<IApiClient, ApiClient>(configureClient =>
            {
                configureClient.BaseAddress = new Uri(apiInfo.Address);
            });
            
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.NonceCookie.SameSite = SameSiteMode.Unspecified;
                    options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;

                    options.Authority = authOptions.Authority;
                    options.ClientId = authOptions.ClientId;
                    options.ClientSecret = authOptions.ClientSecret;
                    options.ResponseType = "code";
                    options.GetClaimsFromUserInfoEndpoint = true;
                    foreach (var scope in authOptions.AllowedScopes.Split(" "))
                    {
                        options.Scope.Add(scope);
                    }
                });
            
            services.AddScoped<AuthenticationStateProvider, CoreAuthenticationStateProvider>();
            services.AddScoped<IUserAuthenticationService, CookieUserAuthenticationService>();
            services.AddScoped<ILobbyService, LobbyService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGamesService, GamesService>();
            services.AddScoped<IFilesService, FilesService>();
            services.AddScoped<FileViewModelsService>();
            
            services.AddServerSideBlazor();
            services.AddScoped<JSHelper>();

            services.AddAutoMapper(typeof(LobbyProfile));
        }

        private void ConfigureDbContext(DbContextOptionsBuilder options)
        {
            options.EnableSensitiveDataLogging();
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(Data.Consts.Infrastructure.MigrationAssembly));
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<OidcRedirectMiddleware>();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}