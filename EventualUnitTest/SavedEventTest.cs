using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventualUnitTest
{
    [TestClass]
    public class SavedEventTest
    {
        [TestMethod]
        public Eventual.Model.SavedEvent CreateSavedEvent()
        {
            Eventual.Model.SavedEvent sevent = new Eventual.Model.SavedEvent();
            Eventual.Model.Event n = new Eventual.Model.Event();
            Eventual.Model.User u = new Eventual.Model.User();
            sevent.Event = n;
            sevent.EventID = 1;
            sevent.User = u;
            sevent.UserID = 4;
            return sevent;
        }

        [TestMethod]
        public void TestSavedEvent()
        {
            Eventual.Model.SavedEvent sevent = CreateSavedEvent();
            Eventual.Model.Event n = new Eventual.Model.Event();
            Eventual.Model.User u = new Eventual.Model.User();

            Assert.AreEqual(sevent.EventID,1);
            Assert.AreEqual(sevent.UserID,4);
            Assert.IsNotNull(sevent.User);
            Assert.IsNotNull(sevent.Event);
        }
    }
}
