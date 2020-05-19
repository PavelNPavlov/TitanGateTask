using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WebsiteWebApI.BLServices.FileProviders.Base;
using WebsiteWebApI.Data;
using WebsiteWebApI.Data.Contracts;
using WebsiteWebApI.Data.Repository.Contracts;
using WebsiteWebApI.DataModels;

namespace WebsiteWebApI.BLServices.FileProviders
{
    public class SqlBlobFileProvider : IBaseFileProvider
    {
        private readonly IBaseDataService<FileBlob, FileBlob, FileBlob, FileBlob, ApplicationDbContext> blobDataService;
        private readonly IGenericRepository<ApplicationDbContext, FileBlob> blobRepo;

        public SqlBlobFileProvider(IBaseDataService<FileBlob, FileBlob, FileBlob, FileBlob, ApplicationDbContext> blobDataService,
            IGenericRepository<ApplicationDbContext, FileBlob> blobRepo)
        {
            this.blobDataService = blobDataService;
            this.blobRepo = blobRepo;
        }
        public bool DeleteFileData(Guid id)
        {
            return false;
        }

        public byte[] GetFileDataById(Guid Id)
        {
            var blob = this.blobRepo.All().Where(x => x.FileId == Id).FirstOrDefault();

            return blob.Data;
        }

        public bool UploadFileData(Guid id, byte[] data)
        {
            var blob = new FileBlob()
            {
                Data = data,
                FileId = id
            };

            this.blobDataService.CreateItemWithNoReturn(blob);
            this.blobDataService.SaveChanges();

            return true; 
        }
    }
}
