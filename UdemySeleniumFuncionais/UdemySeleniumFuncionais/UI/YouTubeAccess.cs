using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                var link = "https://www.youtube.com/watch?v=Ue77esm5Kqs&t=78s";

                InicializaBrowserAnonimo(link);
                Thread.Sleep(3000);

                var VideoPrincipal = IteracaoInicial();
                var listaTotal = new List<VideoYT>();

                foreach (var item in VideoPrincipal.Recomendacoes)
                {
                    AcessaLink(item.Link);
                    item.NomeVideo = ObterNome();
                    item.NomeCanal = ObterNomeCanal();
                    item.Categoria = ObterCategoria();
                    item.QuantViews = ObterQuantViews();
                    item.QuantLikes = ObterQuantLikes();
                    item.Recomendacoes = RetornaLinksRecomendados();
                    //item.Show();
                }

                Console.WriteLine("END\n\n");
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
