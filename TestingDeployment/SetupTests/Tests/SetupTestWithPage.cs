using System;
using FluentAutomation;
using NUnit.Framework;
using TestingDeployment.SetupTests.Pages;

namespace TestingDeployment.SetupTests.Tests
{
        [TestFixture]
        public class SetupTestWithPage : IDisposable
        {
            private string baseUrl;
            private StartUpPage startUpPage;

            public SetupTestWithPage(string url)
            {
                startUpPage = new StartUpPage(SeleniumWebDriver.Browser.Chrome);
                //baseUrl = DeployURL.Host;
                baseUrl = url;
            }


            [Test]
            public void OpenProjectWebPage()
            {
                startUpPage.OpenUrl(baseUrl + "/Search/Index.html");
                startUpPage.EnterSurName("john");
                startUpPage.ClickSubmit();
                startUpPage.ClickOnFirstSearchResult();
            }

            [TestFixtureTearDown]

            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

           private void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (this.startUpPage != null)
                    {
                        this.startUpPage.Dispose();
                        this.startUpPage = null;
                    }
                }
            }
        }
    }
