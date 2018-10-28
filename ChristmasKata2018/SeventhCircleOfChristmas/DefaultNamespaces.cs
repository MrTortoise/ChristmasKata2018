namespace ChristmasKata2018.SeventhCircleOfChristmas
{
    internal class DefaultNamespaces
    {
        public int Count { get; set; } = 1;
        
        public static implicit operator int(DefaultNamespaces d)
        {
            return d.Count;
        }
    }
}