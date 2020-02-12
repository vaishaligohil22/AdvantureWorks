using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using AdventureWorks.DBModels;
using AdventureWorks.Security;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AdventureWorks
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
            services.AddAutoMapper(typeof(Startup));

            //Fetching Connection string from APPSETTINGS.JSON  
            var ConnectionString = Configuration.GetConnectionString("MbkDbConstr");

            //Entity Framework  
            services.AddDbContext<AdventureWorks2017Context>(options => options.UseSqlServer(ConnectionString));

            // Add framework services.
            services
                .AddMvc()
                .AddXmlSerializerFormatters();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AdventureWorks API", Version = "v1" });
            });

            services.AddAuthentication(options =>
                {
                    // If an authentication cookie is present, use it to get authentication information
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                    // If authentication is required, and no cookie is present, use Okta (configured below) to sign in
                    options.DefaultChallengeScheme = "AdventureWorks";
                })
                .AddCookie()
                //.AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationHandler.SchemeName, null)
                .AddOAuth("AdventureWorks", options =>
                 {
                     options.ClientId = Configuration["AdventureWorks:ClientId"];
                     options.ClientSecret = Configuration["AdventureWorks:ClientSecret"];
                     options.CallbackPath = new PathString("/signin-github");

                     options.AuthorizationEndpoint = "http://localhost:58504/oauth2/default/v1/authorize";
                     options.TokenEndpoint = "http://localhost:58504/oauth2/default/v1/access_token";
                     //options.UserInformationEndpoint = "http://localhost:58504/user";

                     options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                     options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                     options.ClaimActions.MapJsonKey("urn:AdventureWorks:login", "login");
                     options.ClaimActions.MapJsonKey("urn:AdventureWorks:url", "html_url");
                     options.ClaimActions.MapJsonKey("urn:AdventureWorks:avatar", "avatar_url");

                     options.Events = new OAuthEvents
                     {
                         OnCreatingTicket = async context =>
                         {
                             var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                             request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                             request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                             var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                             response.EnsureSuccessStatusCode();

                             var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                             //context.RunClaimActions(user);
                         }
                     };
                 });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdventureWorks API");
            });

            app.UseHttpsRedirection();

            app.UseStatusCodePages();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
