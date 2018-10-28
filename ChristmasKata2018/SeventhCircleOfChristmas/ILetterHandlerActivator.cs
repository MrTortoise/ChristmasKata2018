using System;

namespace ChristmasKata2018.SeventhCircleOfChristmas
{
    public interface ILetterHandlerActivator
    {
        ILetterHandler Create(RequestContext requestContext, Type letterHandlerType);
    }
}