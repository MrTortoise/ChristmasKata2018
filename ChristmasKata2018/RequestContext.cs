using System.Collections.Generic;

namespace ChristmasKata2018
{
    /// <summary>
    /// When making a request of santa lots of data is captured about the nature of the request
    /// and the properties / metadata of the caller. These pieces of information are used in order to
    /// help ascertain what tier of presents - if any at all - are appropriate. 
    /// </summary>
    public class RequestContext
    {
        public RequestContext()
        {
            
        }

        public RequestContext(HttpContextBase httpContext, AddressData addressData)
        {
            HttpContext = httpContext as HttpContext;
            AddressData = addressData;
        }
        
        public AddressData AddressData { get; }
        public HttpContext HttpContext { get; }
    }


    public class HttpContext  : HttpContextBase
    {
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
}