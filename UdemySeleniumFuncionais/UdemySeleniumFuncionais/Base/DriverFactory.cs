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
using OpenQA.Selenium.Remote;

namespace Base
{
    public class DriverFactory
    {
        public SqlConnectionStringBuilder SqlSB = new SqlConnectionStringBuilder();
        public ChromeOptions options;

        protected IWebDriver driver;
        protected WebDriverWait wait;


        #region [SQL]

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


        #region [JS]

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
        
        public void EsperaAteMudancaDoAtributoDoElemento(By by, string AtributoDesejado, string NovoValor)
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(45));

            wait.Until(driver => driver.FindElement(by).Enabled
                  && RetornaValorDoAtributoDeUmElemento(driver.FindElement(by), AtributoDesejado).Contains(NovoValor)
              );
        }

        #endregion


        #region[Esta presente na tela?]

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

        public bool isAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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


        #endregion


        #region [Captura de tela]

        public void TirarPrint()
        {
            DateTime dateTime = DateTime.Now;
            string horaAtual = dateTime.ToString();
            horaAtual = horaAtual.Replace('/', '-'); horaAtual = horaAtual.Replace(':', '_');

            string pastaArquivo = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            try
            {
                Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                ss.SaveAsFile(pastaArquivo + "\\fullScreenShot " + horaAtual + ".jpeg", ScreenshotImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }

        #endregion


        #region [Metodos do browser]

        protected void AcessaLink(string link)
        {
            driver.Navigate().GoToUrl(link);
        }

        protected void InicializaBrowserAnonimo(string linkToAccess)
        {
            options = new ChromeOptions();
            options.AddArguments("--incognito");

            InicializaBrowser(linkToAccess);
        }

        protected void InicializaBrowserAnonimoHeadless(string linkToAccess)
        {
            options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArguments("--incognito");

            InicializaBrowser(linkToAccess);
        }

        //OS PRINTS FICAM BUGADOS
        protected void InicializaBrowserHeadLess(string linkToAccess)
        {
            options = new ChromeOptions();
            options.AddArgument("--headless");

            InicializaBrowser(linkToAccess);
        }

        protected void InicializaBrowser(string linkToAccess)
        {
            options.AddArgument("--start-maximized");

            driver = new ChromeDriver(options);
            AcessaLink(linkToAccess);
        }

        protected void FinalizaNavegador()
        {
            if (isAlertPresent())
            {
                driver.SwitchTo().Alert().Accept();
                driver.Quit();
            }

            else
                driver.Quit();
        }

        #endregion


        #region [DSL]
        protected void PreencheCampo(string texto, By by)
        {
            EsperaPorElementoVisivel(by);
            driver.FindElement(by).Clear();
            driver.FindElement(by).SendKeys(texto);
        }

        protected void ClicaBotao(By by)
        {
            EsperaPorElementoClicavel(by);
            driver.FindElement(by).Click();
        }
        #endregion


        #region [Retorno]

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

        #endregion

    }
}

