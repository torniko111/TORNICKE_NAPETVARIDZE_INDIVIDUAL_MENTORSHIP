using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace NetPrimitives
{
    [TestClass]
    public class NETdatatypes
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
            Console.WriteLine("D2 = " + d1);
            double d2 = d1 * (double)1000000;
            decimal dec2 = dec1 * (decimal)1000000;
            Console.WriteLine("D3 = " + d2);

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

    }
}
