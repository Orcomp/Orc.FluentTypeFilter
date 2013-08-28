namespace Orc.FluentTypeFilter.Tests
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Tests_Single
    {
        private PropertyFilter<IntsAndString> _intsAndString;

        [TestInitialize]
        public void Init()
        {
            _intsAndString = new PropertyFilter<IntsAndString>();
        }

        [TestMethod]
        public void AddToExisting()
        {
            // Start with a collection, ints only
            _intsAndString.IncludeAll<int>();

            // String shouldnt be included yet
            Assert.IsFalse(_intsAndString.Names.Any(x => x.Equals("StringProp")), 
                           "Contained StringProp when only ints were expected");

            _intsAndString.Include(x => x.StringProp);
            
            // String should now be included
            Assert.IsTrue(_intsAndString.Names.Any(x => x.Equals("StringProp")), 
                          "Did not contain StringProp");
        }

        [TestMethod]
        public void RemoveFromExisting()
        {
            // Start with string included
            AddToExisting();

            // Remove it
            _intsAndString.Exclude(x => x.StringProp);

            // String should now *not* be included
            Assert.IsFalse(_intsAndString.Names.Any(x => x.Equals("StringProp")), 
                           "Contained StringProp when it should have been removed");
        }
    }
}