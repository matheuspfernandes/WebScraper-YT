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
using System.Collections.Generic;

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
                var link = "https://www.youtube.com/watch?v=a3TpBg1T9_k";

                InicializaBrowserAnonimo(link);
                Thread.Sleep(3000);

                var listaRecomendacoes = PegaTodoMundo();
                var listaTotal = new List<RecomendacoesVideo>();

                foreach (var item in listaRecomendacoes)
                {
                    driver.Url = item.Link;
                    PegaTodoMundo();
                    item.Print();
                    Console.WriteLine("END\n\n");
                }
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
