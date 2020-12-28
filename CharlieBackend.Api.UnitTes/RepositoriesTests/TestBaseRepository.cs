using CharlieBackend.Core.Entities;
using CharlieBackend.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Api.UnitTest.RepositoriesTests
{
    class TestBaseRepository<T> where T : BaseEntity
    {
        private readonly Mock<DbSet<T>> _entitiesMock;
        private readonly Mock<ApplicationContext> _applicationContextMock;

        public TestBaseRepository()
        {
            _applicationContextMock = new Mock<ApplicationContext>();
            _entitiesMock = new Mock<DbSet<T>>();
        }

    }
}
