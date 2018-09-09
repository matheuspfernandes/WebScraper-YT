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
using Base;
using OpenQA.Selenium.Remote;
using UdemySeleniumFuncionais.POM;

namespace UdemySeleniumFuncionais.UI
{
    [TestClass()]
    public class YouTubeAccess : YouTubePageObject
    {

        [TestMethod()]
        [TestCategory("YouTubePage")]
        public void UIAcessarYouTube()
        {
            try
            {
                InicializaBrowserAnonimo("https://www.youtube.com/watch?v=-UY9jUwVTHg");
                Thread.Sleep(10000);

                Console.WriteLine(ObterNome());
                Console.WriteLine(ObterLink());
                //Console.WriteLine(ObterCategoria());
                Console.WriteLine(ObterQuantViews());

                foreach(string url in RetornaLinksRecomendados())
                {
                    Console.WriteLine(url);
                }  

                Console.WriteLine("\nPassed");
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
