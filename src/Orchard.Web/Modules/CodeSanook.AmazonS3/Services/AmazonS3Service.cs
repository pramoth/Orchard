using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using CodeSanook.Configuration.Models;
using Orchard;
using Orchard.ContentManagement;

namespace CodeSanook.AmazonS3.Services
{
    public class AmazonS3Service : IAmazonS3Service
    {
        private IOrchardServices orchardService;
        private IAmazonS3 s3Client;
        private ModuleSettingPart setting;
        private ITransferUtility transferUtility;

        public AmazonS3Service(IOrchardServices orchardService)
        {
            this.orchardService = orchardService;
        }

        public IAmazonS3 S3Clicent
        {
            get
            {
                if (s3Client != null)
                {
                    return s3Client;
                }
                var cred = new BasicAWSCredentials(
                    Setting.AwsAccessKey,
                    Setting.AwsSecretKey);
                s3Client = new AmazonS3Client(cred, RegionEndpoint.APSoutheast1);
                return s3Client;
            }
        }

        public ITransferUtility TransferUtility
        {
            get
            {
                if (TransferUtility != null)
                {
                    return TransferUtility;
                }

                var config = new TransferUtilityConfig();
                var transferUtility = new TransferUtility(S3Clicent, config);
                return transferUtility;
            }
        }

        public ModuleSettingPart Setting
        {
            get
            {
                if (setting != null)
                {
                    return setting;
                }

                setting = orchardService.WorkContext.CurrentSite.As<ModuleSettingPart>();
                return setting;
            }
        }
    }
}