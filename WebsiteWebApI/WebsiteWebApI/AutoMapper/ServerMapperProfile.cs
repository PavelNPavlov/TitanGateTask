using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WebsiteWebApI.DataModels;
using WebsiteWebApI.DataModels.Identity;
using WebsiteWebApI.Infrastructure.DataServiceModels;
using WebsiteWebApI.Infrastructure.InputModels;
using WebsiteWebApI.Infrastructure.OutputModels;

namespace WebsiteWebApI.AutoMapper
{
    public class ServerMapperProfile : Profile
    {
        public ServerMapperProfile()
        {
            this.CreateMap<WebsiteCategory, CategoryDSModel>()
               .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));



            this.CreateMap<CategoryDSModel, SelectListItem>();

            this.CreateMap<SystemUser, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.UserName));

            this.CreateMap<CreateWebsiteIM, UrlCreateEditDSModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.URL))
                .ForMember(dest => dest.UrlPath, opt => opt.MapFrom(src => src.URL));

            this.CreateMap<UrlCreateEditDSModel, Url>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id));

            this.CreateMap<WebsiteEditCreateDSModel, WebsiteEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id));

            this.CreateMap<IFormFile, File>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
               .ForMember(dest => dest.FileExtension, opt => opt.MapFrom(src => src.ContentType));

            this.CreateMap<Url, UrlOM>();
            this.CreateMap<Url, Url>();

            this.CreateMap<CreateWebsiteIM, WebsiteEntity>();

            this.CreateMap<WebsiteEntity, EditWebsiteIM>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
              .ForMember(dest => dest.URL, opt => opt.MapFrom(src => src.Url.UrlPath))
              .ForMember(dest => dest.FileId, opt => opt.MapFrom(src => src.Files.FirstOrDefault(x=>x.FileType == "HomePageSnapShot").Id))
              .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.WebsiteCategoryId))
              .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.SystemUserId));

            this.CreateMap<WebsiteEntity, WebsiteEntity>();

            this.CreateMap<WebsiteEntity, WebsiteListOM>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
              .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url.UrlPath))
              .ForMember(dest => dest.SnapshotUrl, opt => opt.MapFrom(src => $"/Files/GetFile/{src.Files.FirstOrDefault(x => x.FileType == "HomePageSnapShot").Id}"))
              .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.WebsiteCategory.Name))
              .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.SystemUser.Email));

            this.CreateMap<File, File>();
            this.CreateMap<FileBlob, FileBlob>();

        }
    }
}
