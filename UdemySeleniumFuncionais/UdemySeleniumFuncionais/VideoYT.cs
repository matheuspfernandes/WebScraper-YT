using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemySeleniumFuncionais
{
    public class VideoYT
    {
        public string NomeVideo { get; set; }
        public string Link { get; set; }
        public string Categoria { get; set; }
        public string QuantLikes { get; set; }
        public string NomeCanal { get; set; }
        public string QuantViews { get; set; }
        public List<VideoYT> Recomendacoes { get; set; }

        public VideoYT()
        {
            Recomendacoes = new List<VideoYT>();
        }

        public VideoYT(string link)
        {
            Link = link;
            Recomendacoes = new List<VideoYT>();
        }

        public string Show()
        {
            return  NomeVideo + "\n" +
                    NomeCanal + "\n" +
                    Link + "\n" +
                    Categoria + "\n" +
                    QuantViews + "\n" +
                    QuantLikes + "\n" +
                    ReturnLinks();
        }

        public string ReturnLinks()
        {
            StringBuilder sb = new StringBuilder();

            if (Recomendacoes != null)
            {
                foreach (VideoYT s in Recomendacoes)
                {
                    sb.Append(s.Link + "\n");
                }
            }

            return sb.ToString();
        }

    }
}
