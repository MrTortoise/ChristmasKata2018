using System;
using ChristmasKata2018.SeventhCircleOfChristmas;

namespace ChristmasKata2018
{
    /// <summary>
    /// Santa has to handle letters from all kinds of location.
    /// Some locations have certain properties that others do not.
    /// In order to handle these differing behaviours the elves and their mischief created a way of switching
    /// handlers dynamically at runtime based upon some configuration passed in. 
    /// </summary>
    /// <remarks>
    /// Its christmas and once again young hopefuls are sending the requests to santas poor
    /// overburdened workers - the elves (The unpaid army of 'workers' that arn't at all a worrying christmas idea).
    /// Due to scalability and the generally 'ingenious' nature of elves there can be different deployments
    /// based upon letter origin.
    /// This class enables overriding a default set of behaviour found in the base LetterHandlers for optionally specified
    /// letter origins or present tiers in the names of the letter handlers.
    /// </remarks>
    public class CustomLetterHandlerFactory : DefaultLetterHandlerFactory
    {
        private readonly string _letterOrigin;
        private readonly string _presentTier;
        public Func<RequestContext, string, Type> Getter;

        /// <summary>
        /// Configures the Handler factory for a specific origin and present tier combination
        /// </summary>
        /// <param name="letterOrigin">Potentially a country or region in the space that santa operates in</param>
        /// <param name="presentTier">Sadly christmas comes with the veiled threat and
        /// assumption that some delightful darlings respond to coal better than carrots</param>
        public CustomLetterHandlerFactory(string letterOrigin, string presentTier)
        {
            _letterOrigin = letterOrigin;
            _presentTier = presentTier;
            Getter = (context, name) => base.GetLetterHandlerType(context, name);
        }

        /// <summary>
        /// Returns the desired type of <see cref="ILetterHandler"/> to handle the request depending on the name
        /// requested but also config.
        /// </summary>
        /// <remarks>Through a reason only discernible to the higher elves, requests come against specific types of letter.
        /// Eg HandWritten, ClearlyWrittenByParent, CheapDotMatrix, Twitter (Santa embraces omni-chanel)</remarks>
        /// <param name="requestContext">The request context</param>
        /// <param name="letterHandlerName">The requested handler - Eg HandWritten</param>
        /// <returns></returns>
        protected internal override Type GetLetterHandlerType(RequestContext requestContext, string letterHandlerName)
        {
            return ChooseLetterHandlerType(requestContext, letterHandlerName);
        }

        public Type ChooseLetterHandlerType(RequestContext requestContext, string letterHandlerName)
        {
            return Getter(requestContext, GetLetterOriginAndPresentTierSpecificLetterHandlerName(letterHandlerName)) ??
                   Getter(requestContext, GetLetterOriginSpecificLetterHandlerName(letterHandlerName)) ??
                   Getter(requestContext, GetPresentTierSpecificLetterHandlerName(letterHandlerName)) ??
                   Getter(requestContext, letterHandlerName);
        }


        private string GetLetterOriginAndPresentTierSpecificLetterHandlerName(string letterHandlerName)
        {
            return string.Concat(_letterOrigin, _presentTier, letterHandlerName);
        }

        private string GetLetterOriginSpecificLetterHandlerName(string letterHandlerName)
        {
            return string.Concat(_presentTier, letterHandlerName);
        }

        private string GetPresentTierSpecificLetterHandlerName(string letterHandlerName)
        {
            return string.Concat(_letterOrigin, letterHandlerName);
        }
    }
}