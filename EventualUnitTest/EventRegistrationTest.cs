using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventualUnitTest
{
    [TestClass]
    public class EventRegistrationTest
    {
        [TestMethod]
        public Eventual.Model.EventRegistration RegisterEvent()
        {
            Eventual.Model.EventRegistration regevent = new Eventual.Model.EventRegistration();
            Eventual.Model.User user = new Eventual.Model.User();
            Eventual.Model.Event myevent = new Eventual.Model.Event();
            regevent.EventID = 2;
            regevent.User = user;
            regevent.UserID = 20;
            regevent.Event = myevent;
          
            return regevent;
        }


        [TestMethod]
        public void TestEventRegistration()
        {
            Eventual.Model.EventRegistration revent = RegisterEvent();
            Eventual.Model.User user = new Eventual.Model.User();
            Eventual.Model.Event myevent = new Eventual.Model.Event();

            Assert.AreEqual(revent.EventID, 2);
            Assert.AreEqual(revent.UserID, 20);
            Assert.AreNotEqual(revent.User, user);
            Assert.AreNotEqual(revent.Event, myevent);
        }
    }
}
