using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IS3Manager
    {
        string UploadFile(IFormFile formFile);
        string GetImageLink(string imageName);
        void DeleteFile(string fileName);
    }
}
