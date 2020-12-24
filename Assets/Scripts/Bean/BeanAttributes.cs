using System;

namespace UnityBean {
    [AttributeUsage(AttributeTargets.Class)]  
    public class Service : Attribute {
        public string caseName { get; }

        public Service() {
            this.caseName = this.TypeId.ToString();
        }
    }

    [AttributeUsage(AttributeTargets.Class)]  
    public class Controller : Attribute {
        public string caseName { get; }

        public Controller() {
            this.caseName = this.TypeId.ToString();
        }
    }

    [AttributeUsage(AttributeTargets.Class)]  
    public class Repository : Attribute {
        public string caseName { get; }

        public Repository() {
            this.caseName = this.TypeId.ToString();
        }
    }
    
    [AttributeUsage(AttributeTargets.Field)]  
    public class AutoWired : Attribute {
        public AutoWired() {
        }
    }
    
    [AttributeUsage(AttributeTargets.Field)]  
    public class DynamicWired : Attribute {
        public DynamicWired() {
        }
    }    
}
