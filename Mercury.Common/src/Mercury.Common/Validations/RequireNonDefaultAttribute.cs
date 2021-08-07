using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

namespace Mercury.Common
{
    /// <summary>
    /// Override of <see cref="ValidationAttribute.IsValid(object)"/>
    /// </summary>
    /// <remarks>Is meant for use with primitive types, structs (like DateTime, Guid), or enums. Specifically ignores null values (considers them valid) so that this can be combined with RequiredAttribute.</remarks>
    /// <example>
    /// //Allows you to effectively mark the field as required with out having to resort to Guid? and then having to deal with SomeId.GetValueOrDefault() everywhere (and then test for Guid.Empty)
    /// [RequireNonDefault] 
    /// public Guid SomeId { get; set;}
    /// 
    /// //Enforces validation that requires the field cannot be 0
    /// [RequireNonDefault] 
    /// public int SomeId { get; set; }
    /// 
    /// //The nullable int lets the field be optional, but if it IS provided, it can't be 0
    /// [RequireNonDefault]
    /// public int? Age { get; set;}
    /// 
    /// //Forces a value other than the default Enum, so `Unspecified` is not allowd
    /// [RequireNonDefault]
    /// public Fruit Favourite { get; set; }
    /// public enum Fruit { Unspecified, Apple, Banana }
    /// </example>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class RequireNonDefaultAttribute : ValidationAttribute
    {
        private static ConcurrentDictionary<string, object> defaultInstancesCache = new ConcurrentDictionary<string, object>();

        public RequireNonDefaultAttribute()
            : base("The {0} field requires a non-default value.")
        {
        }

        /// <param name="value">The value to test</param>
        /// <returns><c>false</c> if the <paramref name="value"/> is equal the default value of an instance of its own type.</returns>
        public override bool IsValid(object value)
        {
            if (value is null)
                return true; //Only meant to test default values. Use `System.ComponentModel.DataAnnotations.RequiredAttribute` to consider NULL invalid

            var type = value.GetType();
            if (!defaultInstancesCache.TryGetValue(type.FullName, out var defaultInstance))
            {
                //Helps to avoid repeat overhead of reflection for any given type (FullName includes full namespace, so something like System.Int32, System.Decimal, System.Guid, etc)
                defaultInstance = Activator.CreateInstance(Nullable.GetUnderlyingType(type) ?? type);
                defaultInstancesCache[type.FullName] = defaultInstance;
            }
            return !Equals(value, defaultInstance);
        }
    }
}