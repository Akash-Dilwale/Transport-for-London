using TechTalk.SpecFlow;
using Transport_for_London.Support;
using Microsoft.Playwright;
using System.Windows;
using System.Security.Principal;
using System.Drawing;
using System.Windows;
using System.Runtime.InteropServices;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;



namespace Transport_for_London.Hooks
{
    [Binding]
    public class HooksClass: Locators_AppConst



    {
        // Constants for key press and key release events
        const int KEY_DOWN = 0x0000; // Press key
        const int KEY_UP = 0x0002;   // Release key

        // Key codes for Windows and Up Arrow keys
        const int WIN_KEY = 0x5B;    // Left Windows key
        const int UP_ARROW = 0x26;   // Up Arrow key



        IPlaywright playwright;
        IBrowser browser;
        IBrowserContext context;
        public IPage page;


        private static ExtentReports extent;
        ExtentTest test;
        private readonly ScenarioContext _scenarioContext;
        private static readonly object _testLock = new object();  // Test-level lock object for thread safety



        // Constructor Injection for ScenarioContext
        public HooksClass(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }


        [BeforeTestRun]
        public static void SetupReport()
        {
            string CurrentPath = Directory.GetCurrentDirectory();
            string thirdLevelUp = Directory.GetParent(Directory.GetParent(Directory.GetParent(CurrentPath).FullName).FullName).FullName;
            string ExtendReportPath = Path.Combine(thirdLevelUp, "Report", $"TestReport_{DateTime.Now:yyyyMMdd_HHmmss}.html");

            var htmlReporter = new ExtentSparkReporter(ExtendReportPath); // specify path for the report
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }


        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeScenario("@tag1")]
        public void BeforeScenarioWithTag()
        {
            // Example of filtering hooks using tags. (in this case, this 'before scenario' hook will execute if the feature/scenario contains the tag '@tag1')
            // See https://docs.specflow.org/projects/specflow/en/latest/Bindings/Hooks.html?highlight=hooks#tag-scoping

            //TODO: implement logic that has to run before executing each scenario
        }

        [BeforeScenario(Order = 1)]
        public async Task FirstBeforeScenario()
        {

            // Start logging for each test case in the report
            test = extent.CreateTest(_scenarioContext.ScenarioInfo.Title);

            // Example of ordering the execution of hooks
            // See https://docs.specflow.org/projects/specflow/en/latest/Bindings/Hooks.html?highlight=order#hook-execution-order

            //TODO: implement logic that has to run before executing each scenario

            playwright = await Playwright.CreateAsync();

            string Browser = configuration["Settings:Browser"] ?? "chrome"; // Default to chrome
            //string Browser = "chrome";
            String b = Browser.ToLower();

            // 1. Get the dynamic Chrome path
            string chromePath = GetChromePath();
            
            if (b == "chromium")
            {
                browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = false,
                    //SlowMo = 500
                });

            }
            else if (b == "chrome")
            {
                browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    ExecutablePath = chromePath,// Use dynamic Chrome path
                    Headless = false, // You can set this to true if you want the browser to run headles
                    //SlowMo = 1000

                });
            }
            else if (b == "firefox")
            {
                browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            }

            //browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            //{
            //    Headless = false,
            //    //SlowMo = 2000,

               
            //});

            context = await browser.NewContextAsync();

            page = await context.NewPageAsync();
            page.SetDefaultTimeout(60000);
            [DllImport("user32.dll")]
            static extern void keybd_event(byte key, byte scanCode, uint flags, UIntPtr extraInfo);

            // Simulate pressing Windows key and Up Arrow key
            keybd_event((byte)WIN_KEY, 0, KEY_DOWN, UIntPtr.Zero); // Press Windows key
            keybd_event((byte)UP_ARROW, 0, KEY_DOWN, UIntPtr.Zero); // Press Up Arrow

            // Simulate releasing Windows key and Up Arrow key
            keybd_event((byte)UP_ARROW, 0, KEY_UP, UIntPtr.Zero);   // Release Up Arrow
            keybd_event((byte)WIN_KEY, 0, KEY_UP, UIntPtr.Zero);    // Release Windows key


        }

        [AfterStep]
        public async Task AfterStep()
        {
            // Log every step in the scenario
            var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            var stepText = _scenarioContext.StepContext.StepInfo.Text;

            lock (test)
            {

                // Log the step as passed if no errors occurred
                //test.Log(Status.Pass, $"{stepType}: {stepText}");

            }

            if (_scenarioContext.TestError != null)
                {
                // Capture screenshot on step failure
                string CurrentPath = Directory.GetCurrentDirectory();
                string thirdLevelUp = Directory.GetParent(Directory.GetParent(Directory.GetParent(CurrentPath).FullName).FullName).FullName;

                string screenshotFolder = Path.Combine(thirdLevelUp, "Screenshots");
                    //Directory.CreateDirectory(screenshotFolder);
                    string screenshotPath = Path.Combine(screenshotFolder, $"{_scenarioContext.ScenarioInfo.Title}_{DateTime.Now:yyyyMMdd_HHmmss}.png");

                    var screenshotBytes = await page.ScreenshotAsync(new PageScreenshotOptions { FullPage = true });
                    await File.WriteAllBytesAsync(screenshotPath, screenshotBytes);

                    // Log failure with screenshot
                    test.Log(Status.Fail, $"Step Failed: {stepType} - {stepText}");
                    test.AddScreenCaptureFromPath(screenshotPath);
                }
                else
                {
                    test.Log(Status.Pass, $"{stepType}: {stepText}");
                }


            
        }





        [AfterScenario]
        public void AfterScenario()
        {
            lock (_testLock)
            {
                try
                {
                    if (_scenarioContext.TestError != null)
                    {
                        // Log failure details in case of an error
                        test.Log(Status.Fail, "Test Failed: " + _scenarioContext.TestError.Message);
                    }
                    else
                    {
                        // Log success if no errors
                        test.Log(Status.Pass, "Test Passed");
                    }
                }
                catch (Exception e)
                {
                    test.Log(Status.Error, "An exception occurred: " + e.Message);
                }
                finally
                {
                    // Quit the WebDriver instance after scenario execution
                    
                }
            }



            //page.CloseAsync();
            // browser.CloseAsync();

            //TODO: implement logic that has to run after executing each scenario
        }



        [AfterTestRun]
        public static void TearDownReport()
        {
            lock (_testLock)
            {
                // Flush the report to write all logs to the file
                extent.Flush();
            }
        }




        private static string GetChromePath()
        {
            if (OperatingSystem.IsWindows())
            {
                // Common paths for Chrome on Windows
                string[] windowsPaths = {
                @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
            };

                foreach (var path in windowsPaths)
                {
                    if (File.Exists(path))
                    {
                        return path;
                    }
                }
            }
            else if (OperatingSystem.IsMacOS())
            {
                // Common path for Chrome on macOS
                string macPath = @"/Applications/Google Chrome.app/Contents/MacOS/Google Chrome";
                if (File.Exists(macPath))
                {
                    return macPath;
                }
            }
            else if (OperatingSystem.IsLinux())
            {
                // Common path for Chrome on Linux
                string linuxPath = @"/usr/bin/google-chrome";
                if (File.Exists(linuxPath))
                {
                    return linuxPath;
                }
            }

            // If Chrome is not found
            return null;
        }

    }
}