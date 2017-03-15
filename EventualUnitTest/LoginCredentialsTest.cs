using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventualUnitTest
{
    [TestClass]
    public class LoginCredentialsTest
    {
        [TestMethod]
        public Eventual.Model.LoginCredentials LoginCredentials()
        {
            Eventual.Model.LoginCredentials login = new Eventual.Model.LoginCredentials();
            login.UserEmail = "login@email.com";
            login.UserPassword = "loginpassword";
            return login;
        }
        [TestMethod]
        public void TestLogin()
        {
            Eventual.Model.LoginCredentials login = LoginCredentials();
            Assert.AreEqual(login.UserEmail, "login@email.com");
            Assert.AreEqual(login.UserPassword, "loginpassword");
        }
    }
}
