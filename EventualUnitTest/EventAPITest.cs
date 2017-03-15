using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventualUnitTest
{
    [TestClass]
    public class EventAPITest
    {
        [TestMethod]
        public Eventual.Model.EventAPI CreateEventAPI()
        {
            Eventual.Model.EventAPI eventapi = new Eventual.Model.EventAPI();
            eventapi.EventID = 2;
            eventapi.EventTitle = "eventapi";
            eventapi.EventImageURL = "eventurl";
            eventapi.EventPrice = 60;
            eventapi.LocationCity = "Detroit";
            eventapi.LocationStreet1 = "123 westbrook";
            eventapi.StateAbbreviation = "MI";
            eventapi.UserID = 1020;
            return eventapi;
        }

        [TestMethod]
        public void TestEventapi()
        {
            Eventual.Model.EventAPI eventapi = CreateEventAPI();
            Assert.AreEqual(eventapi.EventID,2);
            Assert.AreEqual(eventapi.EventImageURL,"eventurl");
            Assert.AreEqual(eventapi.EventTitle,"eventapi");
            Assert.AreEqual(eventapi.EventPrice,60);
            Assert.AreEqual(eventapi.LocationCity,"Detroit");
            Assert.AreEqual(eventapi.LocationStreet1,"123 westbrook");
            Assert.AreEqual(eventapi.StateAbbreviation,"MI");
            Assert.AreEqual(eventapi.UserID,1020);

        }
    }
}
