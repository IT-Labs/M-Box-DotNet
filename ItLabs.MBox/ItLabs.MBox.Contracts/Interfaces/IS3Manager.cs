using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IS3Manager
    {
        Task<string> UploadFileAsync(string filePath);
        string GetImageLink(string imageName);
        void DeleteFile(string fileName);
    }
}
