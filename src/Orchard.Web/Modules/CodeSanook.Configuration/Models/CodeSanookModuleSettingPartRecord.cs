using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeSanook.Configuration.Models
{
    public class CodeSanookModuleSettingPartRecord : ContentPartRecord
    {
        public virtual string AwsAccessKey { get; set; }
        public virtual string AwsSecretKey { get; set; }
        public virtual string AwsS3BucketName { get; set; }
        public virtual string AwsS3PublicUrl { get; set; }
        public virtual string FacebookAppId { get; set; }
    }
}