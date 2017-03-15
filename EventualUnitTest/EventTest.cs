using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventualUnitTest
{
    [TestClass]
    public class EventTest
    {
        [TestMethod]
        public Eventual.Model.Event CreateEvent()
        {
            Eventual.Model.Event event1 = new Eventual.Model.Event();
            event1.EventID = 17;
            event1.EventTitle = "event1";
            event1.EventPrice = 50;
            event1.EventDescription = "description";
            event1.LocationID = 1020;
            event1.EventImageURL = "url";
            return event1;
        }

        [TestMethod]
        public void TestEvent()
        {
            Eventual.Model.Event event2 = CreateEvent();
            Assert.AreEqual(event2.EventID, 17);
            Assert.AreEqual(event2.EventTitle, "event1");
            Assert.AreEqual(event2.EventPrice, 50);
            Assert.AreEqual(event2.EventDescription, "description");
            Assert.AreEqual(event2.LocationID, 1020);
            Assert.AreEqual(event2.EventImageURL, "url");
        }
    }
}
