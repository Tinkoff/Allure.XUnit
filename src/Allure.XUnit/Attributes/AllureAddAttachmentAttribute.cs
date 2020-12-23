using System;
using Allure.Commons;

namespace Allure.Xunit.Attributes
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple = true)]
    public class AllureAddAttachmentAttribute: Attribute, IAllureInfo
    {
       
        public AllureAddAttachmentAttribute(string path,string name = null)
        {
            AllureLifecycle.Instance.AddAttachment(path, name);
        }
        
        public AllureAddAttachmentAttribute(string name, string type,string path )
        {
            AllureLifecycle.Instance.AddAttachment(name, type, path);
        }
        
        public AllureAddAttachmentAttribute(Type type)
        {
            if (!typeof(IAllureGetAttachmentPath).IsAssignableFrom(type))
                throw  new ArgumentException($"Instance of parameter AllureAddAttachmentAttribute has to implement{nameof(IAllureGetAttachmentPath)}");
           
            var instance =(IAllureGetAttachmentPath)Activator.CreateInstance(type);
            
            if(instance is null)
                throw  new ArgumentException("Can't create instance of parameter AllureAddAttachmentAttribute");

            AllureLifecycle.Instance.AddAttachment(instance.GetAttachmentPath());
        }
    }
}
