using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityBean {
    public static class BeanContainer {
        private static Dictionary<BeanInfo, List<WiredItem>> allBeans;
        private static Dictionary<string, object> beanMap;

        public static async Task<bool> Initialize(Action<string> onBeanStart,
            Action<string> onBeanSuccess,
            Action<string> onBeanFailed) {
            // get bean info
            var services = GetBeans<Service>();
            var repositories = GetBeans<Repository>();
            var controllers = GetBeans<Controller>();

            allBeans = new Dictionary<BeanInfo, List<WiredItem>>();
            foreach (var bean in services) {
                allBeans.Add(bean.Key, bean.Value);
            }

            foreach (var bean in repositories) {
                allBeans.Add(bean.Key, bean.Value);
            }

            foreach (var bean in controllers) {
                allBeans.Add(bean.Key, bean.Value);
            }

            // build map
            beanMap = new Dictionary<string, object>();
            foreach (var bean in allBeans.Keys) {
                beanMap.Add(bean.name, bean.instance);
            }

            // wire beans
            foreach (var info in allBeans) {
                foreach (var autoWired in info.Value) {
                    var name = autoWired.field.FieldType.Name;
                    beanMap.TryGetValue(name, out object bean);
                    if (bean == null) {
                        Debug.LogError("Can not found bean: " + name + " at the " + info.Key.name);
                        return false;
                    }

                    autoWired.field.SetValue(info.Key.instance, bean);
                }
            }

            // initialize
            foreach (var info in allBeans.Keys) {
                if (info.initialize == null) {
                    continue;
                }
                
                onBeanStart?.Invoke(info.name);
                var success = await (Task<bool>) info.initialize.Invoke(info.instance, null);
                if (success) {
                    onBeanSuccess?.Invoke(info.name);
                }
                else {
                    onBeanFailed?.Invoke(info.name);
                    return false;
                }
            }

            return true;
        }

        public static void DynamicDI(object obj) {
            var autoWiredItems = GetWiredItems<DynamicWired>(obj.GetType());
            foreach (var autoWired in autoWiredItems) {
                var name = autoWired.field.FieldType.Name;
                beanMap.TryGetValue(name, out object bean);
                if (bean == null) {
                    Debug.LogError("Can not found bean: " + name + " at the " + obj.GetType().Name);
                    continue;
                }

                autoWired.field.SetValue(obj, bean);
            }
        }

        private static List<WiredItem> GetWiredItems<T>(Type type) {
            var wiredItems = new List<WiredItem>();
            var allAutoWired =
                from a in type.GetFields(BindingFlags.NonPublic |
                                         BindingFlags.Public |
                                         BindingFlags.Instance)
                    
                let attributes = a.IsDefined(typeof(T), false)
                where attributes 
                select new { Field = a };

            foreach (var autoWired in allAutoWired) {
                wiredItems.Add(new WiredItem(autoWired.Field));
            }

            return wiredItems;
        }
        

        public static T GetBean<T>() {
            beanMap.TryGetValue(typeof(T).Name, out object instance);
            return (T) instance;
        }

        public class BeanInfo {
            public string name { get; }
            public object instance { get; }
            public MethodInfo initialize { get; set; }

            public BeanInfo(string name, object instance) {
                this.name = name;
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

        public static Dictionary<BeanInfo, List<WiredItem>> GetBeans<T>() where T : Attribute {
            var res = new Dictionary<BeanInfo, List<WiredItem>>();

            var beans =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                let attributes = t.IsDefined(typeof(T), false)
                where attributes
                select new {Type = t};

            foreach (var bean in beans) {
                var obj = MakeSingletonInstance(bean.Type);
                var autoWiredItems = GetWiredItems<AutoWired>(bean.Type);
                var key = new BeanInfo(bean.Type.FullName, obj);
                res.Add(key, autoWiredItems);

                foreach (var method in bean.Type.GetMethods()) {
                    if (method.Name == "Initialize") {
                        key.initialize = method;
                    }
                }
            }

            return res;
        }

        private static object MakeSingletonInstance(Type t) {
            try {
                return t.GetConstructor(new Type[] { })?.Invoke(new object[] { });
            }
            catch {
                return null;
            }
        }
    }
}