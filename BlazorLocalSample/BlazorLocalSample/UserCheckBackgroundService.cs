
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace BlazorLocalSample
{
    public class UserCheckBackgroundService : BackgroundService
    {
        private readonly ILogger<UserCheckBackgroundService> logger;
        private readonly IServer server;
        private volatile bool _ready = false;
        public UserCheckBackgroundService(ConnectedUserList connectedUserList, IHostApplicationLifetime hostApplicationLifetime, ILogger<UserCheckBackgroundService> logger, IServer server)
        {
            ConnectedUserList = connectedUserList;
            HostApplicationLifetime = hostApplicationLifetime;
            hostApplicationLifetime.ApplicationStarted.Register(() => _ready = true);
            this.logger = logger;
            this.server = server;
        }

        public ConnectedUserList ConnectedUserList { get; }
        public IHostApplicationLifetime HostApplicationLifetime { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            int zeroTime = 0;

            while (!_ready)
            {
                // App hasn't started yet, keep looping!
                await Task.Delay(1_000, stoppingToken);
            }
            StartWebBrowser();
            await Task.Delay(1000, stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                if (ConnectedUserList.Users.Count == 0)
                {
                    if (zeroTime > 2)
                    {
                        HostApplicationLifetime.StopApplication();
                        break;
                    }
                    else
                    {
                        zeroTime += 1;
                        await Task.Delay(5000, stoppingToken);
                    }
                    logger.LogInformation($"没有用户在线 {zeroTime}");
                }
                else
                {
                    zeroTime = 0;
                    logger.LogInformation($"有用户在线： {ConnectedUserList.Users.Count}");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }

        private void StartWebBrowser()
        {
            var addressFeature = server.Features.Get<IServerAddressesFeature>();
            foreach (var address in addressFeature!.Addresses)
            {
                var uri = new Uri(address);
                var port = uri.Port;
                Process.Start(new ProcessStartInfo($"http://localhost:{port}") { UseShellExecute = true });
                return;
            }
        }


    }
}
