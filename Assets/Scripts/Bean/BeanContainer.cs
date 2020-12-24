using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityBean {
    public static class BeanContainer {
        private static Dictionary<BeanInfo, List<AutoWiredItem>> allBeans;
        private static Dictionary<string, object> beanMap;

        public static async Task<bool> Initialize(Action<string> onBeanStart,
            Action<string> onBeanSuccess,
            Action<string> onBeanFailed) {
            // get bean info
            var components = GetBeans<Component>();
            var services = GetBeans<Service>();
            var repositories = GetBeans<Repository>();
            var controllers = GetBeans<Controller>();

            allBeans = new Dictionary<BeanInfo, List<AutoWiredItem>>();
            foreach (var bean in components) {
                allBeans.Add(bean.Key, bean.Value);
            }

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

        public class AutoWiredItem {
            public FieldInfo field { get; }

            public AutoWiredItem(FieldInfo field) {
                this.field = field;
            }
        }

        public static Dictionary<BeanInfo, List<AutoWiredItem>> GetBeans<T>() where T : Attribute {
            var res = new Dictionary<BeanInfo, List<AutoWiredItem>>();

            // get all test suites
            var beans =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(T), true)
                where attributes != null && attributes.Length > 0
                select new {Type = t, Attributes = attributes.Cast<T>()};

            foreach (var bean in beans) {
                var obj = MakeSingletonInstance(bean.Type);
                var tests = new List<AutoWiredItem>();

                var key = new BeanInfo(bean.Type.FullName, obj);
                res.Add(key, tests);

                var allAutoWired =
                    from a in bean.Type.GetFields(BindingFlags.NonPublic |
                                                  BindingFlags.Public |
                                                  BindingFlags.Instance)
                    let attributes = a.GetCustomAttributes(typeof(AutoWired), true)
                    where attributes != null && attributes.Length > 0
                    select new {Type = a, Field = a, Attributes = attributes.Cast<AutoWired>()};

                foreach (var autoWired in allAutoWired) {
                    tests.Add(new AutoWiredItem(autoWired.Field));
                }

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