using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Orc.FluentTypeFilter.Tests
{
    [TestClass]
    public class When_Property_Not_Exists
    {
        private PropertyFilter<IntsAndString> _intsAndString;

        [TestInitialize]
        public void Init()
        {
            _intsAndString = new PropertyFilter<IntsAndString>();
        }

        [TestMethod]
        public void Throws_Collection_Add_If_Bool_Set()
        {
            try
            {
                _intsAndString.IncludeAll<float>(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, true);
            }
            catch (NoSuchPropertyException)
            {
                return;
            }

            Assert.Fail("Expected an exception to be thrown");
        }

        [TestMethod]
        public void Throws_Collection_Remove_If_Bool_Set()
        {
            try
            {
                _intsAndString.ExcludeAll<float>(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, true);
            }
            catch (NoSuchPropertyException)
            {
                return;
            }

            Assert.Fail("Expected an exception to be thrown");
        }

        [TestMethod]
        public void Not_Throws_Collection_Add_If_Bool_Not_Set()
        {
            try
            {
                _intsAndString.IncludeAll<float>(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, false);
            }
            catch (NoSuchPropertyException)
            {
                Assert.Fail("Did not expect an exception to be thrown");
            }
        }

        [TestMethod]
        public void Not_Throws_Collection_Remove_If_Bool_Not_Set()
        {
            try
            {
                _intsAndString.ExcludeAll<float>(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, false);
            }
            catch (NoSuchPropertyException)
            {
                Assert.Fail("Did not expect an exception to be thrown");
            }
        }
    }
}