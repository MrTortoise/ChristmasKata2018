using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Principal;

namespace ChristmasKata2018.SeventhCircleOfChristmas { 
    // from ms docs 

  
    public abstract class HttpContextBase : IServiceProvider {
        public virtual Exception[] AllErrors {
            get
            {
                throw new NotImplementedException();
            }
        } 
  
        public virtual HttpApplicationStateBase Application {
            get { 
                throw new NotImplementedException();
            }
        }
  
        public virtual HttpApplication ApplicationInstance {
            get { 
                throw new NotImplementedException(); 
            }
            set { 
                throw new NotImplementedException();
            }
        }
  
        public virtual Cache Cache {
            get { 
                throw new NotImplementedException(); 
            }
        } 
 
        public virtual IHttpHandler CurrentHandler {
            get {
                throw new NotImplementedException(); 
            }
        } 
  
        public virtual RequestNotification CurrentNotification {
            get { 
                throw new NotImplementedException();
            }
        }
  
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error",
            Justification = "Matches HttpContext class")] 
        public virtual Exception Error { 
            get {
                throw new NotImplementedException(); 
            }
        }
 
        public virtual IHttpHandler Handler { 
            get {
                throw new NotImplementedException(); 
            } 
            set {
                throw new NotImplementedException(); 
            }
        }
 
        public virtual bool IsCustomErrorEnabled { 
            get {
                throw new NotImplementedException(); 
            } 
        }
  
        public virtual bool IsDebuggingEnabled {
            get {
                throw new NotImplementedException();
            } 
        }
  
        public virtual bool IsPostNotification { 
            get {
                throw new NotImplementedException(); 
            }
        }
 
        public virtual IDictionary Items { 
            get {
                throw new NotImplementedException(); 
            } 
        }
  
        public virtual IHttpHandler PreviousHandler {
            get {
                throw new NotImplementedException();
            } 
        }
  
        public virtual ProfileBase Profile { 
            get {
                throw new NotImplementedException(); 
            }
        }
 
        public virtual HttpRequestBase Request { 
            get {
                throw new NotImplementedException(); 
            } 
        }
  
        public virtual HttpResponseBase Response {
            get {
                throw new NotImplementedException();
            } 
        }
  
        public virtual HttpServerUtilityBase Server { 
            get {
                throw new NotImplementedException(); 
            }
        }
 
        public virtual HttpSessionStateBase Session { 
            get {
                throw new NotImplementedException(); 
            } 
        }
  
        public virtual bool SkipAuthorization {
            get {
                throw new NotImplementedException();
            } 
            set {
                throw new NotImplementedException(); 
            } 
        }
  
        public virtual DateTime Timestamp {
            get {
                throw new NotImplementedException();
            } 
        }
  
        public virtual TraceContext Trace { 
            get {
                throw new NotImplementedException(); 
            }
        }
 
        public virtual IPrincipal User { 
            get {
                throw new NotImplementedException(); 
            } 
            set {
                throw new NotImplementedException(); 
            }
        }
 
        public virtual void AddError(Exception errorInfo) { 
            throw new NotImplementedException();
        } 
  
        public virtual void ClearError() {
            throw new NotImplementedException(); 
        }
 
        public virtual object GetGlobalResourceObject(string classKey, string resourceKey) {
            throw new NotImplementedException(); 
        }
  
        public virtual object GetGlobalResourceObject(string classKey, string resourceKey, CultureInfo culture) { 
            throw new NotImplementedException();
        } 
 
        public virtual object GetLocalResourceObject(string virtualPath, string resourceKey) {
            throw new NotImplementedException();
        } 
 
        public virtual object GetLocalResourceObject(string virtualPath, string resourceKey, CultureInfo culture) { 
            throw new NotImplementedException(); 
        }
  
        public virtual object GetSection(string sectionName) {
            throw new NotImplementedException();
        }
  
        public virtual void RemapHandler(IHttpHandler handler) {
            throw new NotImplementedException(); 
        } 
 
        [SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", 
            Justification = "Matches HttpContext class")]
        public virtual void RewritePath(string path) {
            throw new NotImplementedException();
        } 
 
        [SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", 
            Justification = "Matches HttpContext class")] 
        public virtual void RewritePath(string path, bool rebaseClientPath) {
            throw new NotImplementedException(); 
        }
 
        [SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters",
            Justification = "Matches HttpContext class")] 
        public virtual void RewritePath(string filePath, string pathInfo, string queryString) {
            throw new NotImplementedException(); 
        } 
 
        [SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", 
            Justification = "Matches HttpContext class")]
        public virtual void RewritePath(string filePath, string pathInfo, string queryString, bool setClientFilePath) {
            throw new NotImplementedException();
        } 
 
        public virtual void SetSessionStateBehavior(SessionStateBehavior sessionStateBehavior) { 
            throw new NotImplementedException(); 
        }
  
        #region IServiceProvider Members
        [SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase")]
        public virtual object GetService(Type serviceType) {
            throw new NotImplementedException(); 
        }
        #endregion 
    }

    public class TraceContext
    {
    }

    public class HttpSessionStateBase
    {
    }

    public class HttpServerUtilityBase
    {
    }

    public class HttpResponseBase
    {
    }

    public class HttpRequestBase
    {
        public object Path { get; set; }
    }

    public class ProfileBase
    {
    }

    public class RequestNotification
    {
    }

    public interface IHttpHandler
    {
    }

    public class Cache
    {
    }

    public class HttpApplication
    {
    }

    public class HttpApplicationStateBase
    {
    }
}
 

