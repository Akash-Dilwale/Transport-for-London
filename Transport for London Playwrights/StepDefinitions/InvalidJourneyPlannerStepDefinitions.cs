using NUnit.Framework;
using System;
using System.Xml.Linq;
using TechTalk.SpecFlow;
using Transport_for_London.Support;
using Transport_for_London.Hooks;
using Microsoft.Playwright;

namespace Transport_for_London.StepDefinitions
{
    [Binding]
    public class InvalidJourneyPlannerStepDefinitions : Locators_AppConst

    {


        IPage page;

        public InvalidJourneyPlannerStepDefinitions(HooksClass chain)
        {
            this.page = chain.page;
        }


        [Then(@"I should see an error message")]
        public async Task ThenIShouldSeeAnErrorMessage()
        {

            // Wait for the "Accept all cookies" button to be visible
            await page.WaitForSelectorAsync(invalidDestinationError,
                new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = 12000
                });

           
            bool val = await page.Locator(invalidDestinationError).IsVisibleAsync();

            await page.CloseAsync();
            Assert.IsTrue(val);
        }

    }
}
