# IIS Express Bootstrapper

[![CodeFactor](https://www.codefactor.io/repository/github/lambda3/iisexpress-bootstrapper/badge)](https://www.codefactor.io/repository/github/lambda3/iisexpress-bootstrapper)

## What is "IIS Express Bootstrapper"?

IIS Express Bootstrapper is a very simple library built to make easy run IIS
Express from your C# code for a web application project.

It's a great library for acceptance tests.

## How do I install it?

You can install this
[package](https://www.nuget.org/packages/Lambda3.IISExpressBootstrapper/) via NuGet
Package Manager, Package Manager Console, or dotnet.

```powershell
Install-Package IISExpressBootstrapper
# or
dotnet add package IISExpressBootstrapper
```

## Basic sample

```csharp
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
```

Yes, simple as that. With this library you can easily setup IIS Express and
write your acceptance tests for your web application using Selenium Webdriver,
WatyN or whatever you want.

Note: In order to work, the web application and the acceptance test project must
be in the same solution folder. You can see a full sample
[here](https://github.com/abnerdasdores/iisexpress-bootstrapper-sample).

## Contributing

Questions, comments, bug reports, and pull requests are all welcome.  Submit them at
[the project on GitHub](https://github.com/Lambda3/iisexpress-bootstrapper/).

Bug reports that include steps-to-reproduce (including code) are the
best. Even better, make them in the form of pull requests.

## Maintainers

- [Giovanni Bassi](http://blog.lambda3.com.br/L3/giovannibassi/), aka Giggio, [Lambda3](http://www.lambda3.com.br), [@giovannibassi](https://twitter.com/giovannibassi)
- [Many other contributors](https://github.com/Lambda3/iisexpress-bootstrapper/graphs/contributors)

## License

This software is open source, licensed under the MIT.
See [LICENSE](https://github.com/Lambda3/iisexpress-bootstrapper/blob/main/LICENSE) for details.
Check out the terms of the license before you contribute, fork, copy or do anything
with the code. If you decide to contribute you agree to grant copyright of all your contribution to this project, and agree to
mention clearly if do not agree to these terms. Your work will be licensed with
the project at MIT, along the rest of the code.
