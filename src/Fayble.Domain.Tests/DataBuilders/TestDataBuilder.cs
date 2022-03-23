using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Fayble.Domain.Tests.DataBuilders;

public abstract class TestDataBuilder<TObject>
{
    readonly IDictionary<string, object> _properties = new Dictionary<string, object>();

    protected void Set<TValue>(Expression<Func<TObject, TValue>> property, TValue value)
    {
        _properties[GetPropertyName(property)] = value;
    }

    protected TValue Get<TValue>(Expression<Func<TObject, TValue>> property)
    {
        var propertyName = GetPropertyName(property);

        if (!_properties.TryGetValue(propertyName, out var value))
        {
            throw new InvalidOperationException($"Property {propertyName} was not set.");
        }

        return (TValue)value;
    }

    public abstract TObject Build();

    protected static void SetProperty(TObject item, string propertyName, object value)
    {
        PropertyInfo prop = item.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        if (null != prop && prop.CanWrite)
        {
            prop.SetValue(item, value, null);
        }
    }

    private static string GetPropertyName<TValue>(Expression<Func<TObject, TValue>> property)
    {
        var type = typeof(TObject);

        if (!(property.Body is MemberExpression member))
        {
            throw new ArgumentException($"Expression '{property}' refers to a method, not a property.");
        }

        var propInfo = member.Member as PropertyInfo;
        if (propInfo == null)
        {
            throw new ArgumentException($"Expression '{property}' refers to a field, not a property.");
        }

        if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType ?? throw new InvalidOperationException($"ReflectedType for prop {propInfo.Name} is null")))
        {
            throw new ArgumentException(
                string.Format($"Expression '{property}' refers to a property that is not from type {type}."));
        }

        return propInfo.Name;
    }
}