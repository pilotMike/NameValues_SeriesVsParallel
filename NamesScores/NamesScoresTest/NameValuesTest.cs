﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NamesScores;

namespace NamesScoresTest
{
    [TestClass]
    public class NameValuesTest
    {
        [TestMethod]
        public void calculates_name_value_correctly()
        {
            string name = "COLIN";
            int value = 53;

            int result = Program.GetNameNumberValue(name);
            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void calculates_list_correctly()
        {
            //var names = new List<string>{"COLIN", "COLIN"};
            //var expected = 53 + 53*2;
            //var result = Program.GetSumOfNameValuesInSeries(names); 
            //Assert.AreEqual(expected, result);
        }
    }
}