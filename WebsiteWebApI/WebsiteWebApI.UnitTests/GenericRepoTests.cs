using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebsiteWebApI.Data;
using WebsiteWebApI.Data.Repository;
using WebsiteWebApI.Data.Repository.Contracts;
using WebsiteWebApI.DataModels;
using WebsiteWebApI.UnitTests.Moq;

namespace WebsiteWebApI.UnitTests
{
    [TestFixture]
    public class GenericRepoTests
    {
        private IGenericRepository<ApplicationDbContext, FileBlob> repo;
        
        [SetUp]
        public void Setup()
        {
            var context = MoqDbContext.GetMockedContext();
            this.repo = new GenericRepository<ApplicationDbContext, FileBlob>(context.Object);
        }

        [Test]
        public void IfSoftDeletMemebersArNotIncluded()
        {
            var result = this.repo.All().ToList();

            Assert.IsTrue(result.Count == 1);
        }

        [Test]
        public void CheckIfDateTimeIsUpdatedOnCreate()
        {
            var item = new FileBlob { Id = Guid.NewGuid() };
            
            this.repo.Create(item);          

            Assert.IsTrue(item.DateCreated != new DateTime());
            Assert.IsTrue(item.DateModified != new DateTime());
        }




        [Test]
        public void CheckIfDateTimeIsUpdatedOnUpdate()
        {
            var createTime = DateTime.Now;
            var item = new FileBlob { Id = Guid.NewGuid(), DateCreated = createTime, DateModified = createTime };

            this.repo.Edit(item);

            Assert.IsTrue(item.DateModified != createTime);
        }
    }
}
