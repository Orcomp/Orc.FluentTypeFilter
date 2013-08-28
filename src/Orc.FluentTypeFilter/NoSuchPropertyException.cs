using System;
using System.Runtime.Serialization;

namespace Orc.FluentTypeFilter
{
    using System.Reflection;

    [Serializable]
    public class NoSuchPropertyException : Exception
    {
        public NoSuchPropertyException(Type declaringType, string propertyName)
            : base(String.Format("Type {0} has no property named {1}", declaringType, propertyName))
        {
        }

        public NoSuchPropertyException(Type declaringType, Type propertyType)
            : base(String.Format("Type {0} has no property of type {1}", declaringType, propertyType.Name))
        {
        }

        public NoSuchPropertyException(Type declaringType, BindingFlags accessModifier)
            : base(String.Format("Type {0} has no properties with access modifier {1}", declaringType, accessModifier))
        {
            
        }

        public NoSuchPropertyException()
        {
        }

        public NoSuchPropertyException(string message)
            : base(message)
        {
        }

        public NoSuchPropertyException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected NoSuchPropertyException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}