using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingEnviroment
{
    public static class Util
    {
        
        public static void TirarPrint(IWebDriver driver)
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
    }
}
