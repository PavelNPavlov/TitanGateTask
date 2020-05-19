using AutoMapper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebsiteWebApI.AutoMapper;
using WebsiteWebApI.Data;
using WebsiteWebApI.Data.Contracts;
using WebsiteWebApI.Data.CrudAPIModels.Input;
using WebsiteWebApI.Data.Repository;
using WebsiteWebApI.Data.Repository.Contracts;
using WebsiteWebApI.DataModels;
using WebsiteWebApI.UnitTests.Moq;

namespace WebsiteWebApI.UnitTests
{
    [TestFixture]
    public class DbTests
    {
        
        private IBaseDataService<FileBlob, FileBlob, FileBlob, FileBlob, ApplicationDbContext> dataSerivce;
        private IBaseDataService<Url, Url, Url, Url, ApplicationDbContext> dataSerivceUrl;

        [SetUp]
        public void Setup()
        {
            var context = MoqDbContext.GetMockedContext();
            var repo = new GenericRepository<ApplicationDbContext, FileBlob>(context.Object);
            var repoUrl = new GenericRepository<ApplicationDbContext, Url> (context.Object);
            var mapper = new Mapper(new MapperConfiguration(cfg =>
            {
               cfg.AddProfile(new ServerMapperProfile());

            }));


            this.dataSerivce = new BaseDataService<FileBlob, FileBlob, FileBlob, FileBlob, ApplicationDbContext>(repo, mapper, context.Object);

            this.dataSerivceUrl = new BaseDataService<Url, Url, Url, Url, ApplicationDbContext>(repoUrl, mapper, context.Object);
        }

        [Test]
        public void CheckIFSoftDeleteItemsAreNotReturnedById()
        {
            var result = this.dataSerivce.GetItemById(new GetInputModel() { Id = MoqDbContext.fileBlobs[0].Id });

            Assert.IsTrue(result == null);
        }

        [Test]
        public void ChekIfGetByIdReturnsObject()
        {
            var result = this.dataSerivce.GetItemById(new GetInputModel() { Id = MoqDbContext.fileBlobs[1].Id });

            Assert.IsTrue(result != null);
        }

        [Test]
        public void TesFiltering()
        {
            var filters = new List<FilterModel>
            {
                new FilterModel
                {
                    Prop = "Id",
                    Comparison = "equals",
                    Value = MoqDbContext.fileBlobs[1].Id
                }
            };

            var result = this.dataSerivce.GetItems(filters, null,1,10);

            Assert.IsTrue(result.Count == 1);
        }

        [Test]
        public void SkipSoftDelete()
        {
            var filters = new List<FilterModel>
            {
                new FilterModel
                {
                    Prop = "Name",
                    Comparison = "equals",
                    Value = "Hello"
                }
            };

            var result = this.dataSerivceUrl.GetItems(filters, null, 1, 10);

            Assert.IsTrue(result.Count == 2);
        }


        [Test]
        public void SortingDescProperlyOrdersItemsBasedOnDateCreated()
        {
            var filters = new List<FilterModel>
            {
                new FilterModel
                {
                    Prop = "Name",
                    Comparison = "equals",
                    Value = "Hello"
                }
            };

            var sorting = new SortModel
            {
                Prop = "DateCreated",
                Direction = "desc"
            };

            var result = this.dataSerivceUrl.GetItems(filters, sorting, 1, 10);

            Assert.IsTrue(result[0].DateCreated > result[1].DateCreated);
        }


        [Test]
        public void PagingProperlyReturnsOneItemForPagesize1()
        {
            var filters = new List<FilterModel>
            {
                new FilterModel
                {
                    Prop = "Name",
                    Comparison = "equals",
                    Value = "Hello"
                }
            };

            var sorting = new SortModel
            {
                Prop = "DateCreated",
                Direction = "desc"
            };

            var result = this.dataSerivceUrl.GetItems(filters, sorting, 1, 1);

            Assert.IsTrue(result.Count == 1);
        }

        [Test]
        public void PagingProperlyReturnsSecondPage()
        {
            var filters = new List<FilterModel>
            {
                new FilterModel
                {
                    Prop = "Name",
                    Comparison = "equals",
                    Value = "Hello"
                }
            };

            var sorting = new SortModel
            {
                Prop = "DateCreated",
                Direction = "desc"
            };

            var result = this.dataSerivceUrl.GetItems(filters, sorting, 2, 1);
            var wanteObject = MoqDbContext.urls.Where(x => x.Name == "Hello" && x.IsDeleted == false).OrderByDescending(x => x.DateCreated).ToList();
            Assert.IsTrue(result[0].Id == wanteObject[1].Id);
        }
    }
}
