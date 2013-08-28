namespace Orc.FluentTypeFilter
{
    /// <summary>
    /// Static class for TypeFilter
    /// </summary>
    public static class TypeFilter
    {
        /// <summary>
        /// Returns the PeropertyFilter for a given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static PropertyFilter<T> Properties<T>()
        {
            return new PropertyFilter<T>();
        }

    }
}