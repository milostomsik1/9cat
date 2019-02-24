using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using _9Cat.Domain.Services;

namespace _9Cat.Api
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			RegisterIoC(services);
			AddAuthentication(services);
			AddAuthorization(services);
		}

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

			app.UseHttpsRedirection();

			app.UseMvc();
		}

		private void RegisterIoC(IServiceCollection services)
		{
			//Singletons
			services.AddSingleton(_configuration);

			//Scoped

			//Transient
			services.AddTransient<IMagicLinkService, MagicLinkService>();
			services.AddTransient<IJwtTokenService, JwtTokenService>();
		}

		private void AddAuthentication(IServiceCollection services)
		{
			services.AddAuthentication(o =>
			{
				o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(o =>
			{
				o.SaveToken = true;
				o.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = _configuration.GetValue<string>("Hostname"),
					ValidAudience = _configuration.GetValue<string>("Hostname"),
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("IssuerSigningKey")))
				};
			});
		}

		private void AddAuthorization(IServiceCollection services)
		{
			services.AddAuthorization(o =>
			{
				o.AddPolicy("IsMyself", p => p.RequireClaim("sub", "milostomsik1@gmail.com"));
			});
		}
	}
}
