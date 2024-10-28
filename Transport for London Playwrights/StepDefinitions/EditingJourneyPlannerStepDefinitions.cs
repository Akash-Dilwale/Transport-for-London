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
    public class EditingJourneyPlannerStepDefinitions : Locators_AppConst

    {
        

        IPage page;

        public EditingJourneyPlannerStepDefinitions(HooksClass chain)
        {
            this.page = chain.page;
        }


       

        [When(@"I edit the journey details")]
        public void WhenIEditTheJourneyDetails()
        {
            page.Locator(editJourney).ClickAsync();
            
        }

        [Then(@"I should be able to amend the journey")]
        public async Task ThenIShouldBeAbleToAmendTheJourney()
        {
            await page.WaitForSelectorAsync(planjourney,
                new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = 12000
                });

            bool re = await page.Locator(planjourney).IsVisibleAsync();
                
            Assert.IsTrue(re);
            await page.CloseAsync();
            
        }

       
    }
}
