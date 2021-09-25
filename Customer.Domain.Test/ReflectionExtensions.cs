using System;
using System.Reflection;

namespace Customer.Domain.Test
{
    public static class ReflectionExtensions
    {
        public static void SetPrivatePropertyValue<T>(this object obj, string propName, T val)
        {
            var t = obj.GetType();
            if (t.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
                throw new ArgumentOutOfRangeException(nameof(propName),
                    $"Property {propName} was not found in Type {obj.GetType().FullName}");
            t.InvokeMember(propName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null,
                obj, new object[] { val });
        }

        public static void SetPrivateFieldValue<T>(this object obj, string propName, T val)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            Type t = obj.GetType();
            FieldInfo fi = null;
            while (fi == null && t != null)
            {
                fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                t = t.BaseType;
            }
            if (fi == null) throw new ArgumentOutOfRangeException("propName", string.Format("Field {0} was not found in Type {1}", propName, obj.GetType().FullName));
            fi.SetValue(obj, val);
        }
    }
}