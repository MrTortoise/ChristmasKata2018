namespace ChristmasKata2018.SeventhCircleOfChristmas
{
    public interface ILetterHandlerFactory
    {
        ILetterHandler CreateLetterHandler(RequestContext requestContext, string letterHandlerName);
        SessionStateBehavior GetLetterHandlerSessionBehavior(RequestContext requestContext, string letterHandlerName);
        void ReleaseLetterHandler(ILetterHandler letterHandler);
    }
}