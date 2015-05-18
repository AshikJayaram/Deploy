using FluentAutomation;
using OpenQA.Selenium.Chrome;

namespace TestingDeployment.SetupTests.Pages
{
    class StartUpPage : PageBase.PageBase
    {
        public StartUpPage(SeleniumWebDriver.Browser driver)
            : base(driver)
        {

        }
        public void EnterSurName(string surName)
        {
            I.Wait(3);
            I.Enter(surName).In("#surname");
        }

        public void ClickSubmit()
        {
            I.Click("#generalSearch");
        }

        public void ClickOnFirstSearchResult()
        {
            I.Click("div[data-ng-bind='customer.Name']:eq(0)");
            I.Wait(10);
        }

    }
}

