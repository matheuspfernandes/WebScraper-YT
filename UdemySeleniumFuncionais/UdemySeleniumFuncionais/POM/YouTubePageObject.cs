using Base;
using System.Collections.Generic;
using OpenQA.Selenium;
using System;

namespace UdemySeleniumFuncionais.POM
{
    public class YouTubePageObject : DriverFactory
    {
        //public VideoYT RV = new VideoYT(); 

        public string ObterNomeCanal()
        {
            //EsperaPorElementoVisivel(By.XPath("//*[@id='owner - name']/a"));
            return driver.FindElement(By.XPath("//*[@id='owner - name']/a")).Text;
        }

        public string ObterQuantLikes()
        {
            //EsperaPorElementoVisivel(By.XPath("//yt-formatted-string[contains(@aria-label,'gostaram')]"));
            return driver.FindElement(By.XPath("//yt-formatted-string[contains(@aria-label,'gostaram')]")).Text;
        } 

        public string ObterQuantViews()
        {
            //EsperaPorElementoVisivel(By.XPath("//*[@id='count']/yt-view-count-renderer/span[1]"));
            return driver.FindElement(By.XPath("//*[@id='count']/yt-view-count-renderer/span[1]")).Text;
        }

        public string ObterCategoria()
        {
            //EsperaPorElementoClicavel(By.XPath("//*[text()='Mostrar mais']/..//*[@role='button']"));
            driver.FindElement(By.XPath("//*[text()='Mostrar mais']/..//*[@role='button']")).Click();

            //EsperaPorElementoVisivel(By.XPath("//*[@id='collapsible']//*[text()='Categoria']/../..//yt-formatted-string/a"));
            return driver.FindElement(By.XPath("//*[@id='collapsible']//*[text()='Categoria']/../..//yt-formatted-string/a")).Text;
        }

        public string ObterNome()
        {
            //EsperaPorElementoVisivel(By.XPath("//*[@id='container']/h1/yt-formatted-string"));
            return driver.FindElement(By.XPath("//*[@id='container']/h1/yt-formatted-string")).Text;
        }

        public string ObterLink()
        {
            return driver.Url;
        }

        public List<VideoYT> RetornaLinksRecomendados()
        {
            By by = By.XPath("//*[@id='items' and @class='style-scope ytd-watch-next-secondary-results-renderer']//*[@class='style-scope ytd-watch-next-secondary-results-renderer']//*[@id='dismissable']/a");
            VideoYT videoYT;
            List<VideoYT> LinkRecomendados = new List<VideoYT>();

            EsperaPorElementosLocalizadosPor(by, 15);
            var ListaDeVideos = driver.FindElements(by);

            foreach (IWebElement video in ListaDeVideos)
            {
                videoYT = new VideoYT(video.GetAttribute("href"));
                LinkRecomendados.Add(videoYT);
            }

            return LinkRecomendados;
        }

        public VideoYT IteracaoInicial()
        {
            var video = new VideoYT(ObterLink());
            video.NomeVideo = ObterNome();
            video.NomeCanal = ObterNomeCanal();
            video.Categoria = ObterCategoria();
            video.QuantViews = ObterQuantViews();
            video.QuantLikes = ObterQuantLikes();
            video.Recomendacoes = RetornaLinksRecomendados();

            Console.WriteLine(video.Show());
            Console.WriteLine("\nPegou todo mundo");

            return video;
        }

        public VideoYT RealizaIteracoes(int QuantIteracoes)
        {
            VideoYT videoYT = new VideoYT();

            for (int i = 0; i < QuantIteracoes; i++)
            {

            }

            return videoYT;
        }

    }
}
