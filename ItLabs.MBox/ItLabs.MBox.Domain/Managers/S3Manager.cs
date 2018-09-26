using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Dtos;
using ItLabs.MBox.Contracts.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ItLabs.MBox.Domain.Managers
{
    public class S3Manager : IS3Manager
    {

        private readonly IConfigurationManager _configurationManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<S3Manager> _logger;
        public S3Manager(IConfigurationManager configurationManager,IServiceProvider serviceProvider, ILogger<S3Manager> logger)
        {
            _configurationManager = configurationManager;
            _serviceProvider = serviceProvider;
            _logger= logger;
        }
        public string UploadFile(IFormFile formFile)
        {
            try
            {
                var awsConfig = (AwsSettings)_serviceProvider.GetService(typeof(AwsSettings));
                var fileTransferUtility = new TransferUtility(new AmazonS3Client(awsConfig.AwsS3AccessKey, awsConfig.AwsS3SecretAccessKey, RegionEndpoint.USWest2));
                var imageName = Guid.NewGuid().ToString();
                imageName = imageName + Path.GetExtension(formFile.FileName);
                using (var stream = formFile.OpenReadStream())
                {
                    fileTransferUtility.UploadAsync(stream, awsConfig.AwsS3BucketName, imageName).Wait();
                }
                return imageName;
            }
            catch (Exception up)
            {
                _logger.LogInformation(up.Message);
                return null;
            }

        }
        public string GetImageLink(string imageName)
        {
            var awsConfig = (AwsSettings)_serviceProvider.GetService(typeof(AwsSettings));
            var _client = new AmazonS3Client(awsConfig.AwsS3AccessKey, awsConfig.AwsS3SecretAccessKey, RegionEndpoint.USWest2);
            try
            {
                return _client.GetPreSignedURL(
                    new GetPreSignedUrlRequest()
                    {
                        BucketName = awsConfig.AwsS3BucketName,
                        Key = imageName,
                        Expires = DateTime.UtcNow.AddDays(1)
                    });
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return "";
            }
        }
        public void DeleteFile(string fileName)
        {
            var awsConfig = (AwsSettings)_serviceProvider.GetService(typeof(AwsSettings));
            var _client = new AmazonS3Client(awsConfig.AwsS3AccessKey, awsConfig.AwsS3SecretAccessKey, RegionEndpoint.USWest2);

            try
            {
                var request = new DeleteObjectRequest()
                {
                    BucketName = awsConfig.AwsS3BucketName,
                    Key = fileName
                };
                _client.DeleteObjectAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
        }
    }
}
