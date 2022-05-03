using BL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestingForecast
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()
        {
            //Arrange
            double C = -10000;
            C = UserService.GetWeatherApi("tbilisi").Result;
            //Act


            //Assert
            Assert.IsTrue(C != -10000);
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Arrange

            double C = UserService.GetWeatherApi("london").Result;
            //Act

            bool result = false;

            if (C > 16)
            {
                if (UserService.comment.Comment() == "its fresh")
                {
                    result = true;
                }
            }
            else if (C <= 16)
            {
                if (UserService.comment.Comment() == "its Warm")
                {
                    result = true;
                }
            }

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestMethod3()
        {
            //Arrange

            double C = UserService.GetWeatherApi("tbilisi").Result;
            //Act

            bool result = false;

            if (C > 16)
            {
                if (UserService.comment.Comment() == "its fresh")
                {
                    result = true;
                }
            }
            else if (C <= 16)
            {
                if (UserService.comment.Comment() == "its Warm")
                {
                    result = true;
                }
            }

            //Assert
            Assert.IsTrue(result);
        }
    }
}