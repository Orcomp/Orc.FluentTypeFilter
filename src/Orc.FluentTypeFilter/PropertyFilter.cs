using System.Reflection;

namespace Orc.FluentTypeFilter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Fasterflect;

    /// <summary>
    /// Base class for this library
    /// </summary>
    public class PropertyFilter<T>
    {
        private readonly Dictionary<string, PropertyInfo> _properties = new Dictionary<string, PropertyInfo>();

        private static Type TypeConstraint
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// Thread-safe copy of the dictionary
        /// </summary>
        private Dictionary<string, PropertyInfo> CopyProperties
        {
            get
            {
                lock (_properties)
                {
                    return new Dictionary<string, PropertyInfo>(_properties);
                }
            }
        }

        /// <summary>
        /// Thread-safe retrieval of the property names in this set
        /// </summary>
        public IEnumerable<string> Names
        {
            get { return CopyProperties.Keys; }
        }

        #region Protected methods

        /// <summary>
        /// <para>Add a property to the set</para>
        /// <para>If it already exists, it will be overwritten</para>
        /// </summary>
        /// <exception cref="NoSuchPropertyException"/>
        public PropertyFilter<T> Include(Expression<Func<T, object>> keySelector)
        {
            // Get or throw exception
            var property = GetProperty(keySelector);

            // Add
            Add(property);

            return this;
        }

        /// <summary>
        /// <para>Remove a property from the set</para>
        /// <para>If it already exists, it will be overwritten</para>
        /// </summary>
        /// <exception cref="NoSuchPropertyException"/>
        public PropertyFilter<T> Exclude(Expression<Func<T, object>> keySelector)
        {
            // Get or throw exception
            var property = GetProperty(keySelector);

            // Remove
            Remove(property);

            return this;
        }

        /// <summary>
        /// Add all properties to the set
        /// </summary>
        /// <exception cref="NoSuchPropertyException"/>
        public PropertyFilter<T> IncludeAll(BindingFlags bindingFlags = BindingFlags.Default,
                                                bool throwIfNotExists = true)
        {
            // Get or throw if necessary
            var properties = GetOrThrow(bindingFlags, throwIfNotExists);

            // Add or overwrite all
            foreach (var propertyInfo in properties)
            {
                Add(propertyInfo);
            }

            return this;
        }

        /// <summary>
        /// Add properties to the set by type
        /// </summary>
        /// <exception cref="NoSuchPropertyException"/>
        public PropertyFilter<T> IncludeAll(Type propertyType,
                                                BindingFlags bindingFlags = BindingFlags.Default,
                                                bool throwIfNotExists = true)
        {
            // Get or throw if necessary
            var properties = GetOrThrow(propertyType, bindingFlags, throwIfNotExists);

            // Add or overwrite all
            foreach (var propertyInfo in properties)
            {
                Add(propertyInfo);
            }

            return this;
        }

        /// <summary>
        /// Add properties to the set by type, generic
        /// </summary>
        /// <exception cref="NoSuchPropertyException"/>
        public PropertyFilter<T> IncludeAll<K>(BindingFlags bindingFlags = BindingFlags.Default,
                                                    bool throwIfNotExists = true)
        {
            IncludeAll(typeof(K), bindingFlags, throwIfNotExists);
            return this;
        }

        /// <summary>
        /// Remove all properties from the set
        /// </summary>
        /// <exception cref="NoSuchPropertyException"/>
        public PropertyFilter<T> ExcludeAll(BindingFlags bindingFlags = BindingFlags.Default,
                                   bool throwIfNotExists = true)
        {
            // Get or throw if necessary
            var properties = GetOrThrow(bindingFlags, throwIfNotExists);

            // Remove all if exists
            foreach (var propertyInfo in properties)
            {
                Remove(propertyInfo);
            }

            return this;
        }

        /// <summary>
        /// Remove properties from the set by type
        /// </summary>
        /// <exception cref="NoSuchPropertyException"/>
        public PropertyFilter<T> ExcludeAll(Type propertyType,
                                   BindingFlags bindingFlags = BindingFlags.Default,
                                   bool throwIfNotExists = true)
        {
            // Get or throw if necessary
            var properties = GetOrThrow(propertyType, bindingFlags, throwIfNotExists);

            // Remove all if exists
            foreach (var propertyInfo in properties)
            {
                Remove(propertyInfo);
            }

            return this;
        }

        /// <summary>
        /// Remove properties from the set by type, generic
        /// </summary>
        /// <exception cref="NoSuchPropertyException"/>
        public PropertyFilter<T> ExcludeAll<K>(BindingFlags bindingFlags = BindingFlags.Default,
                                       bool throwIfNotExists = true)
        {
            return ExcludeAll(typeof(K), bindingFlags, throwIfNotExists);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Lock and add or overwrite
        /// </summary>
        private void Add(PropertyInfo property)
        {
            lock (_properties)
            {
                _properties[property.Name] = property;
            }
        }

        /// <summary>
        /// Lock and remove if exists
        /// </summary>
        private void Remove(PropertyInfo property)
        {
            lock (_properties)
            {
                var name = property.Name;
                if (!_properties.ContainsKey(name))
                {
                    return;
                }

                _properties.Remove(name);
            }
        }

        private IEnumerable<PropertyInfo> GetProperties(Type propertyType, BindingFlags bindingFlags)
        {
            IEnumerable<PropertyInfo> properties;

            if (bindingFlags == BindingFlags.Default)
            {
                properties = TypeConstraint.Properties().Where(x => x.PropertyType == propertyType);
            }
            else
            {
                properties = TypeConstraint.Properties(bindingFlags).Where(x => x.PropertyType == propertyType);
            }
            
            return properties;
        }

        private IEnumerable<PropertyInfo> GetProperties(BindingFlags bindingFlags)
        {
            if (bindingFlags == BindingFlags.Default)
            {
                return TypeConstraint.Properties();
            }

            return TypeConstraint.Properties(bindingFlags);
        }

        /// <summary>
        /// Get a property using expression, or throw exception
        /// </summary>
        /// <exception cref="NoSuchPropertyException"/>
        private PropertyInfo GetProperty(Expression<Func<T, object>> keySelector)
        {
            var body = keySelector.Body as UnaryExpression;
            var propertyName = body != null
                                   ? ((MemberExpression)body.Operand).Member.Name
                                   : ((MemberExpression)keySelector.Body).Member.Name;

            return GetProperty(propertyName);
        }

        /// <summary>
        /// Get a property by name, or throw exception
        /// </summary>
        /// <exception cref="NoSuchPropertyException"/>
        private PropertyInfo GetProperty(string propertyName)
        {
            var property = TypeConstraint.Property(propertyName);
            
            if (property == null)
            {
                throw new NoSuchPropertyException(TypeConstraint, propertyName);
            }

            return property;
        }

        /// <summary>
        /// Get by type or throw if necessary
        /// </summary>
        /// <exception cref="NoSuchPropertyException"></exception>
        private IEnumerable<PropertyInfo> GetOrThrow(Type propertyType, BindingFlags bindingFlags, bool throwIfNotExists)
        {
            // Get
            var properties = this.GetProperties(propertyType, bindingFlags).ToList();

            // Throw if necessary
            if (throwIfNotExists && properties.Count == 0)
            {
                throw new NoSuchPropertyException(TypeConstraint, propertyType);
            }

            return properties;
        }

        /// <summary>
        /// Get all or throw if necessary
        /// </summary>
        /// <exception cref="NoSuchPropertyException"></exception>
        private IEnumerable<PropertyInfo> GetOrThrow(BindingFlags bindingFlags, bool throwIfNotExists)
        {
            // Get
            var propertyInfos = GetProperties(bindingFlags).ToList();

            // Throw if necessary
            if (throwIfNotExists && propertyInfos.Count == 0)
            {
                throw new NoSuchPropertyException(TypeConstraint, bindingFlags);
            }

            return propertyInfos;
        }

        #endregion
    }
}
