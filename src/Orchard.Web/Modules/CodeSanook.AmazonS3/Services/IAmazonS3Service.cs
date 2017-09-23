using Amazon.S3;
using Amazon.S3.Transfer;
using CodeSanook.Configuration.Models;
using Orchard;

namespace CodeSanook.AmazonS3.Services
{
    public interface IAmazonS3Service:IDependency
    {
        ModuleSettingPart Setting { get; }
        IAmazonS3 S3Clicent { get; }
        ITransferUtility TransferUtility { get; }
    }
}