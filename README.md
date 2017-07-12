IIS Express Bootstrapper
=======================

[![CodeFactor](https://www.codefactor.io/repository/github/abnerdasdores/iisexpress-bootstrapper/badge)](https://www.codefactor.io/repository/github/abnerdasdores/iisexpress-bootstrapper)

What is "IIS Express Bootstrapper"?
----------------------------------

IIS Express Bootstrapper is a very simple library built to make easy run IIS Express from your C# code for a web application project.

It's a great library for acceptance tests.

How do I install it?
--------------------------------

You can install this [package](https://www.nuget.org/packages/IISExpressBootstrapper/) via NuGet Package Manager or via Package Manager Console:

    PM> Install-Package IISExpressBootstrapper

Basic sample
--------------------------------

    namespace IISExpressSample
    {
        [TestFixture]
        public class YourTestClass
        {
            [Test]
            public void YourTestMethod()
            {
                // You must replace "IISExpressBootstrapper.SampleWebApp" parameters for your web Application Name
                var host = new IISExpressHost("IISExpressBootstrapper.SampleWebApp", 8088);

                // Your amazing test code goes here

                host.Dispose();
            }
        }
    }
        
Yes, simple as that. With this library you can easily setup IIS Express and write your acceptance tests for your web application using Selenium Webdriver, WatyN or whatever you want.

Note: In order to work, the web application and the acceptance test project must be in the same solution folder. You can see a full sample [here](https://github.com/abnerdasdores/iisexpress-bootstrapper-sample).
