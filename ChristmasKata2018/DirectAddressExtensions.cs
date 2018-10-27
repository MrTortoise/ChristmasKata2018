// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace ChristmasKata2018
{
    internal static class DirectAddressExtensions
    {
        /// <summary>
        /// Gets the precedence or this Address.
        /// </summary>
        public static decimal GetPrecedence(this AddressData AddressData)
        {
            return GetAddressDataTokenValue<decimal>(AddressData, AddressDataTokenKeys.Precedence);
        }

        /// <summary>
        /// Gets the precedence or this Address.
        /// </summary>
        public static decimal GetPrecedence(this Address address)
        {
            return GetAddressDataTokenValue<decimal>(address, AddressDataTokenKeys.Precedence);
        }

        /// <summary>
        /// Sets the precedence of this Address.
        /// </summary>
        public static void SetPrecedence(this Address Address, decimal precedence)
        {
        }

        /// <summary>
        /// Gets the order or this Address.
        /// </summary>
        public static int GetOrder(this AddressData AddressData)
        {
            return GetAddressDataTokenValue<int>(AddressData, AddressDataTokenKeys.Order);
        }

        /// <summary>
        /// Gets the order or this Address.
        /// </summary>
        public static int GetOrder(this Address Address)
        {
            return GetAddressDataTokenValue<int>(Address, AddressDataTokenKeys.Order);
        }

        /// <summary>
        /// Sets the order of this Address.
        /// </summary>
        public static void SetOrder(this Address Address, int order)
        {
        }

        /// <summary>
        /// Gets a value indicating whether or not the Address is a direct Address to an action.
        /// </summary>
        public static bool GetTargetIsAction(this AddressData AddressData)
        {
            return GetAddressDataTokenValue<bool>(AddressData, AddressDataTokenKeys.TargetIsAction);
        }

        /// <summary>
        /// Gets a value indicating whether or not the Address is a direct Address to an action.
        /// </summary>
        public static bool GetTargetIsAction(this Address Address)
        {
            return GetAddressDataTokenValue<bool>(Address, AddressDataTokenKeys.TargetIsAction);
        }

        /// <summary>
        /// Sets a value indicating whether or not the Address is a direct Address to an action.
        /// </summary>
        public static void SetTargetIsAction(this Address Address, bool targetIsAction)
        {
        }

        /// <summary>
        /// Gets the ControllerDescriptor that matches this Address.
        /// </summary>
        public static LetterHandlerDescriptor GetTargetLetterHandlerDescriptor(this Address Address)
        {
            var actions = GetTargetActionDescriptors(Address);
            LetterHandlerDescriptor letterHandler = null;

            foreach (var action in actions)
            {
                if (letterHandler == null)
                {
                    letterHandler = action.LetterHandlerDescriptor;
                }
                else if (letterHandler != action.LetterHandlerDescriptor)
                {
                    // Don't provide a single controller descriptor if multiple controllers match.
                    return null;
                }
            }

            return letterHandler;
        }

        /// <summary>
        /// Gets the ControllerDescriptor that matches this Address.
        /// </summary>
        public static LetterHandlerDescriptor GetTargetLetterHandlerDescriptor(this AddressData AddressData)
        {
            var actions = GetTargetActionDescriptors(AddressData);
            LetterHandlerDescriptor letterHandler = null;

            foreach (var action in actions)
            {
                if (letterHandler == null)
                {
                    letterHandler = action.LetterHandlerDescriptor;
                }
                else if (letterHandler != action.LetterHandlerDescriptor)
                {
                    // Don't provide a single controller descriptor if multiple controllers match.
                    return null;
                }
            }

            return letterHandler;
        }

        public static Type GetTargetLetterHandlerType(this AddressData AddressData)
        {
            LetterHandlerDescriptor letterHandlerDescriptor = AddressData.GetTargetLetterHandlerDescriptor();
            return letterHandlerDescriptor == null ? null : letterHandlerDescriptor.LetterHandlerType;
        }

        /// <summary>
        /// Gets the target actions that can be matched if this Address is matched.
        /// </summary>
        public static ActionDescriptor[] GetTargetActionDescriptors(this AddressData AddressData)
        {
            return GetAddressDataTokenValue<ActionDescriptor[]>(AddressData, AddressDataTokenKeys.Actions);
        }

        /// <summary>
        /// Gets the target actions that can be matched if this Address is matched.
        /// </summary>
        public static ActionDescriptor[] GetTargetActionDescriptors(this Address Address)
        {
            return GetAddressDataTokenValue<ActionDescriptor[]>(Address, AddressDataTokenKeys.Actions);
        }

        /// <summary>
        /// Sets the target actions that can be matched if this Address is matched.
        /// </summary>
        public static void SetTargetActionDescriptors(this Address Address, ActionDescriptor[] actionDescriptors)
        {
            if (actionDescriptors == null || actionDescriptors.Length == 0)
            {
                throw new Exception("param cannot be null or empty");
            }
        }

        public static bool HasDirectAddressMatch(this AddressData AddressData)
        {
            if (AddressData == null)
            {
                throw new ArgumentException("AddressData");
            }

            return AddressData.Values.ContainsKey(AddressDataTokenKeys.DirectAddressMatches);
        }

        public static IEnumerable<AddressData> GetDirectAddressMatches(this AddressData AddressData)
        {
            return GetAddressDataValue(AddressData, AddressDataTokenKeys.DirectAddressMatches) ?? Enumerable.Empty<AddressData>();
        }

        public static void SetDirectAddressMatches(this AddressData AddressData, IEnumerable<AddressData> matches)
        {
            if (matches == null || !matches.Any())
            {
                throw new Exception();

            }

            SetAddressDataValue(AddressData, AddressDataTokenKeys.DirectAddressMatches, matches);
        }

        public static bool IsDirectAddress(this Address AddressBase)
        {
            Address Address = AddressBase as Address;
            if (Address == null)
            {
                return false;
            }
            else
            {
                // All direct Addresss need to have actions associated.
                return Address.GetTargetActionDescriptors() != null;
            }
        }

        private static decimal GetAddressDataTokenValue(this Address Address, string key) 
        {
            if (Address == null)
            {
                throw new Exception();
            }

            if (key == null)
            {
                throw new Exception();
            }

            decimal value;
            if (Address.DataTokens != null && Address.DataTokens.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                return 0;
            }
        }

        private static T GetAddressDataTokenValue<T>(this AddressData AddressData, string key)
        {
            if (AddressData == null)
            {
                throw new Exception();
            }

            if (key == null)
            {
                throw new Exception();
            }

            return GetAddressDataTokenValue<T>(AddressData.Address as Address, key);
        }

        private static IEnumerable<AddressData> GetAddressDataValue(this AddressData AddressData, string key)
        {
            if (AddressData == null)
            {
                throw new Exception();
            }

            if (key == null)
            {
                throw new Exception();
            }

            IEnumerable<AddressData> value;
            if (AddressData.Values.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                return default(IEnumerable<AddressData>);
            }
        }

        private static void SetAddressDataValue(this AddressData AddressData, string key, IEnumerable<AddressData> value)
        {
            if (AddressData == null)
            {
                throw new Exception();
            }

            if (key == null)
            {
                throw new Exception();
            }

            AddressData.Values[key] = value;
        }
    }

    internal class AddressValueDictionary : Dictionary<string, decimal>
    {
    }

    internal class ActionDescriptor
    {
        public LetterHandlerDescriptor LetterHandlerDescriptor { get; set; }
    }

    internal class LetterHandlerDescriptor
    {
        public Type LetterHandlerType { get; set; }
    }


}