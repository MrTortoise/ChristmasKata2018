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
    /// Santa has to handle letters from all kinds of location.
    /// Some locations have certain properties that others do not.
    /// In order to handle these differing behaviours the elves and their mischeif created a way of switching
    /// handlers dynamically at runtime based upon some configuration passed in. 
    /// </summary>
    /// <remarks>
    /// Its christmas and once again young hopefuls are sending the requests to santas poor
    /// overburdened workers - the elves (The unpaid army of 'workers' that arn't at all a
    /// worrying christmas idea).
    /// Due to scalability and the generally 'ingenious' nature of elves there can be different deployments
    /// based upon letter origin.
    /// This class enables overriding a default set of behaviour found in the base LetterHandlers for optionally specified
    /// letter origins or present tiers in the names of the letter handlers.
    /// </remarks>
    public class CustomLetterHandlerFactory : DefaultLetterHandlerFactory
    {
        private readonly string _siteName;
        private readonly string _platformName;

        /// <summary>
        /// Configures the Handler factory for a specific origin and present tier combination
        /// </summary>
        /// <param name="letterOrigin">Potentially a country or region in the space that santa operates in</param>
        /// <param name="presentTier">Sadly christmas comes with the veiled threat and
        /// assumption that some delightful darlings respond to coal better than carrots</param>
        public CustomLetterHandlerFactory(string letterOrigin, string presentTier)
        {
            _siteName = letterOrigin;
            _platformName = presentTier;
            
        }

        protected internal override Type GetLetterHandlerType(RequestContext requestContext, string letterHandlerName)
        {
            return base.GetLetterHandlerType(requestContext, GetSiteAndPlatformSpecificControllerName(letterHandlerName)) ??
                   base.GetLetterHandlerType(requestContext, GetPlatformSpecificControllerName(letterHandlerName)) ??
                   base.GetLetterHandlerType(requestContext, GetSiteSpecificControllerName(letterHandlerName)) ??
                   base.GetLetterHandlerType(requestContext, letterHandlerName);
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
}