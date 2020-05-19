using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteWebApI.BLServices.Contracts;
using WebsiteWebApI.BLServices.FileProviders.Base;
using WebsiteWebApI.Data;
using WebsiteWebApI.Data.Contracts;
using WebsiteWebApI.Data.CrudAPIModels.Input;
using WebsiteWebApI.Data.Repository.Contracts;
using WebsiteWebApI.DataModels;

namespace WebsiteWebApI.BLServices.FileProviders
{
    public class FilesBLService : IFileBlService
    {
        private readonly IBaseDataService<DataModels.File, DataModels.File, DataModels.File, DataModels.File, ApplicationDbContext> filesDataService;
        private readonly IGenericRepository<ApplicationDbContext,FileProvider> providerRepository;
        private readonly IServiceProvider serviceProvider;
        private readonly IMapper mapper;
        private static string DefaultFileProviderName = typeof(SqlBlobFileProvider).FullName;

        public FilesBLService(
            IBaseDataService<DataModels.File, DataModels.File, DataModels.File, DataModels.File, ApplicationDbContext> filesDataService,
            IGenericRepository<ApplicationDbContext, FileProvider> providerRepository,
            IServiceProvider serviceProvider,
            IMapper mapper)
        {
            this.filesDataService = filesDataService;
            this.providerRepository = providerRepository;
            this.serviceProvider = serviceProvider;
            this.mapper = mapper;
        }

        public DataModels.File UploadFile(IFormFile formFile, string fileType, Guid siteId, string fileProvider = "")
        {
            var file = this.mapper.Map<DataModels.File>(formFile);
            file.FileType = fileType;
            file.WebsiteId = siteId;

            var providerType = fileProvider;

            if (string.IsNullOrEmpty(providerType))
            {
                var providerData = this.providerRepository.All().FirstOrDefault();
                file.FileProviderId = providerData.Id;
                providerType = providerData.ImplementationServiceName;
            }

            var provider = this.serviceProvider.GetService(Type.GetType(providerType)) as IBaseFileProvider;

          

            var fileResult = this.filesDataService.CreateItem(file);


            using (var stream = formFile.OpenReadStream())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    var bytes = ms.ToArray();
                    provider.UploadFileData(file.Id, bytes);
                }
            }

            return file;               
        }

        public (DataModels.File File, byte[] Data) GetFile(Guid fileId)
        {
            var file = this.filesDataService.GetItemById(new GetInputModel() { Id = fileId });

            var provider = this.serviceProvider.GetService(Type.GetType(file.FileProvider.ImplementationServiceName)) as IBaseFileProvider;

            var data = provider.GetFileDataById(file.Id); 

            return (file, data);
        }

        public async Task DeleteFile(Guid fileId)
        {
            this.filesDataService.DeleteItem(new DeleteInputModel { Id = fileId });
        }
    }
}
