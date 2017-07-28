using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeSanook.Configuration.Models
{
    public class CodeSanookModuleSettingPart : ContentPart<CodeSanookModuleSettingPartRecord>
    {

        public virtual string AwsAccessKey
        {
            get { return Record.AwsAccessKey; }
            set { Record.AwsAccessKey = value; }
        }

        public virtual string AwsSecretKey
        {
            get { return Record.AwsSecretKey; }
            set { Record.AwsSecretKey = value; }
        }

        public virtual string AwsS3BucketName
        {
            get { return Record.AwsS3BucketName; }
            set { Record.AwsS3BucketName = value; }
        }

        public virtual string AwsS3PublicUrl
        {
            get { return Record.AwsS3PublicUrl; }
            set { Record.AwsS3PublicUrl = value; }
        }

        public virtual string FacebookAppId 
        {
            get { return Record.FacebookAppId; }
            set { Record.FacebookAppId = value; }
        }

    }
}