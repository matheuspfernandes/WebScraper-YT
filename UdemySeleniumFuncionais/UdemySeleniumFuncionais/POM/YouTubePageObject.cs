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
            EsperaPorElementoVisivel(By.XPath("//*[@id='count']/yt-view-count-renderer/span[1]"));
            return driver.FindElement(By.XPath("//*[@id='count']/yt-view-count-renderer/span[1]")).Text;
        }

        public string ObterCategoria()
        {
            EsperaPorElementoClicavel(By.XPath("//*[text()='Mostrar mais']/..//*[@role='button']"));
            driver.FindElement(By.XPath("//*[text()='Mostrar mais']/..//*[@role='button']")).Click();

            EsperaPorElementoVisivel(By.XPath("//*[@id='collapsible']//*[text()='Categoria']/../..//yt-formatted-string/a"));
            return driver.FindElement(By.XPath("//*[@id='collapsible']//*[text()='Categoria']/../..//yt-formatted-string/a")).Text;
        }

        public string ObterNome()
        {
            EsperaPorElementoVisivel(By.XPath("//*[@id='container']/h1/yt-formatted-string"));
            return driver.FindElement(By.XPath("//*[@id='container']/h1/yt-formatted-string")).Text;
        }

        public string ObterLink()
        {
            return driver.Url;
        }

        public List<RecomendacoesVideo> RetornaLinksRecomendados()
        {
            By by = By.XPath("//*[@id='items' and @class='style-scope ytd-watch-next-secondary-results-renderer']//*[@class='style-scope ytd-watch-next-secondary-results-renderer']//*[@id='dismissable']/a");
            RecomendacoesVideo RV;
            List<RecomendacoesVideo> LinkRecomendados = new List<RecomendacoesVideo>();

            EsperaPorElementosLocalizadosPor(by, 15);
            var ListaDeVideos = driver.FindElements(by);

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
