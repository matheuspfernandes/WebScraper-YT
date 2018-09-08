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
    [TestClass]
    public class GooglePage : DriverFactory
    {
        [TestMethod]
        [TestCategory("Interface")]
        public void UIAcessarGoogle()
        {
            //IWebDriver driver = new ChromeDriver();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            ////ERRO AO INSTANCIAR O FIREFOX DRIVER
            //FirefoxOptions ffOp = new FirefoxOptions();
            //ffOp.AddArgument("https://www.google.com.br/");
            //ffOp.BrowserExecutableLocation = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";

            //IWebDriver driver = new FirefoxDriver(
            //    @"C:\Users\matheus.pires\Documents\Visual Studio 2015\Projects\SettingEnviroment\SettingEnviroment\bin\Debug",
            //    ffOp,
            //    TimeSpan.FromSeconds(30)
            //);

            //IWebDriver driver = new FirefoxDriver(new FirefoxBinary(), new FirefoxProfile(), TimeSpan.FromSeconds(180));


            try
            {            
                driver.Navigate().GoToUrl("https://www.google.com.br/");
                driver.Manage().Window.Size = new Size(1080, 960);
                driver.Manage().Window.Position = new Point(12, 12);
                
                driver.FindElement(By.Name("q")).SendKeys("Rumo Soluções");
                driver.FindElement(By.Name("q")).SendKeys(Keys.Enter);
                driver.FindElement(By.LinkText("Imagens")).Click();
                Util.TirarPrint(driver);
                driver.FindElement(By.LinkText("Todas")).Click();
                wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Rumo Soluções")));
                driver.FindElement(By.LinkText("Rumo Soluções")).Click();
                Assert.AreEqual("Rumo Soluções", driver.Title);
                Util.TirarPrint(driver);
                Console.WriteLine("Passed");
           }
           catch (Exception e)
           {                
                Util.TirarPrint(driver);

                throw e;
           }
           finally
           {
               driver.Quit();
           }

        }
        
    }
}
