namespace Orc.FluentTypeFilter.Tests
{
    using System.Linq;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Tests_AccessMods
    {
        private PropertyFilter<IntsOnePrivate> _intsOnePrivate;
        private PropertyFilter<IntsOneStatic> _intsOneStatic;

        [TestInitialize]
        public void Init()
        {
            _intsOnePrivate = new PropertyFilter<IntsOnePrivate>();
            _intsOneStatic = new PropertyFilter<IntsOneStatic>();
        }

        [TestMethod]
        public void Private_Included()
        {
            // Add all int properties
            _intsOnePrivate.IncludeAll<int>(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            // Verify
            var expectedMembers = new[] { "Int1", "Int2" };
            foreach (var s in expectedMembers)
            {
                Assert.IsTrue(_intsOnePrivate.Names.Any(x => x.Equals(s)), 
                    "Did not contain " + s);
            }
        }

        [TestMethod]
        public void Private_Excluded()
        {
            // Add all int properties
            _intsOnePrivate.IncludeAll<int>(BindingFlags.Instance | BindingFlags.Public);

            // Verify
            Assert.IsFalse(_intsOnePrivate.Names.Any(x => x.Equals("Int2")),
                           "Contained Int2 even though it's private");
        }

        [TestMethod]
        public void Static_Included()
        {
            // Add all int properties
            _intsOneStatic.IncludeAll<int>(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            // Verify
            var expectedMembers = new[] { "Int1", "Int2" };
            foreach (var s in expectedMembers)
            {
                Assert.IsTrue(_intsOneStatic.Names.Any(x => x.Equals(s)), 
                    "Did not contain " + s);
            }
        }

        [TestMethod]
        public void Static_Excluded()
        {
            // Add all int properties
            _intsOneStatic.IncludeAll<int>(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            // Verify
            Assert.IsFalse(_intsOneStatic.Names.Any(x => x.Equals("Int2")),
                           "Contained Int2 even though it's static");
        }
    }
}