using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace CodeSanook.Configuration
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            //create table for content part records 
            SchemaBuilder.CreateTable("CodeSanookModuleSettingPartRecord",
                table => table
                .ContentPartRecord()
                .Column<string>("AwsAccessKey", c => c.WithLength(255))
                .Column<string>("AwsSecretKey", c => c.WithLength(255))
                .Column<string>("AwsS3BucketName", c => c.WithLength(255))
                .Column<string>("AwsS3PublicUrl", c => c.WithLength(255))
                .Column<string>("FacebookAppId", c => c.WithLength(255)));

            return 1;
        }
    }
}