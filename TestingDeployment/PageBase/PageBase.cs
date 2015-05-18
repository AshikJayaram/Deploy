using System;
using FluentAutomation;

namespace TestingDeployment.PageBase
{
    public class PageBase : IDisposable
    {
        protected FluentTest FluentTest;
        
        public PageBase(SeleniumWebDriver.Browser browser)
        {
            SeleniumWebDriver.Bootstrap(browser);
            this.FluentTest = new FluentTest();
            this.I = this.FluentTest.I;
        }

        
         public PageBase()
        {
            SeleniumWebDriver.Bootstrap(SeleniumWebDriver.Browser.Chrome);
            this.FluentTest = new FluentTest();
            this.I = this.FluentTest.I;
        }

        protected FluentAutomation.Interfaces.INativeActionSyntaxProvider I { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        public void OpenUrl(string url)
        {
            this.I.Open(url);
            this.I.Wait(10);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.I != null)
                {
                    this.I.Dispose();
                    this.I = null;
                }

                if (this.FluentTest != null)
                {
                    this.FluentTest.Dispose();
                    this.FluentTest = null;
                }
            }
        }
    }
}
