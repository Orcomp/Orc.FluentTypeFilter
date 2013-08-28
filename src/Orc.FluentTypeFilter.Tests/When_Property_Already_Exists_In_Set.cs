namespace Orc.FluentTypeFilter.Tests
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class When_Property_Already_Exists_In_Set
    {
        private PropertyFilter<IntsAndString> _intsAndString;

        [TestInitialize]
        public void Init()
        {
            _intsAndString = new PropertyFilter<IntsAndString>();
        }

        [TestMethod]
        public void Single_Add_Is_Overwritten()
        {
            // Add property
            _intsAndString.Include(x => x.Int1);

            // Property now exists, and should be overwritten
            _intsAndString.Include(x => x.Int1);

            // Total should be 1
            Assert.AreEqual(_intsAndString.Names.Count(), 1,
                "Expected the set have a size of 1");
        }

        [TestMethod]
        public void Collection_Adds_Are_Overwritten()
        {
            // Add all properties of type int
            _intsAndString.IncludeAll<int>();

            // Properties exists, and should be overwritten
            _intsAndString.IncludeAll<int>();

            // Total should be 2
            Assert.AreEqual(_intsAndString.Names.Count(), 2,
                "Expected the set have a size of 2");
        }
    }
}
