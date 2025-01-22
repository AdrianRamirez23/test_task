using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Infraestructure.Data;

namespace ToDo.Test
{
    public static class DbSetExtensions
    {
        public static Mock<DbSet<T>> ToDbSetMock<T>(this IEnumerable<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            dbSetMock.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(sourceList.ToList().Add);

            return dbSetMock;
        }


        public static void ReturnsDbSet<T>(this Mock<AppDbContext> dbContextMock, DbSet<T> dbSet) where T : class
        {
            dbContextMock.Setup(db => db.Set<T>()).Returns(dbSet);
        }
    }
}
