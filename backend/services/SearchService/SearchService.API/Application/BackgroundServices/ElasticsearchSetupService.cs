using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Options;
using SearchService.API.Application.Configurations;
using SearchService.API.Models;

namespace SearchService.API.Application.BackgroundServices
{
    public class ElasticsearchSetupService : IHostedService
    {
        private readonly ElasticsearchClient _client;
        private readonly ElasticsearchConfiguration _configuration;
        private readonly ILogger<ElasticsearchSetupService> _logger;

        public ElasticsearchSetupService(
            ElasticsearchClient client, 
            IOptions<ElasticsearchConfiguration> configuration,
            ILogger<ElasticsearchSetupService> logger)
        {
            _client = client;
            _configuration = configuration.Value;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var index = _configuration.CourseIndexName;
            var response = await _client.Indices.ExistsAsync(index);

            if (!response.Exists)
            {
                var createIndexResponse = await _client.Indices.CreateAsync<Course>(index, c => c
                    .Mappings(map => map
                        .Properties(p => p
                            .Keyword(k => k.Id)
                            .Keyword(k => k.Level)
                        )
                    )
                );

                if (!createIndexResponse.IsValidResponse)
                {
                    _logger.LogError($"Error creating index {index}: {createIndexResponse.DebugInformation}");
                }
                else
                {
                    _logger.LogInformation($"Index {index} created.");
                }
            }
            else
            {
                Console.WriteLine($"Index {index} already exists.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}