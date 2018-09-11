using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemySeleniumFuncionais
{
    public class RecomendacoesVideo
    {
        public string Nome { get; set; }
        public string Link { get; set; }
        public string Categoria { get; set; }
        public string QuantViews { get; set; }
        List<RecomendacoesVideo> Recomendacoes { get; set; }

        public RecomendacoesVideo()
        {
            Recomendacoes = new List<RecomendacoesVideo>();
        }

        public RecomendacoesVideo(string link)
        {
            Link = link;
            Recomendacoes = new List<RecomendacoesVideo>();
        }

        public RecomendacoesVideo(string nome, string link, string categoria, string quantViews, List<RecomendacoesVideo> recomendacoes)
        {
            Nome = nome;
            Link = link;
            Categoria = categoria;
            QuantViews = quantViews;
            Recomendacoes = recomendacoes;
        }

        public RecomendacoesVideo(string nome, string link, string categoria, string quantViews)
        {
            Nome = nome;
            Link = link;
            Categoria = categoria;
            QuantViews = quantViews;
            Recomendacoes = new List<RecomendacoesVideo>();
        }

    }
}
