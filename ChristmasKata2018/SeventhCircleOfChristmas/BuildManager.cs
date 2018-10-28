using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ChristmasKata2018.SeventhCircleOfChristmas
{
    internal class BuildManager
    {
        public static object GetObjectFactory(string virtualPath, bool throwIfNotFound)
        {
            return new BuildManager();
        }

        public static Type GetCompiledType(string virtualPath)
        {
            return typeof(string);
        }

        public static ICollection GetReferencedAssemblies()
        {
            return new List<string>();
        }

        public static Stream ReadCachedFile(string fileName)
        {
            return Stream.Null;
        }

        public static Stream CreateCachedFile(string fileName)
        {
            return Stream.Null;
        }
    }
}