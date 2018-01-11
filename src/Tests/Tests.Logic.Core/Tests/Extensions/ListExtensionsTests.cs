namespace deveer.DoctorFlox.Tests.Logic.Core.Tests.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using devdeer.DoctorFlox.Extensions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains unit tests for the type <see cref="ListExtensions" />.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ListExtensionsTests
    {
        #region methods
        
        /// <summary>
        /// Tests if <see cref="ListExtensions.Merge{T}" /> resolves error messages correctly
        /// from resources.
        /// </summary>
        [TestMethod]
        public void ListExtensionsMergeTest()
        {
            // arrange
            var firstList = new List<string>();
            var secondList = new List<string>();
            var expectedResultList = new List<string>();
            for (var i = 0; i < 100; i++)
            {
                var firstListItem = Guid.NewGuid().ToString("N");
                var secondListItem = Guid.NewGuid().ToString("N");
                firstList.Add(firstListItem);
                secondList.Add(secondListItem);
                expectedResultList.Add(firstListItem);
                expectedResultList.Add(secondListItem);
            }
            var listsToMerge = new List<IEnumerable<string>>
            {
                firstList,
                secondList
            };
            // act 
            var resultList = listsToMerge.Merge();
            // assert
            Assert.AreEqual(expectedResultList.Count, resultList.Count, "Expected amount of elements in result list is not present.");
            foreach (var item in expectedResultList)
            {
                Assert.IsTrue(resultList.Contains(item), "Expected element of result is not present there.");
            }
        }

        #endregion
    }
}