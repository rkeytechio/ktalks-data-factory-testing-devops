namespace KTalks.AzureDataFactory.DevOps.Tests.Helpers
{
    internal class Settings
    {
        public string TenantId => Environment.GetEnvironmentVariable("AZURE_TENANT_ID") ?? string.Empty;
        public string ClientId => Environment.GetEnvironmentVariable("CLIENT_ID") ?? string.Empty;
        public string ClientSecret => Environment.GetEnvironmentVariable("CLIENT_SECRET") ?? string.Empty;
        public string AzureSubscriptionId => Environment.GetEnvironmentVariable("AZURE_SUBSCRIPTION_ID") ?? string.Empty;
        public string AzureResourceGroupName => Environment.GetEnvironmentVariable("AZURE_RESOURCE_GROUP_NAME") ?? string.Empty;
        public string AzureDataFactoryName => Environment.GetEnvironmentVariable("AZURE_DATA_FACTORY_NAME") ?? string.Empty;
        public string AzureDataLakeName => Environment.GetEnvironmentVariable("AZURE_DATA_LAKE_NAME") ?? string.Empty;
    }
}
