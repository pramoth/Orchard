using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeSanook.AmazonS3.Models;
using Orchard.Data.Migration;

namespace CodeSanook.AmazonS3
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("AmazonS3SettingsRecord",
                table => table
                .Column<int>("Id",x=>x.PrimaryKey().Identity())
                .Column<string>("AWSAccessKey", x => x.Unlimited())
                .Column<string>("AWSSecretKey", x => x.Unlimited())
                .Column<string>("AWSFileBucket", x => x.Unlimited())
                .Column<string>("RootFolder", x => x.Unlimited())
                .Column<string>("AWSS3PublicUrl", x => x.Unlimited()));
            return 1;
        }
    }
}
