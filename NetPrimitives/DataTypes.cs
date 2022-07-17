using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NetPrimitives
{
    [TestClass]
    public class DataTypes : DataTypesBase
    {

        //Task N1 working With primitives, 
        [TestMethod]
        public void Precision_Loss_Double_Decimal()
        {
            //Arrange
            double d = 1004233 / (double)1000000;
            decimal dec = 1004233 / (decimal)1000000;

            //Act
            double d1 = d - Math.Truncate(d);
            decimal dec1 = dec - Math.Truncate(dec);  
            double d2 = d1 * (double)1000000;
            decimal dec2 = dec1 * (decimal)1000000;

            //Assert
            Assert.AreNotEqual(0.004233M, d1);
            Assert.AreNotEqual(4233M, d2);
            Assert.AreEqual(0.004233M, dec1);
            Assert.AreEqual(4233M, dec2);
        }

        //Task N2 working With strings 
        [TestMethod]
        public void String_Vs_Stringbuilder_Immutability()
        {
            //Arrange
            string s = "exadel";           
            StringBuilder sb = new StringBuilder();

            //Act
            s.Insert(0, "exadel georgia");
            sb.Insert(0, "exadel georgia");
            
            //Assert
            Assert.AreEqual(s, "exadel");
            Assert.AreEqual(sb.ToString(), "exadel georgia");
        }

        //Task N3 Algorithms, Palindrome checker
        [TestMethod]
        public void Is_Palindrome()
        {
            //Arrange
            bool palindrome1; 
            bool palindrome2; 
            bool palindrome3; 
            bool palindrome4; 
            bool palindrome5;
            bool palindrome6;

            //Act
            palindrome1 = IsPalindrome("madam");
            palindrome2 = IsPalindrome("abrakadabra");
            palindrome3 = IsPalindrome("issi");
            palindrome4 = IsPalindrome("kayak");
            palindrome5 = IsPalindrome("peep");
            palindrome6 = IsPalindrome("deed");

            //Assert
            Assert.IsTrue(palindrome1);
            Assert.IsFalse(palindrome2);
            Assert.IsTrue(palindrome3);
            Assert.IsTrue(palindrome4);
            Assert.IsTrue(palindrome5);
            Assert.IsTrue(palindrome6);
        }

        //Task N4 Collections, Array.
        [TestMethod]
        public void Arrays()
        {
            //Arrange
            int[] arrayZero = new int[10];
            int?[] arrayNull = new int?[10];

            //Act
            int? IsZero = arrayZero[0];
            int? IsNull = arrayNull[0];

            //Assert
            Assert.AreEqual(0, IsZero);
            Assert.AreEqual(null, IsNull);
        }

        //Task N5 Generic Collection, List.
        [TestMethod]
        public void DefferedWorkExecute()
        {
            //Arrange
            List<int> list = new List<int> { 3, 2, 3 };

            IEnumerable<int> data = list.Where(x => x > 1);

            IEnumerable<int> unique = data.Distinct();

            IEnumerable<int> dataList = data.ToList();

            //Act
            list.Add(5);

            //Assert
            Assert.IsTrue(data.SequenceEqual(new List<int>() { 3, 2, 3, 5 }));
            Assert.IsTrue(unique.SequenceEqual(new List<int>() { 3, 2, 5 }));
            Assert.IsTrue(dataList.SequenceEqual(new List<int>() { 3, 2, 3 }));
        }
    }
}
