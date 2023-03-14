using Azure.Core;
using Azure.Identity;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Rest;

namespace KTalks.AzureDataFactory.DevOps.Tests.Helpers
{
    public class AdfHelper
    {
        private readonly Settings _settings;
        private readonly DataFactoryManagementClient _dataFactoryManagementClient;

        public AdfHelper()
        {
            _settings = new Settings();
            var token = GetTokenAsync().GetAwaiter().GetResult().Token;
            var managementToken = new TokenCredentials(token);

            _dataFactoryManagementClient = new DataFactoryManagementClient(managementToken)
            {
                SubscriptionId = _settings.AzureSubscriptionId
            };
        }

        public async Task<string> RunPipelineRunOutcomeWithFile(string pipelineName, string fileName)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "FileName", fileName }
            };

            var response = await _dataFactoryManagementClient.Pipelines.CreateRunWithHttpMessagesAsync(_settings.AzureResourceGroupName, _settings.AzureDataFactoryName, pipelineName, parameters: parameters);
            var runId = response.Body.RunId;

            // Waiting for the pipeline to finish.
            var run = await _dataFactoryManagementClient.PipelineRuns.GetAsync(_settings.AzureResourceGroupName, _settings.AzureDataFactoryName, runId);
            while (run.Status is "Queued" or "InProgress" or "Canceling")
            {
                Thread.Sleep(2000);
                run = await _dataFactoryManagementClient.PipelineRuns.GetAsync(_settings.AzureResourceGroupName, _settings.AzureDataFactoryName, runId);
            }

            return run.Status;
        }

        private ValueTask<AccessToken> GetTokenAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var credential = new ClientSecretCredential(_settings.TenantId, _settings.ClientId, _settings.ClientSecret);
            var tokenContext = new TokenRequestContext(new[] { "https://management.azure.com/.default" });
            return credential.GetTokenAsync(tokenContext, cancellationToken);
        }
    }
}
