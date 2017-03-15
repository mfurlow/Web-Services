using Microsoft.VisualStudio.TestTools.UnitTesting;
using Eventual.Model;


namespace EventualUnitTest
{
    [TestClass]
    public class UserTest
    {
        //staging
        [TestMethod]
        public Eventual.Model.User CreateUser()
        {
            Eventual.Model.User user1 = new Eventual.Model.User();
            user1.UserID = 500000017;
            user1.UserEmail = "user1@user.com";
            user1.UserFirstName = "userfirst";
            user1.UserLastName = "userlast";
            user1.UserPhoneNumber = "1234567891";
            user1.UserRoleID = 1;
            return user1;
        }

        [TestMethod]
        public void TestUser()
        {
            Eventual.Model.User user2 = CreateUser();
            Assert.AreEqual(user2.UserID, 500000017);
            Assert.AreEqual(user2.UserEmail,"user1@user.com");
            Assert.AreEqual(user2.UserFirstName,"userfirst");
            Assert.AreEqual(user2.UserLastName,"userlast");
            Assert.AreEqual(user2.UserRoleID, 1);
        }
    }
}
