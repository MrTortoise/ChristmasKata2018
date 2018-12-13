// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Adapted for a Kata by John Nicholas (for the worse)

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ChristmasKata2018.SeventhCircleOfChristmas
{
    public class DefaultLetterHandlerFactory : ILetterHandlerFactory
    {
        private static readonly ConcurrentDictionary<Type, SessionStateBehavior> _sessionStateCache =
            new ConcurrentDictionary<Type, SessionStateBehavior>();

        private static LetterHandlerTypeCache _staticLetterHandlerTypeCache = new LetterHandlerTypeCache();
        private IBuildManager _buildManager;
        private IResolver<ILetterHandlerActivator> _activatorResolver;
        private ILetterHandlerActivator _letterHandlerActivator;
        private LetterHandlerBuilder _letterHandlerBuilder;
        private LetterHandlerTypeCache _instanceLetterHandlerTypeCache;

        public DefaultLetterHandlerFactory()
            : this(null, null, null)
        {
           
        }

        public DefaultLetterHandlerFactory(ILetterHandlerActivator letterHandlerActivator)
            : this(letterHandlerActivator, null, null)
        {
        }

        internal DefaultLetterHandlerFactory(ILetterHandlerActivator letterHandlerActivator,
            IResolver<ILetterHandlerActivator> activatorResolver, IDependencyResolver dependencyResolver)
        {
            if (letterHandlerActivator != null)
            {
                _letterHandlerActivator = letterHandlerActivator;
                
            }
            else
            {
                _activatorResolver = activatorResolver ?? new SingleServiceResolver<ILetterHandlerActivator>(
                                         () => null,
                                         new DefaultLetterHandlerActivator(dependencyResolver),
                                         "DefaultLetterHandlerFactory constructor");
            }
        }

        private ILetterHandlerActivator LetterHandlerActivator
        {
            get
            {
                if (_letterHandlerActivator != null)
                {
                    return _letterHandlerActivator;
                }

                _letterHandlerActivator = _activatorResolver.Current;
                return _letterHandlerActivator;
            }
        }

        internal IBuildManager BuildManager
        {
            get
            {
                if (_buildManager == null)
                {
                    _buildManager = new BuildManagerWrapper();
                }

                return _buildManager;
            }
            set { _buildManager = value; }
        }

        internal LetterHandlerBuilder LetterHandlerBuilder
        {
            get { return _letterHandlerBuilder ?? SeventhCircleOfChristmas.LetterHandlerBuilder.Current; }
            set { _letterHandlerBuilder = value; }
        }

        internal LetterHandlerTypeCache LetterHandlerTypeCache
        {
            get { return _instanceLetterHandlerTypeCache ?? _staticLetterHandlerTypeCache; }
            set { _instanceLetterHandlerTypeCache = value; }
        }

        internal static InvalidOperationException CreateAmbiguousLetterHandlerException(Address address,
            string letterHandlerName, ICollection<Type> matchingTypes)
        {
            // we need to generate an exception containing all the controller types
            StringBuilder typeList = new StringBuilder();
            foreach (Type matchedType in matchingTypes)
            {
                typeList.AppendLine();
                typeList.Append(matchedType.FullName);
            }

            string errorText;
            Address castAddress = address as Address;
            if (castAddress != null)
            {
                errorText = String.Format(CultureInfo.CurrentCulture, "Letter Handler ambiguous with address name",
                    letterHandlerName, castAddress.Url, typeList, Environment.NewLine);
            }
            else
            {
                errorText = String.Format(CultureInfo.CurrentCulture, "Letter Handler ambiguous without address name",
                    letterHandlerName, typeList, Environment.NewLine);
            }

            return new InvalidOperationException(errorText);
        }

        private static InvalidOperationException CreateDirectAddressAmbiguousLetterHandlerException(
            ICollection<Type> matchingTypes)
        {
            // we need to generate an exception containing all the letter handler types
            StringBuilder typeList = new StringBuilder();
            foreach (Type matchedType in matchingTypes)
            {
                typeList.AppendLine();
                typeList.Append(matchedType.FullName);
            }

            string errorText = String.Format(
                CultureInfo.CurrentCulture,
                "Direct address ambiguous in default letter handler",
                typeList,
                Environment.NewLine);

            return new InvalidOperationException(errorText);
        }

        public virtual ILetterHandler CreateLetterHandler(RequestContext requestContext, string letterHandlerName)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }

            if (String.IsNullOrEmpty(letterHandlerName) &&
                (requestContext.AddressData == null || !requestContext.AddressData.HasDirectAddressMatch()))
            {
                throw new ArgumentException("Null or Empty", "letterHandlerName");
            }

            Type letterHandlerType = GetLetterHandlerType(requestContext, letterHandlerName);
            ILetterHandler letterHandler = GetLetterHandlerInstance(requestContext, letterHandlerType);
            return letterHandler;
        }

        protected internal virtual ILetterHandler GetLetterHandlerInstance(RequestContext requestContext,
            Type letterHandlerType)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }

            if (letterHandlerType == null)
            {
                throw new Exception(
                    String.Format(
                        CultureInfo.CurrentCulture,
                        "No letter handler found",
                        requestContext.HttpContext.Request.Path));
            }

            if (!typeof(ILetterHandler).IsAssignableFrom(letterHandlerType))
            {
                throw new ArgumentException(
                    String.Format(
                        CultureInfo.CurrentCulture,
                        "Type is not of the base letter handler",
                        letterHandlerType),
                    "letterHandlerType");
            }

            return LetterHandlerActivator.Create(requestContext, letterHandlerType);
        }

        protected internal virtual SessionStateBehavior GetLetterHandlerSessionBehavior(RequestContext requestContext,
            Type letterHandlerType)
        {
            if (letterHandlerType == null)
            {
                return SessionStateBehavior.Default;
            }

            return _sessionStateCache.GetOrAdd(
                letterHandlerType,
                type =>
                {
                    var attr = type.GetCustomAttributes(typeof(SessionStateAttribute), inherit: true)
                        .OfType<SessionStateAttribute>()
                        .FirstOrDefault();

                    return (attr != null) ? attr.Behavior : SessionStateBehavior.Default;
                });
        }

   
        protected internal virtual Type GetLetterHandlerType(RequestContext requestContext, string letterHandlerName)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }

            if (String.IsNullOrEmpty(letterHandlerName) &&
                (requestContext.AddressData == null || !requestContext.AddressData.HasDirectAddressMatch()))
            {
                throw new ArgumentException("null or empty", "letterHandlerName");
            }

            AddressData addressData = requestContext.AddressData;
            if (addressData != null && addressData.HasDirectAddressMatch())
            {
                return GetLetterHandlerTypeFromDirectRoute(addressData);
            }

            // first search in the current route's namespace collection
            object routeNamespacesObj;
            Type match;
            if (addressData != null &&
                addressData.DataTokens.TryGetValue(AddressDataTokenKeys.Namespaces, out routeNamespacesObj))
            {
                IEnumerable<string> routeNamespaces = routeNamespacesObj as IEnumerable<string>;
                if (routeNamespaces != null && routeNamespaces.Any())
                {
                    HashSet<string> namespaceHash =
                        new HashSet<string>(routeNamespaces, StringComparer.OrdinalIgnoreCase);
                    match = GetLetterHandlerTypeWithinNamespaces(addressData.Address, letterHandlerName, namespaceHash);

                    // the UseNamespaceFallback key might not exist, in which case its value is implicitly "true"
                    if (match != null || false.Equals(addressData.DataTokens[AddressDataTokenKeys.UseNamespaceFallback]))
                    {
                        // got a match or the route requested we stop looking
                        return match;
                    }
                }
            }

            // then search in the application's default namespace collection
            Address route = addressData == null ? null : addressData.Address;
            if (LetterHandlerBuilder.DefaultNamespaces.Count > 0)
            {
                HashSet<string> namespaceDefaults = new HashSet<string>(LetterHandlerBuilder.DefaultNamespaces,
                    StringComparer.OrdinalIgnoreCase);
                match = GetLetterHandlerTypeWithinNamespaces(route, letterHandlerName, namespaceDefaults);
                if (match != null)
                {
                    return match;
                }
            }

            // if all else fails, search every namespace
            return GetLetterHandlerTypeWithinNamespaces(route, letterHandlerName, null /* namespaces */);
            
            
            // If you are reading this then you probably want to do something different.
        }

        private static Type GetLetterHandlerTypeFromDirectRoute(AddressData addressData)
        {
            Contract.Assert(addressData != null);

            var matchingAddressDatas = addressData.GetDirectAddressMatches();

            List<Type> letterHandlerTypes = new List<Type>();
            foreach (var directAddressData in matchingAddressDatas)
            {
                if (directAddressData != null)
                {
                    Type letterHandlerType = directAddressData.GetTargetLetterHandlerType();
                    if (letterHandlerType == null)
                    {
                        // We don't expect this to happen, but it could happen if some code messes with the 
                        // address data tokens and removes the key we're looking for. 
                        throw new InvalidOperationException("Missing Letter Handler Type");
                    }

                    if (!letterHandlerTypes.Contains(letterHandlerType))
                    {
                        letterHandlerTypes.Add(letterHandlerType);
                    }
                }
            }

            // We only want to handle the case where all matched direct addresses refer to the same letter handler.
            // Handling the multiple-letter handlers case would put attribute routing down a totally different
            // path than traditional addressing.
            if (letterHandlerTypes.Count == 0)
            {
                return null;
            }
            else if (letterHandlerTypes.Count == 1)
            {
                return letterHandlerTypes[0];
            }
            else
            {
                throw CreateDirectAddressAmbiguousLetterHandlerException(letterHandlerTypes);
            }
        }

        private Type GetLetterHandlerTypeWithinNamespaces(Address route, string letterHandlerName,
            HashSet<string> namespaces)
        {
            // Once the master list of letter handlers has been created we can quickly index into it
            LetterHandlerTypeCache.EnsureInitialized(BuildManager);

            ICollection<Type> matchingTypes = LetterHandlerTypeCache.GetLetterHandlerTypes(letterHandlerName, namespaces);
            switch (matchingTypes.Count)
            {
                case 0:
                    // no matching types
                    return null;

                case 1:
                    // single matching type
                    return matchingTypes.First();

                default:
                    // multiple matching types
                    throw CreateAmbiguousLetterHandlerException(route, letterHandlerName, matchingTypes);
            }
        }

        public virtual void ReleaseLetterHandler(ILetterHandler letterHandler)
        {
            IDisposable disposable = letterHandler as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        internal IReadOnlyList<Type> GetControllerTypes()
        {
            LetterHandlerTypeCache.EnsureInitialized(BuildManager);
            return LetterHandlerTypeCache.GetLetterHandlerTypes();
        }

        SessionStateBehavior ILetterHandlerFactory.GetLetterHandlerSessionBehavior(RequestContext requestContext,
            string controllerName)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }

            if (String.IsNullOrEmpty(controllerName))
            {
                throw new ArgumentException("Null or emptyy", "letterHandlerName");
            }

            Type controllerType = GetLetterHandlerType(requestContext, controllerName);
            return GetLetterHandlerSessionBehavior(requestContext, controllerType);
        }

        private class DefaultLetterHandlerActivator : ILetterHandlerActivator
        {
            private Func<IDependencyResolver> _resolverThunk;

            public DefaultLetterHandlerActivator()
                : this(null)
            {
            }

            public DefaultLetterHandlerActivator(IDependencyResolver resolver)
            {
                if (resolver == null)
                {
                    _resolverThunk = () => DependencyResolver.Current;
                }
                else
                {
                    _resolverThunk = () => resolver;
                }
            }

            public ILetterHandler Create(RequestContext requestContext, Type controllerType)
            {
                try
                {
                    return (ILetterHandler) (_resolverThunk().GetService(controllerType) ??
                                             Activator.CreateInstance(controllerType));
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        String.Format(
                            CultureInfo.CurrentCulture,
                            "Error Creating Letter Handler",
                            controllerType),
                        ex);
                }
            }
        }
    }
}
    