using NUnit.Framework;
using Ranorex;
using System;
using System.Linq;
using System.Threading;

namespace RanorexTestsReworked
{
    [TestFixture]
    public class SampleTests
    {
        [Test]
        [STAThread]
        public void CalcTest(/*[Values(1, 2, 3, 4)] int x*/) // NOTE: uncomment this to run several times, as a theory
        {
            TestingBootstrapper.SetupCore();
            int error = 0;
            //Start calculator and wait for UI to be loaded
            // la, Thiago is 
            try
            {
                Host.Local.RunApplication("calc.exe");
                Thread.Sleep(5000); 
                //Get process name
                var processName = GetActualCalculatorProcessName("calc");

                //Find Calculator | Windows 10
                if (IsWindows10())
                {
                    WindowsApp calculator = Host.Local.Find<WindowsApp>("winapp[@processname='" + processName + "']", Duration.FromMilliseconds(2000)).First();

                    Button button = calculator.FindSingle<Ranorex.Button>(".//button[@automationid='num2Button']");
                    button.Click();

                    button = calculator.FindSingle<Ranorex.Button>(".//button[@automationid='plusButton']");
                    button.Click();

                    button = calculator.FindSingle<Ranorex.Button>(".//button[@automationid='num3Button']");
                    button.Click();

                    button = calculator.FindSingle<Ranorex.Button>(".//button[@automationid='equalButton']");
                    button.Click();

                    //Close calculator
                    calculator.As<Form>().Close();
                }
                //Find Calculator | Windows 8.X or older
                else
                {
                    Form calculator = Host.Local.FindSingle<Form>("form[@processname='" + processName + "']");
                    calculator.EnsureVisible();

                    Button button = calculator.FindSingle<Ranorex.Button>(".//button[@controlid='132']");
                    button.Click();

                    button = calculator.FindSingle<Ranorex.Button>(".//button[@controlid='92']");
                    button.Click();

                    button = calculator.FindSingle<Ranorex.Button>(".//button[@controlid='133']");
                    button.Click();

                    button = calculator.FindSingle<Ranorex.Button>(".//button[@controlid='121']");
                    button.Click();

                    //Close calculator
                    calculator.Close();
                }
            }
            catch (RanorexException e)
            {
                Console.WriteLine(e.ToString());
                Assert.Fail(e.Message);
            }

            Assert.Pass();
        }

        [Test]
        [STAThread]
        public void AppTest()
        {
            TestingBootstrapper.SetupCore();
            Host.Local.RunApplication("notepad.exe");
            Thread.Sleep(2000);

            var processName = GetActualCalculatorProcessName("notepad");
            var calculator = Host.Local.Find<Form>("form[@processname='" + processName + "']").First();
            calculator.EnsureVisible();

            calculator.As<Form>().Close();

            Assert.Pass();
        }


        private static string GetActualCalculatorProcessName(string n)
        {
            string processName = String.Empty;
            var processes = System.Diagnostics.Process.GetProcesses();

            foreach (var item in processes)
            {
                if (item.ProcessName.ToLowerInvariant().Contains(n))
                {
                    processName = item.ProcessName;
                    break;
                }
            }

            return processName;
        }

        private static bool IsWindows10()
        {
            var reg = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

            string productName = (string)reg.GetValue("ProductName");

            return productName.StartsWith("Windows 10");
        }
    }
}
