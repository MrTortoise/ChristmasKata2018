using System;

namespace ChristmasKata2018.SeventhCircleOfChristmas
{
    public class SessionStateAttribute : Attribute
    {
        public SessionStateBehavior Behavior { get; set; } = new SessionStateBehavior();
    }
}