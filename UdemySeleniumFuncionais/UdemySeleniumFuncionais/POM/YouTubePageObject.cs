using Base;
using System.Threading;
using OpenQA.Selenium.Interactions;
using System.IO;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;
using OpenQA.Selenium;
using System;

namespace UdemySeleniumFuncionais.POM
{
    public class YouTubePageObject : DriverFactory
    {
        public string UrlBase = "https://www.youtube.com";

        public string ObterQuantViews()
        {
            return driver.FindElement(By.XPath("//*[@id='count']/yt-view-count-renderer/span[1]")).Text;
        }

        public string ObterCategoria()
        {
            driver.FindElement(By.XPath("//*[text()='Mostrar mais']/..//*[@role='button']")).Click();
            return driver.FindElement(By.XPath("//*[@id='collapsible']//*[text()='Categoria']/../..//yt-formatted-string/a")).Text;
        }

        public string ObterNome()
        {
            return driver.FindElement(By.XPath("//*[@id='container']/h1/yt-formatted-string")).Text;
        }

        public string ObterLink()
        {
            return driver.Url;
        }

        public List<string> RetornaLinksRecomendados()
        {
            List<string> LinkRecomendados = new List<string>();
            var ListaDeVideos = driver.FindElements(By.XPath("//*[@id='items' and @class='style-scope ytd-watch-next-secondary-results-renderer']//*[@class='style-scope ytd-watch-next-secondary-results-renderer']//*[@id='dismissable']/a"));

            foreach (IWebElement video in ListaDeVideos)
            {
                LinkRecomendados.Add(video.GetAttribute("href"));
            }

            return LinkRecomendados;
        }


    }
}
