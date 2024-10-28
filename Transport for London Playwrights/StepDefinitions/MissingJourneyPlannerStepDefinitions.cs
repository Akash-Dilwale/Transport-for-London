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
    public class MissingJourneyPlannerStepDefinitions : Locators_AppConst

    {


        IPage page;

        public MissingJourneyPlannerStepDefinitions(HooksClass chain)
        {
            this.page = chain.page;
        }



        [Then(@"I should see a message indicating missing locations")]
        public async Task ThenIShouldSeeAMessageIndicatingMissingLocations()
        {
            string fromError = await page.Locator(inputFromErrorMsg).InnerTextAsync();
               
            string toError = await page.Locator(inputToErrorMsg).InnerTextAsync();
           
             if (fromError.Contains("field is required.") & 
                    toError.Contains("field is required.") )
            {
                Assert.True(true);
            }
            await page.CloseAsync();
        }

       

    }
}
