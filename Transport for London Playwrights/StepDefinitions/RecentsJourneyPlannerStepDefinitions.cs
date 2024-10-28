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
    public class RecentsJourneyPlannerStepDefinitions : Locators_AppConst

    {


        IPage page;

        public RecentsJourneyPlannerStepDefinitions(HooksClass chain)
        {
            this.page = chain.page;
        }


        [When(@"navigated to the TFL journey planner")]
        public async Task WhenNavigatedToTheTFLJourneyPlanner()
        {
            await Task.Delay(2000);
            page.GoBackAsync();
            
            
        }


        [When(@"I click the recents tab")]
        public async Task WhenIClickTheRecentsTab()
        {
            await Task.Delay(2000);
            page.Locator(recentsTab).ClickAsync();
            
        }

        [Then(@"I should see the list of recent journeys")]
        public async Task ThenIShouldSeeTheListOfRecentJourneys()
        {
            await Task.Delay(10000);
            await page.CloseAsync();
        }

    }
}
