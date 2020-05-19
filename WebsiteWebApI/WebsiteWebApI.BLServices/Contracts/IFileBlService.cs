using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteWebApI.BLServices.Contracts
{
    public interface IFileBlService
    {
        DataModels.File UploadFile(IFormFile formFile, string fileType, Guid siteId, string fileProvider = "");

        (DataModels.File File, byte[] Data) GetFile(Guid fileId);

        Task DeleteFile(Guid fileId);
    }
}
