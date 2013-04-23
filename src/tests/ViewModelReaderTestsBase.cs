using System;
using NUnit.Framework;
using cr.viewmodels.core;

namespace cr.viewmodels.tests
{
    // ReSharper disable InconsistentNaming
    public abstract class ViewModelRepositoryTestFixture
    {
        protected IViewModelReader Reader { get; set; }
        protected IViewModelWriter Writer { get; set; }

        [Test] 
        public void get_by_null_key_throws_argument_exception()
        {
            Assert.Throws<ArgumentException>(() => Reader.GetByKey<TestEntity1>(null));
        }
    }
    // ReSharper restore InconsistentNaming
}
