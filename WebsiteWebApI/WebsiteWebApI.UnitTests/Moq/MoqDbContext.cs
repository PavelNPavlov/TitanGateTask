using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebsiteWebApI.Data;
using WebsiteWebApI.Data.Repository;
using WebsiteWebApI.Data.Repository.Contracts;
using WebsiteWebApI.DataModels;

namespace WebsiteWebApI.UnitTests.Moq
{
    public static class MoqDbContext
    {
        public static List<FileBlob> fileBlobs = new List<FileBlob>()
            {
                new FileBlob(){Id = Guid.NewGuid(), Data = Encoding.UTF8.GetBytes("Hello"), IsDeleted = true,  DateCreated = new DateTime()},
                new FileBlob(){Id = Guid.NewGuid(), Data = Encoding.UTF8.GetBytes("Hello"), IsDeleted = false, DateCreated = new DateTime()}

            };

        public static List<Url> urls = new List<Url>()
            {
                new Url(){Id = Guid.NewGuid(), Name = "Hello", IsDeleted = true,  DateCreated = new DateTime()},
                new Url(){Id = Guid.NewGuid(), Name = "Hello", IsDeleted = false, DateCreated = new DateTime()},
                new Url(){Id = Guid.NewGuid(), Name = "Hello", IsDeleted = false, DateCreated = new DateTime().AddDays(1)}

            };
        public static Mock<ApplicationDbContext> GetMockedContext()
        {
            
            var fileBlobList = fileBlobs.AsQueryable();
            var urlList = urls.AsQueryable();
           

            var mockSet = new Mock<DbSet<FileBlob>>();
            mockSet.As<IQueryable<FileBlob>>().Setup(m => m.Provider).Returns(fileBlobList.Provider);
            mockSet.Setup(m => m.Find(It.IsAny<object>())).Returns((object id) => fileBlobs.FirstOrDefault(x=> x.Id.ToString() == JsonConvert.DeserializeObject<object[]>(JsonConvert.SerializeObject(id))[0].ToString()));
            mockSet.As<IQueryable<FileBlob>>().Setup(m => m.Expression).Returns(fileBlobList.Expression);
            mockSet.As<IQueryable<FileBlob>>().Setup(m => m.ElementType).Returns(fileBlobList.ElementType);
            mockSet.As<IQueryable<FileBlob>>().Setup(m => m.GetEnumerator()).Returns(fileBlobList.GetEnumerator());

            var mockSet2 = new Mock<DbSet<Url>>();
            mockSet2.As<IQueryable<Url>>().Setup(m => m.Provider).Returns(urlList.Provider);
            //mockSet2.Setup(m => m.Find(It.IsAny<object>())).Returns((Guid id) => urls.FirstOrDefault(x => x.Id.ToString() == JsonConvert.DeserializeObject<object[]>(JsonConvert.SerializeObject(id))[0].ToString());
            mockSet2.As<IQueryable<Url>>().Setup(m => m.Expression).Returns(urlList.Expression);
            mockSet2.As<IQueryable<Url>>().Setup(m => m.ElementType).Returns(urlList.ElementType);
            mockSet2.As<IQueryable<Url>>().Setup(m => m.GetEnumerator()).Returns(urlList.GetEnumerator());


            var mockedContext = new Mock<ApplicationDbContext>();
            mockedContext.Setup(c => c.FileBlobs).Returns(mockSet.Object);
            mockedContext.Setup(c => c.Urls).Returns(mockSet2.Object);
            mockedContext.Setup(c => c.Set<FileBlob>()).Returns(mockSet.Object);
            mockedContext.Setup(c => c.Set<Url>()).Returns(mockSet2.Object);
            mockedContext.Setup(c => c.SetState(It.IsAny<object>(), It.IsAny<EntityState>()));
            return mockedContext;
        }

        public static IGenericRepository<ApplicationDbContext, FileBlob> GetFileBlobGenericRepo (ApplicationDbContext dbContext)
        {
            var mockedRepo = new GenericRepository<ApplicationDbContext, FileBlob>(dbContext);

            return mockedRepo;
        }
    }
}
