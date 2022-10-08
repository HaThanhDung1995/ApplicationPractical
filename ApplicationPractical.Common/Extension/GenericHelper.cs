using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationPractical.Common.Extension
{
    public static class GenericHelper
    {
        public static To PopulateWith<To, From>(this To to, From from)
        {
            Func<PropertyInfo, From, bool> predicate = (p, s) => typeof(From).HasProperty(p.Name);

            foreach (var propertyInfo in typeof(To).GetProperties().Where(prop => prop.CanRead && prop.CanWrite))
            {
                if (predicate(propertyInfo, from))
                {
                    try
                    {
                        var output = TryGetProperty(from, propertyInfo.Name);
                        var val = output.GetValue(from);
                        propertyInfo.SetValue(to, val);
                    }
                    catch
                    {
                    }
                    // var val = from.TryGetValue<>(propertyInfo.Name);
                }
            }

            return to;
        }

        public static To PopulateIgnoreEmptyWith<To, From>(this To to, From from)
        {
            Func<PropertyInfo, From, bool> predicate = (p, s) => typeof(From).HasProperty(p.Name);

            foreach (var propertyInfo in typeof(To).GetProperties().Where(prop => prop.CanRead && prop.CanWrite))
            {
                if (predicate(propertyInfo, from))
                {
                    try
                    {
                        var output = TryGetProperty(from, propertyInfo.Name);
                        var valDest = output.GetValue(from);
                        if (valDest == null) continue;
                        if (propertyInfo.PropertyType == typeof(string) && string.IsNullOrEmpty(valDest.ToString())) continue;

                        propertyInfo.SetValue(to, valDest);
                    }
                    catch
                    {
                    }

                }
            }

            return to;
        }

        public static To PopulateWithoutNullValue<To, From>(this To to, From from)
        {
            Func<PropertyInfo, From, bool> predicate = (p, s) => typeof(From).HasProperty(p.Name);

            foreach (var propertyInfo in typeof(To).GetProperties().Where(prop => prop.CanRead && prop.CanWrite))
            {
                if (predicate(propertyInfo, from))
                {
                    try
                    {
                        var output = TryGetProperty(from, propertyInfo.Name);
                        var valDest = output.GetValue(from);
                        propertyInfo.SetValue(to, valDest);
                    }
                    catch
                    {
                    }
                }
            }

            return to;
        }
        public static bool HasProperty(this Type type, string propName)
        {
            return GetProperty(type, propName) != null;
        }
        public static PropertyInfo TryGetProperty<T>(this T obj, string propName)
        {
            return GetProperty(obj.GetType(), propName);
        }
        static PropertyInfo GetProperty(Type type, string propName)
        {
            var output = type is Type ? type : type.GetType();
            return output.GetProperties().FirstOrDefault(n => n.Name.Equals(propName));
        }
        public static U TryGetValue<U>(this object obj, string propName)
        {
            try
            {
                var output = TryGetProperty(obj, propName);
                var val = output.GetValue(obj);
                return (U)val;
            }
            catch
            {
                return default(U);
            }
        }

        public static object TryGetValue(this object obj, string propName)
        {
            try
            {
                var output = TryGetProperty(obj, propName);
                var val = output.GetValue(obj);
                return val;
            }
            catch
            {
                return default;
            }
        }

        public static bool TrySetValue<T>(this T obj, string propName, object value)
        {
            try
            {
                var output = obj.GetType().GetProperties().FirstOrDefault(e => e.Name.Equals(propName));
                if (output != null)
                {
                    output.SetValue(obj, value);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static T SetDefaultString<T>(this T obj)
        {
            var props = obj.GetType().GetProperties().Where(n => n.CanWrite && n.PropertyType == typeof(string));
            if (props.Count() > 0)
            {
                props.ToList().ForEach(p =>
                {
                    var value = obj.TryGetValue<string>(p.Name);
                    if (string.IsNullOrEmpty(value))
                        value = "";

                    obj.TrySetValue(p.Name, value.Trim());
                });
            }
            return obj;
        }

        public static async Task<T> ReadAsAsync<T>(this System.Net.Http.HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }

        public static T ReadAs<T>(this System.Net.Http.HttpResponseMessage response)
        {
            var json = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }
    }

    //public static class IMappingOperationOptionsExtensions
    //{
    //    public static void ExcludeMembers(this AutoMapper.IMappingOperationOptions options, params string[] members)
    //    {
    //        options.Items[MappingProfile.MemberExclusionKey] = members;
    //    }

    //    public static TDestination Map<TSource, TDestination>(this TDestination destination, TSource source, IMapper _mapper)
    //    {
    //        return _mapper.Map(source, destination);
    //    }
    //}
}
