using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventualUnitTest
{
    /// <summary>
    /// Summary description for StateTest
    /// </summary>
    [TestClass]
    public class StateTest
    {
        [TestMethod]
        public Eventual.Model.State CreateState()
        {
            Eventual.Model.State state = new Eventual.Model.State();
            state.StateID = 102;
            state.StateAbbreviation = "UL";
            state.StateLongName = "Ultra State";
            return state;
        }   

        [TestMethod]
        public void TestState()
        {
            Eventual.Model.State state = CreateState();
            Assert.AreEqual(state.StateID,102);
            Assert.AreEqual(state.StateAbbreviation,"UL");
            Assert.AreEqual(state.StateLongName,"Ultra State");
        }
    }
}
