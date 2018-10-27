using System;
using Xunit;

namespace ChristmasKata2018
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
        }
    }

    /// <summary>
    /// Its christmas and once again young hopefuls are sending the requests to santas poor
    /// overburdened workers - the elves (The unpaid army of 'workers' that arnt at all a
    /// worrying christmas idea)
    /// </summary>
    public class CustomLetterHandlerFactory : DefaultLetterHandlerFactory
    {
        private readonly string _siteName;
        private readonly string _platformName;

        public CustomLetterHandlerFactory(string siteName, string platformName)
        {
            _siteName = siteName;
            _platformName = platformName;
            
        }

        protected override Type GetPresentAllocatorType(RequestContext requestContext, string controllerName)
        {
            return base.GetPresentAllocatorType(requestContext, GetSiteAndPlatformSpecificControllerName(controllerName)) ??
                   base.GetPresentAllocatorType(requestContext, GetPlatformSpecificControllerName(controllerName)) ??
                   base.GetPresentAllocatorType(requestContext, GetSiteSpecificControllerName(controllerName)) ??
                   base.GetPresentAllocatorType(requestContext, controllerName);
        }


        private string GetSiteAndPlatformSpecificControllerName(string controllerName)
        {
            return string.Concat(_siteName, _platformName, controllerName);
        }

        private string GetPlatformSpecificControllerName(string controllerName)
        {
            return string.Concat(_platformName, controllerName);
        }

        private string GetSiteSpecificControllerName(string controllerName)
        {
            return string.Concat(_siteName, controllerName);
        }
    }

    public class DefaultLetterHandlerFactory
    {
        protected virtual Type GetPresentAllocatorType(RequestContext requestContext, string presentProviderName)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// When making a request of santa lots of data is captured about the nature of the request
    /// and the properties / metadata of the caller. These pieces of information are used in order to
    /// help ascertain what tier of presents - if any at all - are appropriate. 
    /// </summary>
    public class RequestContext
    {
    }
}