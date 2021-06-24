using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestImplementaciónJWT.Models.Common;
using TestImplementaciónJWT.Services;

namespace TestImplementaciónJWT
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


            //Extracción del SecretKey del appSettingsSections
            var appSettingsSection = Configuration.GetSection("AppSettings");  //obtengo sección AppSettings del appsettings.json
            services.Configure<AppSettings>(appSettingsSection);   //Agrego el SecretKet a la configuración que almacene en el SecretKey la key obtenida de la sección AppSettings

            //configuración JWT
            var appSettings = appSettingsSection.Get<AppSettings>();  //obtengo el AppSettings para obtener el SecretKey
            var llave = Encoding.ASCII.GetBytes(appSettings.SecretKey); //SecretKey en array de bytes

            services.AddAuthentication(d => //Agregamos autentificación de JWT, le decimos al .Net que coja la autentificación con JWT
            {
                d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(d =>
                {
                    d.RequireHttpsMetadata = false;
                    d.SaveToken = true;
                    d.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(llave),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                }
            );


            //inyección de IUserService y la clase que implementa el método de autenticación 
            // Scoped me permite inyectarlo en CADA PETICIÓN, a través de DependencyInyection
            services.AddScoped<IUserService, UserService>(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication(); //Indico que necesito autenticación
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
