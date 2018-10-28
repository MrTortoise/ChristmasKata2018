// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChristmasKata2018.SeventhCircleOfChristmas
{
    internal sealed class LetterHandlerTypeCache
    {
        private const string TypeCacheName = "LetterHandlerTypeCache.xml";

        private volatile Dictionary<string, ILookup<string, Type>> _cache;
        private object _lockObj = new object();

        internal int Count
        {
            get
            {
                int count = 0;
                foreach (var lookup in _cache.Values)
                {
                    foreach (var grouping in lookup)
                    {
                        count += grouping.Count();
                    }
                }
                return count;
            }
        }

        internal IReadOnlyList<Type> GetLetterHandlerTypes()
        {
            return new ReadOnlyCollection<Type>(_cache.Values.SelectMany(lookup => lookup.SelectMany(t => t)).ToList());
        }

        public void EnsureInitialized(IBuildManager buildManager)
        {
            if (_cache == null)
            {
                lock (_lockObj)
                {
                    if (_cache == null)
                    {
                        List<Type> controllerTypes = TypeCacheUtil.GetFilteredTypesFromAssemblies(TypeCacheName, IsLetterHandlerType, buildManager);
                        var groupedByName = controllerTypes.GroupBy(
                            t => t.Name.Substring(0, t.Name.Length - "Controller".Length),
                            StringComparer.OrdinalIgnoreCase);
                        _cache = groupedByName.ToDictionary(
                            g => g.Key,
                            g => g.ToLookup(t => t.Namespace ?? String.Empty, StringComparer.OrdinalIgnoreCase),
                            StringComparer.OrdinalIgnoreCase);
                    }
                }
            }
        }

        public ICollection<Type> GetLetterHandlerTypes(string letterHandlerName, HashSet<string> namespaces)
        {
            HashSet<Type> matchingTypes = new HashSet<Type>();

            ILookup<string, Type> namespaceLookup;
            if (_cache.TryGetValue(letterHandlerName, out namespaceLookup))
            {
                // this friendly name was located in the cache, now cycle through namespaces
                if (namespaces != null)
                {
                    foreach (string requestedNamespace in namespaces)
                    {
                        foreach (var targetNamespaceGrouping in namespaceLookup)
                        {
                            if (IsNamespaceMatch(requestedNamespace, targetNamespaceGrouping.Key))
                            {
                                matchingTypes.UnionWith(targetNamespaceGrouping);
                            }
                        }
                    }
                }
                else
                {
                    // if the namespaces parameter is null, search *every* namespace
                    foreach (var namespaceGroup in namespaceLookup)
                    {
                        matchingTypes.UnionWith(namespaceGroup);
                    }
                }
            }

            return matchingTypes;
        }

        internal static bool IsLetterHandlerType(Type t)
        {
            return
                t != null &&
                t.IsPublic &&
                t.Name.EndsWith("LetterHandler", StringComparison.OrdinalIgnoreCase) &&
                !t.IsAbstract &&
                typeof(ILetterHandler).IsAssignableFrom(t);
        }

        internal static bool IsNamespaceMatch(string requestedNamespace, string targetNamespace)
        {
            // degenerate cases
            if (requestedNamespace == null)
            {
                return false;
            }
            else if (requestedNamespace.Length == 0)
            {
                return true;
            }

            if (!requestedNamespace.EndsWith(".*", StringComparison.OrdinalIgnoreCase))
            {
                // looking for exact namespace match
                return String.Equals(requestedNamespace, targetNamespace, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                // looking for exact or sub-namespace match
                requestedNamespace = requestedNamespace.Substring(0, requestedNamespace.Length - ".*".Length);
                if (!targetNamespace.StartsWith(requestedNamespace, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                if (requestedNamespace.Length == targetNamespace.Length)
                {
                    // exact match
                    return true;
                }
                else if (targetNamespace[requestedNamespace.Length] == '.')
                {
                    // good prefix match, e.g. requestedNamespace = "Foo.Bar" and targetNamespace = "Foo.Bar.Baz"
                    return true;
                }
                else
                {
                    // bad prefix match, e.g. requestedNamespace = "Foo.Bar" and targetNamespace = "Foo.Bar2"
                    return false;
                }
            }
        }
    }
}