using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System.Threading;
using OpenQA.Selenium.Chrome;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.UI;
using System.Drawing;
using SettingEnviroment;

namespace UI
{
    [TestClass()]
    public class GooglePage : DriverFactory
    {
        [TestMethod]
        [TestCategory("Interface")]
        public void UIAcessarGoogle()
        {
                
            try 
            {
                InicializaBrowser();
                driver.Navigate().GoToUrl("https://www.google.com.br/");
                
                driver.FindElement(By.Name("q")).SendKeys("Rumo Soluções");
                driver.FindElement(By.Name("q")).SendKeys(Keys.Enter);
                driver.FindElement(By.LinkText("Imagens")).Click();
                TirarPrint();

                driver.FindElement(By.LinkText("Todas")).Click();
                EsperaPorElementoVisivel(By.LinkText("Rumo Soluções"));
                driver.FindElement(By.LinkText("Rumo Soluções")).Click();
                Assert.AreEqual("Rumo Soluções", driver.Title);

                TirarPrint();
                Console.WriteLine("Passed");
           }
           catch (Exception e)
           {                
                TirarPrint();

                throw e;
           }
           finally
           {
               driver.Quit();
           }

        }
        
    }
}
