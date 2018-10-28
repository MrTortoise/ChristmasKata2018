namespace ChristmasKata2018.SeventhCircleOfChristmas
{
    internal interface IResolver<T>
    {
        T Current { get; }
    }
}