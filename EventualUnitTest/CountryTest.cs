using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventualUnitTest
{
    [TestClass]
    public class CountryTest
    {
        [TestMethod]
        public Eventual.Model.Country CreateCountry()
        {
            Eventual.Model.Country country = new Eventual.Model.Country();
            country.CountryID = 35;
            country.CountryAbbreviation = "SM";
            country.CountryLongName = "Sumike";
            return country;
        }

        [TestMethod]
        public void TestCountry()
        {
            Eventual.Model.Country country = CreateCountry();
            Assert.AreEqual(country.CountryID,35);
            Assert.AreEqual(country.CountryAbbreviation, "SM");
            Assert.AreEqual(country.CountryLongName, "Sumike");

        }
    }
}
