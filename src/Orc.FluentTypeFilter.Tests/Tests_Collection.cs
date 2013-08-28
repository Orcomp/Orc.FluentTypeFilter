namespace Orc.FluentTypeFilter.Tests
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Tests_Collection
    {
        private PropertyFilter<IntsAndString> _intsAndString;

        [TestInitialize]
        public void Init()
        {
            _intsAndString = new PropertyFilter<IntsAndString>();
        }

        [TestMethod]
        public void IncludeAll()
        {
            // Add all properties
            _intsAndString.IncludeAll();

            // Verify
            var expectedMembers = new[] { "Int1", "Int2", "StringProp" };
            foreach (var s in expectedMembers)
            {
                Assert.IsTrue(_intsAndString.Names.Any(x => x.Equals(s)),
                    "Did not contain " + s);
            }
        }

        [TestMethod]
        public void IncludeAllByType()
        {
            // Add all int properties
            _intsAndString.IncludeAll<int>();

            // Verify
            var expectedMembers = new[] { "Int1", "Int2" };
            foreach (var s in expectedMembers)
            {
                Assert.IsTrue(_intsAndString.Names.Any(x => x.Equals(s)), 
                    "Did not contain " + s);
            }
        }

        [TestMethod]
        public void ExcludeAll()
        {
            // Add all properties
            IncludeAll();

            // Remove all int properties
            _intsAndString.ExcludeAll();

            // Should now be empty
            Assert.IsFalse(_intsAndString.Names.Any(),
                           "Expected collection to be empty");
        }

        [TestMethod]
        public void ExcludeAllByType()
        {
            // Add all int properties
            IncludeAllByType();

            // Remove all int properties
            _intsAndString.ExcludeAll<int>();

            // Should now be empty
            Assert.IsFalse(_intsAndString.Names.Any(), 
                           "Expected collection to be empty");
        }
    }
}