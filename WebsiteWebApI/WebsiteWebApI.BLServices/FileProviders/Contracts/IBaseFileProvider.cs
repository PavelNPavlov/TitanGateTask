using System;
using System.Collections.Generic;
using System.Text;
using WebsiteWebApI.DataModels;

namespace WebsiteWebApI.BLServices.FileProviders.Base
{
    public interface IBaseFileProvider
    {
        byte[] GetFileDataById(Guid Id);

        bool UploadFileData(Guid id, byte[] data);

        bool DeleteFileData(Guid id);
    }
}
