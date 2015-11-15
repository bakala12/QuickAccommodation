using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;
using Moq;

namespace QuickAccommodationTests
{
    /// <summary>
    /// A class used for mocking a table from database.
    /// </summary>
    /// <typeparam name="T">Generic parameter must be a type derived from Entity class.</typeparam>
    public class MockDbSet<T> where T : Entity
    {
        /// <summary>
        /// Initializes a new instance of MockDbSet class with the specified start data collection.
        /// </summary>
        /// <param name="data">A data used to mock the DbSet.</param>
        public MockDbSet(IEnumerable<T> data)
        {
            Initialize(data);
        }
            
        /// <summary>
        /// Gets the mocked DbSet.
        /// </summary>
        public Mock<IDbSet<T>> MockContext { get; private set; }

        /// <summary>
        /// Make some sufficient initialization and create a mocked DbSet.
        /// </summary>
        /// <param name="data">A collection of entities that should be putted into mocked DbSet.</param>
        private void Initialize(IEnumerable<T> data)
        {
            Mock<IDbSet<T>> mock = new Mock<IDbSet<T>>();
            var enumerable = data as IList<T> ?? data.ToList();
            mock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(enumerable.AsQueryable().Provider);
            mock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(enumerable.AsQueryable().Expression);
            mock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(enumerable.AsQueryable().ElementType);
            mock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(enumerable.AsQueryable().GetEnumerator());
            mock.Setup(m => m.Add(It.IsAny<T>())).Returns((T t) => t).Callback((T t) => enumerable.Add(t));
            mock.Setup(m => m.Remove(It.IsAny<T>())).Returns((T t) => t).Callback((T t) => enumerable.Remove(t));
            MockContext = mock;
        }
    }
}
