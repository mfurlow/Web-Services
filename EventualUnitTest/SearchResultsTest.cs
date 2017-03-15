using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventualUnitTest
{
    [TestClass]
    public class SearchResultsTest
    {
        [TestMethod]
        public Eventual.Model.SearchResult CreateSearchResult()
        {
            Eventual.Model.SearchResult search = new Eventual.Model.SearchResult();
            search.EventID = 28;
            search.EventImageURL = "searchurl";
            search.EventPrice = 70;
            search.EventTitle = "searchtitle";
            search.LocationBuildingName = "building";
            search.LocationCity = "Herndon";
            search.LocationStreet1 = "123 more";
            search.StateAbbreviation = "VA";
            return search;
        }
        [TestMethod]
        public void TestSearchResult()
        {
            Eventual.Model.SearchResult search = CreateSearchResult();
            Assert.AreEqual(search.EventID,28);
            Assert.AreEqual(search.EventImageURL,"searchurl");
            Assert.AreEqual(search.EventPrice,70);
            Assert.AreEqual(search.EventTitle,"searchtitle");
            Assert.AreEqual(search.LocationBuildingName,"building");
            Assert.AreEqual(search.LocationCity,"Herndon");
            Assert.AreEqual(search.LocationStreet1,"123 more");
            Assert.AreEqual(search.StateAbbreviation,"VA");
        }
    }
}
