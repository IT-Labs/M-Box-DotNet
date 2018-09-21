using Amazon.S3;
using Amazon.S3.Transfer;
using ItLabs.MBox.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ItLabs.MBox.Domain.Managers
{
    public class S3Manager:IS3Manager
    {
        private readonly IAmazonS3 _client;
        public S3Manager(IAmazonS3 client)
        {
            _client = client;
        }
        public async Task UploadFileAsync(string bucketName)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_client);
                var ImageName = Guid.NewGuid().ToString();
                //await fileTransferUtility.UploadAsync(filePath, bucketName);
            }
            catch (Exception up)
            {
                throw up;
            }
        }
    }
}
