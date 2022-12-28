using System;
using System.Reflection;

namespace UnityBean {
    public class BeanInfo {
        public Type type { get; }
        public Type baseType { get; }
        public string name { get; }
        public object instance { get; }
        public MethodInfo initialize { get; set; }
        public MethodInfo postInitialize { get; set; }

        public BeanInfo(Type type, object instance) {
            this.type = type;
            this.name = type.Name;
            this.instance = instance;
        }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(BeanInfo)) {
                return false;
            }

            return ((BeanInfo) obj).name == name;
        }

        public override int GetHashCode() {
            return name.GetHashCode();
        }
    }   
    
    public class WiredItem {
        public FieldInfo field { get; }

        public WiredItem(FieldInfo field) {
            this.field = field;
        }
    }
}