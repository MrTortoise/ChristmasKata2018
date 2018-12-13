using ChristmasKata2018.SeventhCircleOfChristmas;
using Xunit;

namespace ChristmasKata2018
{
    public class UnitTest1
    {
        [Fact]
        public void AStartingPoint()
        {
            var factory = new CustomLetterHandlerFactory("some", "values");
            Assert.NotNull(factory.CreateLetterHandler(new RequestContext(), "test" ));
        }
    }
}