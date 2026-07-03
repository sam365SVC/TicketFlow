using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Infrastructure.Configurations
{
    public class AwsS3Options
    {
        public const string SectionName = "AWS";

        public string? AccessKey { get; set; }
        public string? SecretKey { get; set; }
        public string? Region { get; set; } = "us-east-1";
        public string? BucketName { get; set; }
    }
}
