using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Orc.FluentTypeFilter.Tests
{
    [TestClass]
    public class When_Property_Not_Exists_In_Set
    {
        private PropertyFilter<IntsAndString> _intsAndString;

        [TestInitialize]
        public void Init()
        {
            _intsAndString = new PropertyFilter<IntsAndString>();
        }

        [TestMethod]
        public void Single_Remove_Ignored()
        {
            // Property exists, but not in current set
            _intsAndString.Exclude(x => x.Int1);

            // Nothing should happen
            Assert.AreEqual(_intsAndString.Names.Count(), 0, 
                            "Expected the set to be empty");
        }

        [TestMethod]
        public void Collection_Remove_Ignored()
        {
            // Properties exist, but not in current set
            _intsAndString.ExcludeAll<int>();

            // Nothing should happen
            Assert.AreEqual(_intsAndString.Names.Count(), 0,
                            "Expected the set to be empty");
        }
    }
}