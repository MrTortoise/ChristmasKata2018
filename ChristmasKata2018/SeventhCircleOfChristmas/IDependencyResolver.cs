// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace ChristmasKata2018.SeventhCircleOfChristmas
{
    public interface IDependencyResolver
    {
        object GetService(Type serviceType);
        T GetService<T>();
        IEnumerable<object> GetServices(Type serviceType);
    }
}