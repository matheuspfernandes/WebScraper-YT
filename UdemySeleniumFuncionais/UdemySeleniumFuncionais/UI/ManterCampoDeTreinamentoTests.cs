using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using System.Drawing;
using System.Threading;

namespace UdemySeleniumFuncionais
{
    [TestClass]
    public class ManterCampoDeTreinamentoTests
    {
        IWebDriver driver = new ChromeDriver();
        IList<IWebElement> ListaDeOpcoesDoDropDown;
        SelectElement DropDown;


        #region[Metodos Principais]

        [TestMethod()]
        [TestCategory("Interface")]
        public void UITestePrincipalCampoDeTreinamento()
        {
            try
            {
                string path = "file:///" + Environment.CurrentDirectory + "../../../Campo de treinamento HTML/componentes.html";
                driver.Navigate().GoToUrl(path);
                driver.Manage().Window.Size = new Size(1200, 800);

                /*InsereNoTextBoxNomeSobrenome();
                SelecionaSexoEInsereNoTextBox();
                InsereNoDropDownEscolaridade();
                InsereNoDropDownEsportes();*/
                InsereElementos();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                driver.Quit();
            }
        }

        public void InsereNoTextBoxNomeSobrenome()
        {
            driver.FindElement(By.Id("elementosForm:nome")).SendKeys("Diogo");
            Assert.AreEqual("Diogo", driver.FindElement(By.Id("elementosForm:nome")).GetAttribute("value"));

            driver.FindElement(By.Id("elementosForm:sobrenome")).SendKeys("Mario");
            Assert.AreEqual("Mario", driver.FindElement(By.Id("elementosForm:sobrenome")).GetAttribute("value"));

        }

        public void InsereNoDropDownEsportes()
        {
            //Assert da quantidade elementos presentes no DropDown
            DropDown = new SelectElement(driver.FindElement(By.Id("elementosForm:esportes")));

            ListaDeOpcoesDoDropDown = DropDown.Options;
            Assert.AreEqual(5, ListaDeOpcoesDoDropDown.Count);

            //Select 3 elementos por text (Assert do text)
            DropDown.SelectByText("Natacao");
            DropDown.SelectByText("Corrida");
            DropDown.SelectByText("O que eh esporte?");

            //Assert de quant. de elementos selecionados 
            ListaDeOpcoesDoDropDown = DropDown.AllSelectedOptions;
            Assert.AreEqual(3, ListaDeOpcoesDoDropDown.Count);

            Assert.AreEqual("Natacao", DropDown.SelectedOption.GetAttribute("text"));
            DropDown.DeselectByText("Natacao");

            Assert.AreEqual("Corrida", DropDown.SelectedOption.GetAttribute("value").ToString());
            DropDown.DeselectByText("Corrida");

            Assert.AreEqual("O que eh esporte?", DropDown.SelectedOption.GetAttribute("text"));
            DropDown.DeselectByText("O que eh esporte?");

            DropDown.SelectByText("O que eh esporte?");
            DropDown.SelectByText("Corrida");
            DropDown.SelectByText("Natacao");

            //Deselect(por index) de 1 dos elementos do dropdown (Assert desse deselect)
            DropDown.DeselectByIndex(4);
            ListaDeOpcoesDoDropDown = DropDown.AllSelectedOptions;
            Assert.AreEqual(2, ListaDeOpcoesDoDropDown.Count);

            //Assert elemento que nao existe(AssertFail)
            Assert.AreNotEqual("Basquete", DropDown.SelectedOption);
        }

        public void InsereNoDropDownEscolaridade()
        {
            DropDown = new SelectElement(driver.FindElement(By.Id("elementosForm:escolaridade")));

            DropDown.SelectByIndex(1);
            Assert.AreEqual(1, Convert.ToInt32(DropDown.SelectedOption.GetAttribute("index")));

            DropDown.SelectByValue("2graucomp");
            Assert.AreEqual("2graucomp", DropDown.SelectedOption.GetAttribute("value"));

            DropDown.SelectByText("Mestrado");
            Assert.AreEqual("Mestrado", DropDown.SelectedOption.Text);

            ListaDeOpcoesDoDropDown = DropDown.Options;
            Assert.AreEqual(8, ListaDeOpcoesDoDropDown.Count);

            DropDown.SelectByIndex(2);
            Assert.AreNotEqual(" kjh ioudf jhiou", DropDown.SelectedOption.Text);
        }

        public void ClicaCadastrar()
        {
            driver.FindElement(By.Id("elementosForm:cadastrar")).Click();
            Assert.AreEqual("Cadastrado!", driver.FindElement(By.XPath("//*[@id='resultado']/span")).Text);
        }

        public void SelecionaSexoEInsereNoTextBox()
        {
            driver.FindElement(By.Id("elementosForm:sugestoes")).SendKeys("SE fala Aidi, e não Idê OPAA!! FERA");
            Assert.AreEqual("SE fala Aidi, e não Idê OPAA!! FERA", driver.FindElement(By.Id("elementosForm:sugestoes")).GetAttribute("value"));

            driver.FindElement(By.Id("elementosForm:sexo:0")).Click();
            Assert.IsTrue(driver.FindElement(By.Id("elementosForm:sexo:0")).Selected);

            driver.FindElement(By.Id("elementosForm:sexo:1")).Click();
            Assert.IsFalse(driver.FindElement(By.Id("elementosForm:sexo:0")).Selected);
        }

        public void InsereElementos()
        {
            driver.FindElement(By.Id("buttonSimple")).Click();
            Assert.AreEqual("Obrigado!", driver.FindElement(By.Id("buttonSimple")).GetAttribute("value"));
            Thread.Sleep(1500);

            driver.FindElement(By.LinkText("Voltar")).Click();
            Thread.Sleep(3000);
            //Assert.AreEqual("Voltou!", driver.FindElement(By.Id("resultado")).Text.ToString());
            //Thread.Sleep(1500);

            //Console.WriteLine(driver.FindElement(By.Id("resultado")).ToString());
            //Console.WriteLine(driver.FindElement(By.Id("resultado")).Text);
            //Console.WriteLine(driver.FindElement(By.Id("resultado")).GetAttribute("text"));
            //Console.WriteLine(driver.FindElement(By.Id("resultado")).GetAttribute("value"));
            //Console.WriteLine(driver.FindElement(By.Id("resultado")).GetAttribute("text").ToString());

            driver.FindElement(By.TagName("a")).Click();
            Assert.IsTrue(driver.FindElement(By.TagName("center")).Text.Contains("Voltou!"));

        }

        #endregion


        #region[Metodos de Alert]

        [TestMethod()]
        [TestCategory("Interface")]
        public void UITestePrincipalCampoDeTreinamentoAlert()
        {
            try
            {
                string path = "file:///" + Environment.CurrentDirectory + "../../../Campo de treinamento HTML/componentes.html";
                driver.Navigate().GoToUrl(path);
                driver.Manage().Window.Size = new Size(1200, 800);

                InteragirAlertaSimples();
                InteragirAlertaConfirmacao();
                InteragirAlertaPrompt();
            }
            catch (Exception)
            {
                throw;
            }

            finally
            {
                driver.Quit();
            }

        }
        public void InteragirAlertaSimples()
        {
            driver.FindElement(By.Id("alert")).Click();
            IAlert alert = driver.SwitchTo().Alert();
            string texto = alert.Text;
            Assert.AreEqual("Alert Simples", texto);
            Thread.Sleep(1000);
            alert.Accept();

            driver.FindElement(By.Id("elementosForm:sugestoes")).SendKeys(texto);
            Assert.AreNotEqual("", driver.FindElement(By.Id("elementosForm:sugestoes")).GetAttribute("value"));
        }

        public void InteragirAlertaConfirmacao()
        {
            driver.FindElement(By.Id("confirm")).Click();
            IAlert alert = driver.SwitchTo().Alert();
            Assert.AreEqual("Confirm Simples", alert.Text);
            Thread.Sleep(1000);
            alert.Accept();

            Assert.AreEqual("Confirmado", alert.Text);
            Thread.Sleep(1000);
            alert.Accept();

            driver.FindElement(By.Id("confirm")).Click();
            alert = driver.SwitchTo().Alert();
            Thread.Sleep(1000);
            alert.Dismiss();

            Assert.AreEqual("Negado", alert.Text);
            Thread.Sleep(1000);
            alert.Accept();
        }

        public void InteragirAlertaPrompt()
        {
            driver.FindElement(By.Id("prompt")).Click();
            IAlert alert = driver.SwitchTo().Alert();

            alert.SendKeys("Curso Udemy");
            Thread.Sleep(1000);
            alert.Accept();

            Assert.AreEqual("Era Curso Udemy?", alert.Text);
            Thread.Sleep(1000);
            alert.Accept();

            Assert.AreEqual(":D", alert.Text);
            Thread.Sleep(1000);
            alert.Accept();

            driver.FindElement(By.Id("prompt")).Click();
            alert = driver.SwitchTo().Alert();
            Thread.Sleep(1000);
            alert.Dismiss();

            Assert.AreEqual("Era null?", alert.Text);
            Thread.Sleep(1000);
            alert.Dismiss();

            Assert.AreEqual(":(", alert.Text);
            Thread.Sleep(1000);
            alert.Accept();
        }

        #endregion


    }
}
