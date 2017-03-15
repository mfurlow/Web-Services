using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventualUnitTest
{
    [TestClass]
    public class EventTypeTest
    {
        [TestMethod]
        public Eventual.Model.EventType CreateEventType()
        {
            Eventual.Model.EventType type = new Eventual.Model.EventType();
            type.EventTypeID = 3;
            type.EventTypeName = "Concert";
            return type;
        }

        [TestMethod]
        public void TestEventType()
        {
            Eventual.Model.EventType type = CreateEventType();
            Assert.AreEqual(type.EventTypeID, 3);
            Assert.AreEqual(type.EventTypeName, "Concert");
        }
    }
}
