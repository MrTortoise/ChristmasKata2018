using System.Collections;
using System.IO;

namespace ChristmasKata2018.SeventhCircleOfChristmas
{
    internal interface IBuildManager
    {
        ICollection GetReferencedAssemblies();
        Stream ReadCachedFile(string fileName);
        Stream CreateCachedFile(string fileName);
    }
}