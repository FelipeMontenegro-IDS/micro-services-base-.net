namespace Shared.Configurations;

public class AzureBlobStorageOption
{
    public string Protocol { get; set; }
    public string AccountName { get; set; }
    public string AccountKey { get; set; }
    public string EndpointSuffix { get; set; }
}