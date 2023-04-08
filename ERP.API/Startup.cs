using System.Reflection;
using System.Text;
using ERP.API.Security;
using ERP.Infrastructure.DependencyInjection;
using ERP.Services.DependencyInjection;
using ERP.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WatchDog;

namespace ERP.API;

public class Startup
{
    private const string SecretKey = "aBCDE4JNKNLKDNARVAJN545N4J5N4PL4H4P44H5JBSSDBNF3453S2223KJNH";
    private const string Version = "2";

    public static readonly SymmetricSecurityKey SINGING_KEY = new(Encoding.UTF8.GetBytes(SecretKey));

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddInfrastrucutreCollections(Configuration);
        services.AddScoped(typeof(ICurrentUser), typeof(CurrentUser));
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddAplicationServices();
        services.AddWatchDogServices();
        services.AddCors(options =>
        {
            options.AddPolicy("SpaLocal", builder =>
            {
                //builder.WithOrigins("http://localhost:3000").AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });
        services.AddAuthentication(op =>
        {
            op.DefaultAuthenticateScheme = "JwtBearer";
            op.DefaultSignInScheme = "JwtBearer";
        });

        services.AddControllers();


        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ERP.API", Version = Version });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Autorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HADA.ERP.API V" + Version));
        }

        app.UseHttpsRedirection();


        app.UseRouting();
        app.UseCors("SpaLocal");
        app.UseAuthorization();
        app.UseDeveloperExceptionPage();
        //app.UseWatchDog(opt =>
        //{
        //    opt.WatchPageUsername = "jmontilla";
        //    opt.WatchPagePassword = "830434";
        //});
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}