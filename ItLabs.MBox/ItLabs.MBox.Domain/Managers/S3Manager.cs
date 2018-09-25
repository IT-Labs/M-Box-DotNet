using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ItLabs.MBox.Domain.Managers
{
    public class S3Manager : IS3Manager
    {

        private readonly IConfigurationManager _configurationManager;
        public S3Manager(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }
        public async Task<string> UploadFileAsync(string filePath)
        {
            try
            {
                var accessKey = _configurationManager.GetOne(filter: x => x.Key == ConfigurationKey.AwsS3AccessKey).Value;
                var secretKey = _configurationManager.GetOne(filter: x => x.Key == ConfigurationKey.AwsS3SecretAccessKey).Value;
                var bucketName = _configurationManager.GetOne(filter: x => x.Key == ConfigurationKey.AwsS3BucketName).Value;
                var _client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.USWest2);

                //Need to register the credentials, and should be working
                var fileTransferUtility = new TransferUtility(_client);
                var imageName = Guid.NewGuid().ToString();
                imageName = imageName + Path.GetExtension(filePath);
                await fileTransferUtility.UploadAsync(filePath, bucketName, imageName);
                return imageName;
            }
            catch (Exception up)
            {
                throw up;
            }

        }
        public string GetImageLink(string imageName)
        {
            var accessKey = _configurationManager.GetOne(filter: x => x.Key == ConfigurationKey.AwsS3AccessKey).Value;
            var secretKey = _configurationManager.GetOne(filter: x => x.Key == ConfigurationKey.AwsS3SecretAccessKey).Value;
            var bucketName = _configurationManager.GetOne(filter: x => x.Key == ConfigurationKey.AwsS3BucketName).Value;
            var _client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.USWest2);

            try
            {
                return _client.GetPreSignedURL(
                    new GetPreSignedUrlRequest()
                    {
                        BucketName = bucketName,
                        Key = imageName,
                        Expires = DateTime.UtcNow.AddDays(1)
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteFile(string fileName)
        {
            var accessKey = _configurationManager.GetOne(filter: x => x.Key == ConfigurationKey.AwsS3AccessKey).Value;
            var secretKey = _configurationManager.GetOne(filter: x => x.Key == ConfigurationKey.AwsS3SecretAccessKey).Value;
            var bucketName = _configurationManager.GetOne(filter: x => x.Key == ConfigurationKey.AwsS3BucketName).Value;
            var _client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.USWest2);

            try
            {
                var request = new DeleteObjectRequest()
                {
                    BucketName = bucketName,
                    Key = fileName
                };
                _client.DeleteObjectAsync(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
