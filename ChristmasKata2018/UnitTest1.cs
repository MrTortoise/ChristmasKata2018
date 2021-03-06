using ChristmasKata2018.SeventhCircleOfChristmas;
using Xunit;

namespace ChristmasKata2018
{
    public class UnitTest1
    {
        [Fact]
        public void AStartingPoint()
        {
            bool wasCalled = false;
            var requestContext = new RequestContext();
            var factory = new CustomLetterHandlerFactory("some", "values");
            factory.Getter = (context, name) =>
            {
                // The point is here you can inject scenarios / strategies 
                // that describe what combinations of controllers exist
                // and what you want the behaviour to be in those scenarios.
                Assert.Equal(requestContext, context);
                Assert.Equal("somevaluestest", name);
                wasCalled = true;
                return typeof(LetterHandlers.HandWrittenLetterHandler);
            };

            factory.ChooseLetterHandlerType(requestContext, "test");
            
            Assert.True(wasCalled);
        }
    }
}