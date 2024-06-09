
using System.Net.Http;

namespace HardwareMonitorService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public Worker(ILogger<Worker> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {


            while (!stoppingToken.IsCancellationRequested)
            {
                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    var httpResponseMessage = await httpClient.GetAsync("http://localhost:8085/data.json");
                    await httpClient.PostAsync("https://nodered.ruben.cl/api/home/computer/state", new StringContent(await httpResponseMessage.Content.ReadAsStringAsync()));
                    //_logger.BeginScope("HTTP Request");
                    //_logger.LogInformation("HTTP Request: {httpResponseMessage}", await httpResponseMessage.Content.ReadAsStringAsync());

                    //if (_logger.IsEnabled(LogLevel.Information))
                    //{
                    //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    //}
                    httpClient.Dispose();

                    //await Task.Delay(1000, stoppingToken);
                }

            }
        }
    }
}
