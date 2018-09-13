using Base;
using System.Collections.Generic;
using OpenQA.Selenium;
using System;

namespace UdemySeleniumFuncionais.POM
{
    public class YouTubePageObject : DriverFactory
    {
        public RecomendacoesVideo RV = new RecomendacoesVideo(); 

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

        public List<RecomendacoesVideo> RetornaLinksRecomendados()
        {
            RecomendacoesVideo RV;
            List<RecomendacoesVideo> LinkRecomendados = new List<RecomendacoesVideo>();
            var ListaDeVideos = driver.FindElements(By.XPath("//*[@id='items' and @class='style-scope ytd-watch-next-secondary-results-renderer']//*[@class='style-scope ytd-watch-next-secondary-results-renderer']//*[@id='dismissable']/a"));

            foreach (IWebElement video in ListaDeVideos)
            {
                RV = new RecomendacoesVideo(video.GetAttribute("href"));
                LinkRecomendados.Add(RV);
            }

            return LinkRecomendados;
        }

        public List<RecomendacoesVideo> PegaTodoMundo()
        {
            var video = new RecomendacoesVideo(ObterLink());
            video.Nome = ObterNome();
            video.Categoria = ObterCategoria();
            video.QuantViews = ObterQuantViews();
            video.Recomendacoes = RetornaLinksRecomendados();

            Console.WriteLine(video.Print());
            Console.WriteLine("\nPassed");

            return video.Recomendacoes;
        }


    }
}
