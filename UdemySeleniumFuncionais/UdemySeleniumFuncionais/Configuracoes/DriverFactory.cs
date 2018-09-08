using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using OpenQA.Selenium.Interactions;
using System.IO;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Chrome;

namespace SettingEnviroment
{
    public abstract class DriverFactory
    {
        public string local = AppDomain.CurrentDomain.BaseDirectory;
        protected WebDriverWait wait;
        private Actions action;
        protected IWebDriver driver = new ChromeDriver();
        public SqlConnectionStringBuilder SqlSB = new SqlConnectionStringBuilder();
        

        #region [Métodos para colocar no AuxiliarSQL (Metodos novos)]

        public void RunSQLScript(string script)
        {
            using (SqlConnection SqlC = new SqlConnection(SqlSB.ConnectionString))
            {
                SqlC.Open();

                using (SqlCommand dbCommand = new SqlCommand(script, SqlC))
                {
                    dbCommand.CommandText = script;
                    dbCommand.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Rodou script SQL");
        }

        public List<string> RetornaResultadoQueryDeUmaLinha(string ConsultaASerExecutada)
        {
            List<string> QueryResults = new List<string>();
            int NumberOfColumns;

            using (SqlConnection SqlC = new SqlConnection(SqlSB.ConnectionString))
            {
                SqlC.Open();

                using (SqlCommand dbCommand = new SqlCommand(ConsultaASerExecutada, SqlC))
                {
                    using (SqlDataReader reader = dbCommand.ExecuteReader())
                    {
                        NumberOfColumns = reader.FieldCount;

                        while (reader.Read())
                        {
                            for (int i = 0; i < NumberOfColumns; i++)
                            {
                                QueryResults.Add(reader.GetString(i));
                            }
                        }

                    }
                }
            }

            return QueryResults;
        }
     

        #endregion


        #region [Parametrização e refactoring (Metodos novos)]

        public void EsperaPeloAlertaVisivel()
        {
            EsperaPorElementoVisivel(By.CssSelector("p"));
        }

        public DateTime RetornaMesReferenciaDoDiaAtualParaFaturamentoDia15()
        {
            DateTime Hoje = DateTime.Now;

            if (Hoje.Day > 15)
                Hoje = DateTime.Now.AddMonths(1);

            return Hoje;
        }

        public bool AreElementsPresent(By by)
        {
            try
            {
                ReadOnlyCollection<IWebElement> ElementsList = driver.FindElements(by);

                if (ElementsList.Count == 0)
                {
                    Console.WriteLine("Não foi encontrado nenhum elemento pelo locator: " + by.ToString());
                    return false;
                }
                else
                {
                    foreach (IWebElement element in ElementsList)
                    {
                        if (!element.Displayed)
                        {
                            Console.WriteLine("Um dos elementos não foram encontrados pelo locator " + by.ToString());
                            return false;
                        }

                    }

                    return true;
                }
            }

            catch (NoSuchElementException)
            {
                Console.WriteLine("Não foi encontrado nenhum elemento pelo locator: " + by.ToString());
                return false;
            }
        }

        public bool OpcaoEstaClicavelNoDropDown(By LocatorDoDropDown, string NomeDaOpcaoDesejada)
        {
            EsperaPorElementoClicavel(LocatorDoDropDown);
            driver.FindElement(LocatorDoDropDown).Click();
            var DropDown = new SelectElement(driver.FindElement(LocatorDoDropDown)).Options;

            foreach (IWebElement ItemDropDown in DropDown)
            {

                if (ItemDropDown.Text.Equals(NomeDaOpcaoDesejada))
                {
                    if ((RetornaValorDoAtributoDeUmElemento(ItemDropDown, "disabled")).Equals("empty") &&      //Se ele não tiver o disabled
                       (RetornaValorDoAtributoDeUmElemento(ItemDropDown, "style")).Equals("empty"))            //Nem tiver o style
                    {
                        Console.WriteLine("Opção " + NomeDaOpcaoDesejada + " está presente e clicavel no dropdown");
                        return true;
                    }

                    else if (!(RetornaValorDoAtributoDeUmElemento(ItemDropDown, "disabled").Equals("true")) &&              //E não tiver o disabled ativado
                            (!(RetornaValorDoAtributoDeUmElemento(ItemDropDown, "style").Equals("display: none;"))))        //E não estiver com display desativado
                    {
                        Console.WriteLine("Opção " + NomeDaOpcaoDesejada + " está presente e clicavel no dropdown");
                        return true;
                    }

                }

            }

            Console.WriteLine("Opção " + NomeDaOpcaoDesejada + " não está presente e clicavel no dropdown");
            return false;

        }

        public int RetornaNumeroDaLinhaDeUmDiaUtil()
        {
            for (int i = 1; i <= 15; i++)
            {
                if (!driver.FindElement(By.XPath("(//tr[@id='linha']/td[2])[" + i + "]")).Text.Contains("sáb") &&
                   !driver.FindElement(By.XPath("(//tr[@id='linha']/td[2])[" + i + "]")).Text.Contains("dom"))

                    return i;
            }

            return 0;
        }


        /// <summary>
        /// Retorna o texto contido no elemento. Caso não tiver nenhum texto, retorna "empty"
        /// </summary>
        /// <param name="element"></param>
        /// <param name="AtributoDesejado"></param>
        /// <returns></returns>
        public string RetornaValorDoAtributoDeUmElemento(IWebElement element, string AtributoDesejado)
        {
            try
            {
                if (String.IsNullOrEmpty(element.GetAttribute(AtributoDesejado)))
                    return "empty";
                else
                    return element.GetAttribute(AtributoDesejado);
            }

            catch (Exception)
            {
                return "empty";
            }

        }

        public bool IsTextInAlert(string MensagemEsperadaNoAlerta)
        {
            ReadOnlyCollection<IWebElement> Alertas;
            bool TextoEstaNoAlerta = false;

            if (IsElementPresent((By.CssSelector("p"))))
            {
                Alertas = driver.FindElements((By.XPath("//p")));

                foreach (IWebElement alert in Alertas)
                {
                    if (alert.Text.Equals(MensagemEsperadaNoAlerta))
                    {
                        TextoEstaNoAlerta = true;
                        break;
                    }
                }

            }
            else
            {
                Console.WriteLine("O alerta não foi encontrado");
                return TextoEstaNoAlerta;
            }

            return TextoEstaNoAlerta;
        }

        public bool AreTextsInAlerts(List<string> MensagensEsperadasNosAlertas)
        {
            ReadOnlyCollection<IWebElement> Alertas;
            bool TextosEstaoNosAlertas = true;
            bool AlertaEstaComOTexto = false;

            if (IsElementPresent((By.CssSelector("p"))))
            {
                Alertas = driver.FindElements((By.XPath("//p")));

                foreach (string alertText in MensagensEsperadasNosAlertas)
                {
                    foreach (IWebElement alert in Alertas)
                    {
                        AlertaEstaComOTexto = false;

                        if (alert.Text.Equals(alertText))
                        {
                            AlertaEstaComOTexto = true;
                            break;
                        }
                    }

                    if (AlertaEstaComOTexto == false)
                    {
                        TextosEstaoNosAlertas = false;
                        Console.WriteLine("O texto: " + "\"" + alertText + "\"" + " não foi encontrado nos alertas");
                        break;
                    }

                }

                //TextosEstaoNosAlertas = true;
            }
            else
            {
                Console.WriteLine("O alerta não foi encontrado");
                return TextosEstaoNosAlertas;
            }

            return TextosEstaoNosAlertas;
        }

        public void ValidaCamposObrigatoriosGeral(List<string> MensagensEsperadasNosAlertas)
        {
            EsperaPeloAlertaVisivel();

            if (!AreTextsInAlerts(MensagensEsperadasNosAlertas))
                Assert.Fail("Os campos deveriam ser obrigatorios, porem nao foram requisitados");
            else
                Console.WriteLine("Validou campos obrigatorios\n");
        }

        public void ValidaCamposObrigatoriosGeral(string MensagemEsperadaNoAlerta)
        {
            EsperaPorElementoVisivel(By.CssSelector("p"));

            if (!IsTextInAlert(MensagemEsperadaNoAlerta))
                Assert.Fail("Os campo deveria ser obrigatorio, porem nao foi requisitado");
            else
                Console.WriteLine("Validou campo obrigatorio");
        }


        #endregion


        #region [Métodos JS (Tem métodos novos)]

        public object ExecutaComandoJavaScript(string ComandoJS)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            return js.ExecuteScript(ComandoJS);
        }

        public void RealizaScrollAteElemento(By by)
        {
            IWebElement element = driver.FindElement(by);
            ExecutaComandoJavaScript("window.scrollBy(0, " + element.Location.Y + ")");
        }

        public void MarcaPosicaoOndeDeveriaReceberOClique(int X, int Y)
        {
            ExecutaComandoJavaScript("document.elementFromPoint(" + X + "," + Y + ").style.color = 'red'");
        }

        public void SelecionarOpcaoDropDown(String idDropDown, String opcaoSelecionar)
        {
            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
            String dropdownScript = "var select = window.document.getElementById('" +
             idDropDown +
             "'); for(var i = 0; i < select.options.length; i++){if(select.options[i].text == '" +
             opcaoSelecionar +
             "'){ select.options[i].selected = true; } }";

            Thread.Sleep(2000);
            executor.ExecuteScript(dropdownScript);
            Thread.Sleep(2000);

            String clickScript = "if (" + "\"createEvent\"" + " in document) {var evt = document.createEvent(" + "\"HTMLEvents\"" + ");     evt.initEvent(" + "\"change\"" + ", false, true); " + idDropDown + ".dispatchEvent(evt); } else " + idDropDown + ".fireEvent(" + "\"onchange\"" + ");";

            executor.ExecuteScript(clickScript);
        }

        #endregion


        #region [Esperas explicitas]

        public void EsperaPorElementoClicavel(By by)
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.ElementToBeClickable(by));
        }

        public void EsperaPorElementoVisivel(By by)
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.ElementIsVisible(by));
        }

        public void EsperaPorElementosLocalizadosPor(By elemento, int tempo)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(tempo)).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(elemento));
        }

        //Tem que testar ainda
        public void EsperaAteMudancaDoAtributoDoElemento(By by, string AtributoDesejado, string NovoValor)
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(45));

            wait.Until(driver => driver.FindElement(by).Enabled
                  && RetornaValorDoAtributoDeUmElemento(driver.FindElement(by), AtributoDesejado).Contains(NovoValor)
              );
        }

        public bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion

       
    }
}

