namespace Orc.FluentTypeFilter.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestManyProperties
    {
        private PropertyFilter<ManyProperties> _manyProperties;

        [TestInitialize]
        public void Initialize()
        {
            _manyProperties = new PropertyFilter<ManyProperties>();
        }

        [TestMethod]
        public void IncludeAll()
        {
            // Add all
            _manyProperties.IncludeAll(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            
            // Should now include all properties available
            Assert.IsTrue(this.SetIncludes(this.AllPropertyNames())); 
        }

        [TestMethod]
        public void ExcludeAll()
        {
            // Add all
            IncludeAll();

            // Remove all
            _manyProperties.ExcludeAll(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            // Should now be empty
            Assert.IsTrue(this.SetIsEmpty());
        }

        [TestMethod]
        public void IncludeAllPublic()
        {
            // Add all public
            _manyProperties.IncludeAll(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            // Should now include all properties available except the private ones
            Assert.IsTrue(SetIncludes(AllPropertyNamesExcept(this.PrivatePropertyNames()))); 
        }

        [TestMethod]
        public void RemoveAllIntegers()
        {
            // Add all
            IncludeAll();

            // Remove all ints
            _manyProperties.ExcludeAll<int>(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            // Verify all ints are gone and the rest still exists
            Assert.IsTrue(SetIncludes(AllPropertyNamesExcept("Int1", "Int2", "Int3"))); 
        }

        [TestMethod]
        public void RemoveAllPrivateIntegers()
        {
            // Add all, then remove private ints
            _manyProperties
                .IncludeAll(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                .ExcludeAll<int>(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            // Verify all private ints are gone and the rest still exists
            Assert.IsTrue(SetIncludes(AllPropertyNamesExcept("Int2", "Int3"))); 
        }

        [TestMethod]
        public void RemoveAllIntegersAndTimeSpans()
        {
            const BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

            // Add all
            IncludeAll();

            // Remove all ints and timespans
            _manyProperties.ExcludeAll<int>(Flags).ExcludeAll<TimeSpan>(Flags);

            // Verify all ints are gone and the rest still exists
            Assert.IsTrue(SetIncludes(AllPropertyNamesExcept("Int1", "Int2", "Int3", "TimeSpan1", "TimeSpan2", "TimeSpan3")));
        }

        [TestMethod]
        public void AddAllStaticDateTimes()
        {
            // Add all static DateTimes
            _manyProperties.IncludeAll<DateTime>(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

            // Verify they, and only they, were added
            Assert.IsTrue(SetIncludesOnly("DateTime2", "DateTime3")); 
        }

        [TestMethod]
        public void AddAllPublicStaticDateTimes()
        {
            // Add only the public static DateTimes
            _manyProperties.IncludeAll<DateTime>(BindingFlags.Public | BindingFlags.Static);

            // Verify it, and only it, was added
            Assert.IsTrue(SetIncludesOnly("DateTime2"));
        }

        [TestMethod]
        public void IncludeAll_DateTimeAndInt_Then_ExcludePrivate()
        {
            const BindingFlags FlagsAll = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
            const BindingFlags FlagsPriv = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            // Add all DTs and ints, then remove privates
            _manyProperties
                .IncludeAll<DateTime>(FlagsAll)
                .IncludeAll<int>(FlagsAll)
                .ExcludeAll(FlagsPriv);

            // Should now include all public DTs and ints
            Assert.IsTrue(SetIncludesOnly("Int1", "DateTime1", "DateTime2"));
        }

        [TestMethod]
        public void IncludeAll_Public_Then_ExcludeString1()
        {
            const BindingFlags FlagsPub = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

            // Add all public, then remove String1
            _manyProperties
                .IncludeAll(FlagsPub)
                .Exclude(x => x.String1);

            // Should now include all public except String1
            Assert.IsTrue(SetIncludesOnly(AllPropertyNamesExcept("String1", "Int2", "Int3", "DateTime3")));
        }

        #region Helpers

        private string[] AllPropertyNames()
        {
            return new[]
                       {
                           "Int1", "Int2", "Int3", "String1", "String2", "String3", "DateTime1", "DateTime2", "DateTime3",
                           "TimeSpan1", "TimeSpan2", "TimeSpan3"
                       };
        }

        private string[] PrivatePropertyNames()
        {
            return new[] { "Int2", "Int3", "DateTime3", };
        }

        private string[] AllPropertyNamesExcept(params string[] propertyNames)
        {
            return this.AllPropertyNames().Except(propertyNames).ToArray();
        }

        private bool SetIncludes(params string[] propertyNames)
        {
            return propertyNames.All(propertyName => this._manyProperties.Names.Contains(propertyName));
        }

        private bool SetDoesNotInclude(params string[] propertyNames)
        {
            return propertyNames.All(propertyName => !this._manyProperties.Names.Contains(propertyName));
        }

        private bool SetIncludesOnly(params string[] propertyNames)
        {
            var includesOk = SetIncludes(propertyNames);
            var excludesOk = SetDoesNotInclude(AllPropertyNamesExcept(propertyNames));
            return includesOk && excludesOk;
        }

        private bool SetIsEmpty()
        {
            return !this._manyProperties.Names.Any();
        }

        #endregion
    }
}