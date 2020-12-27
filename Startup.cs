using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using WebApp.SoE.Helpers;
using WebApp.SoE.Services;
using WebApp.SoE.ViewModels;
using Westwind.AspNetCore.LiveReload;

namespace WebApp.SoE
{
	public class Startup
	{
		public Startup(IConfiguration configuration) => Configuration = configuration;

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IAppInfoService, AppInfoService>();
			services.AddScoped<CompareViewModel>(sp => new() {LocalStorage = sp.GetRequiredService<ProtectedLocalStorage>()});
			services.AddScoped<SetOffsetValueViewModel>(sp => new()
			{
				JsRuntime = sp.GetRequiredService<IJSRuntime>(),
				LocalStorage = sp.GetRequiredService<ProtectedLocalStorage>()
			});

			services.AddOptions<Settings>().Bind(Configuration.GetSection(nameof(Settings)));
			services.AddSingleton(cfg => cfg.GetService<IOptionsMonitor<Settings>>()!.CurrentValue);

			services.AddLiveReload(config => {
				config.LiveReloadEnabled = true;
				config.ClientFileExtensions = ".css,.js,.htm,.html";
				config.FolderToMonitor = "~/../";
			});

			services.AddHttpClient();
			services.AddRazorPages();
			services.AddServerSideBlazor();
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

#if DEBUG
			app.UseLiveReload();
#endif
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
			});
		}
	}
}
