using System.Collections;
using System.IO;

namespace ChristmasKata2018.SeventhCircleOfChristmas
{
    internal sealed class BuildManagerWrapper : IBuildManager
    {
        ICollection IBuildManager.GetReferencedAssemblies()
        {
            return BuildManager.GetReferencedAssemblies();
        }

        Stream IBuildManager.ReadCachedFile(string fileName)
        {
            return BuildManager.ReadCachedFile(fileName);
        }

        Stream IBuildManager.CreateCachedFile(string fileName)
        {
            return BuildManager.CreateCachedFile(fileName);
        }
    }
}