namespace ItLabs.MBox.Contracts.Dtos
{
    public class AwsSettings
    {
        public string AwsS3BucketName { get; set; }
        public string AwsS3SecretAccessKey { get; set; }
        public string AwsS3AccessKey { get; set; }
        public string AwsSesFromAddress { get; set; }
        public string AwsSesUsername { get; set; }
        public string AwsSesPassword { get; set; }
        public string AwsSesHost { get; set; }
        public int AwsSesPort { get; set; }
        public string ContactFormRecieverMail { get; set; }
        public string TestRecieverMail { get; set; }
    }
}
