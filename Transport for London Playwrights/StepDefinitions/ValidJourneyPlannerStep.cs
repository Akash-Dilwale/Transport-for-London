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
    public class ValidJourneyPlannerStep : Locators_AppConst

    {


        IPage page;

        public ValidJourneyPlannerStep(HooksClass chain)
        {
            this.page = chain.page;
        }


        [Given(@"I have navigated to the TFL journey planner")]
        public async Task GivenIHaveNavigatedToTheTFLJourneyPlanner()
        {
            //await page.GotoAsync("https://tfl.gov.uk/plan-a-journey");
            await page.GotoAsync(configuration["URL:HomePage"]);
            
            //driver.FindElement(By.XPath("//strong[normalize-space()='Accept all cookies']")).Click();

            // Wait for the "Accept all cookies" button to be visible
            await page.WaitForSelectorAsync(acceptAllcookies, 
                new PageWaitForSelectorOptions {
                    State = WaitForSelectorState.Visible ,
                    Timeout = 12000
                });

            bool re = await page.Locator(acceptAllcookies).IsVisibleAsync();
            if (!re)
            {
                
                
                // Wait for the "Accept all cookies" button to be visible
                await page.WaitForSelectorAsync(acceptAllcookies,
                    new PageWaitForSelectorOptions
                    {
                        State = WaitForSelectorState.Visible,
                        Timeout = 12000
                    });
                await page.ClickAsync(acceptAllcookies);
                page.ReloadAsync();

            }
           try
            {
                await page.ClickAsync(acceptAllcookies);
            }
            catch { 
                await page.ClickAsync(acceptAllcookies);
            }

            page.ReloadAsync();
            


        }

        [When(@"I enter '([^']*)' in the From field")]
        public async Task WhenIEnterInTheFromField(string heathrow)
        {
            await page.Locator(inputFrom).FillAsync(heathrow);
            await page.Keyboard.PressAsync("ArrowDown");
            await page.Keyboard.PressAsync("Enter");

            

        }

        [When(@"I enter '([^']*)' in the To field")]
        public async Task WhenIEnterInTheToField(string paddington)
        {
            await page.Locator(inputTo).FillAsync(paddington);
            await page.Keyboard.PressAsync("ArrowDown");
            await page.Keyboard.PressAsync("Enter");
            }

        [When(@"I click the Plan My Journey button")]
        public async void WhenIClickThePlanMyJourneyButton()
        {

            try
            {
                await page.Locator(planjourney).ClickAsync();
            }
            catch
            {
                //await page.Locator(planjourney).ClickAsync(
                //    new() { Force = true });
            }
                
           
        }

        [Then(@"I should see the journey results")]
        public async Task ThenIShouldSeeTheJourneyResults()
        {

            string txt = await page.Locator(journeyresult).InnerTextAsync();
            Assert.IsTrue(txt.Contains("results"));
            //Assert.IsTrue(txt.Contains("Akash"));
            await page.CloseAsync();
        }



    }
}
