using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventualUnitTest
{
    [TestClass]
    public class LocationTest
    {
        [TestMethod]
        public Eventual.Model.Location CreateLocation()
        {
            Eventual.Model.Location location = new Eventual.Model.Location();
            Eventual.Model.Country count = new Eventual.Model.Country();           
            Eventual.Model.State state = new Eventual.Model.State();

            location.LocationID = 11;
            location.Country = count;
            location.CountryAbbreviation = "USA";
            location.CountryID = 1;
            location.LocationBuildingName = "building";
            location.LocationCity = "Nashville";
            location.LocationStreet1 = "123 street";
            location.LocationZipcode = "37209";
            location.State = state;
            location.StateAbbreviation = "TN";
            location.StateID = 2;
            return location;
        }

        [TestMethod]
        public void TestLocation()
        {
            Eventual.Model.Location location = CreateLocation();
            Eventual.Model.Country count = new Eventual.Model.Country();
            Eventual.Model.State state = new Eventual.Model.State();

            Assert.AreEqual(location.LocationID,11);
            Assert.IsNotNull(location.Country);
            Assert.AreEqual(location.CountryAbbreviation, "USA");
            Assert.AreEqual(location.CountryID,1);
            Assert.AreEqual(location.LocationBuildingName, "building");
            Assert.AreEqual(location.LocationCity, "Nashville");
            Assert.IsNotNull(location.State);
            Assert.AreEqual(location.LocationStreet1, "123 street");
            Assert.AreEqual(location.LocationZipcode, "37209");
            Assert.AreEqual(location.StateAbbreviation, "TN");
            Assert.AreEqual(location.StateID,2);
        }
    }
}
