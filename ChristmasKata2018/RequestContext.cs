using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ChristmasKata2018
{
    /// <summary>
    /// When making a request of santa lots of data is captured about the nature of the request
    /// and the properties / metadata of the caller. These pieces of information are used in order to
    /// help ascertain what tier of presents - if any at all - are appropriate. 
    /// </summary>
    public class RequestContext
    {
        public AddressData AddressData { get; set; }
        public HttpContext HttpContext { get; set; }
    }

    public class HttpContext
    {
        public Request Request { get; set; }
    }

    public class Request
    {
        public string Path { get; set; }
    }

    public class AddressData
    {
        public Dictionary<string, IEnumerable<AddressData>> Values { get; private set; }
        public Dictionary<string, object> DataTokens { get; set; }
        public Address Address { get; set; }

    
    }

    public class Address
    {
        public object Url { get; set; }
        public Dictionary<string, decimal> DataTokens { get; set; }
        
        public static implicit operator AddressData(Address d)
        {
            return new AddressData();
        }
    }


    public class DataToken
    {
    }
}