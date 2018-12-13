using System;
using System.Collections.Generic;

namespace ChristmasKata2018.SeventhCircleOfChristmas
{
    internal class LetterHandlerBuilder
    {
        private static LetterHandlerBuilder _instance = new LetterHandlerBuilder();
        private HashSet<string> _namespaces = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public static LetterHandlerBuilder Current => _instance;

        public HashSet<string> DefaultNamespaces => _namespaces;
    }
}