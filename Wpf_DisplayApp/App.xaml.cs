using CrawlDataServices;
using CrawlStoriesData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using Wpf_DisplayApp.StartupHelpers;

namespace Wpf_DisplayApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? _appHost { get; private set; }
        public App()
        {
            _appHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddFormFactory<CrawDataResultForm>();

                    services.AddSingleton<ICrawlData, HttpClientCrawlData>();
                    services.AddTransient<ICrawWebpageServices, CrawQidianWebpageServices>();
                }).Build();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            await _appHost!.StartAsync();

            var startupForm = _appHost.Services.GetRequiredService<MainWindow>();
            startupForm.Show();

            base.OnStartup(e);
        }
        protected override async void OnExit(ExitEventArgs e)
        {
            await _appHost!.StopAsync();

            base.OnExit(e);
        }
    }
}
