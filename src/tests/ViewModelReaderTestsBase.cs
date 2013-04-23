using System;
using CR.ViewModels.Core;
using NUnit.Framework;

namespace CR.ViewModels.Tests
{
    // ReSharper disable InconsistentNaming
    public abstract class ViewModelRepositoryTestFixture
    {
        protected IViewModelReader Reader { get; set; }
        protected IViewModelWriter Writer { get; set; }

        [Test] 
        public void get_by_null_key_throws_argument_null_exception()
        {
            Assert.Throws<ArgumentNullException>(() => Reader.GetByKey<TestEntity1>(null));
        }

        [Test]
        public void get_by_empty_key_throws_argument_exception()
        {
            Assert.Throws<ArgumentException>(() => Reader.GetByKey<TestEntity1>(""));
        }

        [Test]
        public void get_from_empty_repository_returns_null()
        {
            Assert.IsNull(Reader.GetByKey<TestEntity1>("someKey"));
        }

        [Test]
        public void added_entity_can_be_retrieved()
        {
            var entity = new TestEntity1("woftam", "hello");
            Writer.Add(entity.Id, entity);
            Assert.AreEqual(entity, Reader.GetByKey<TestEntity1>(entity.Id));
        }

        [Test]
        public void get_key_not_in_repository_returns_null()
        {
            var entity = new TestEntity1("woftam", "hello");
            Writer.Add(entity.Id,entity);
            Assert.IsNull(Reader.GetByKey<TestEntity1>("notWoftam"));
        }

        [Test]
        public void multiple_entities_can_be_added_and_retreived()
        {
            var entity1 = new TestEntity1("woftam", "hello");
            var entity2 = new TestEntity1("woftam2", "hello2");
            Writer.Add(entity1.Id, entity1);
            Writer.Add(entity2.Id, entity2);

            Assert.AreEqual(entity1, Reader.GetByKey<TestEntity1>(entity1.Id));
            Assert.AreEqual(entity2, Reader.GetByKey<TestEntity1>(entity2.Id));
        }

        [Test]
        public void adding_duplicate_key_throws_duplicate_key_exception()
        {
            var entity1 = new TestEntity1("woftam", "hello");
            var entity2 = new TestEntity1("woftam", "hello2");
            Writer.Add(entity1.Id, entity1);
            
            Assert.Throws<DuplicateKeyException>(() => Writer.Add(entity2.Id, entity2));
        }

        [Test]
        public void get_from_type_not_in_repository_returns_null()
        {
            var entity = new TestEntity1("woftam", "hello");
            Writer.Add(entity.Id, entity);
            Assert.IsNull(Reader.GetByKey<TestEntity2>("woftam"));
        }

        [Test]
        public void retreiving_key_with_wrong_type_returns_null()
        {
            var entity1 = new TestEntity1("woftam", "hello");
            var entity2 = new TestEntity2("woftam2", 123);
            Writer.Add(entity1.Id, entity1);
            Writer.Add(entity2.Id, entity2);

            Assert.IsNull(Reader.GetByKey<TestEntity2>("woftam"));
        }

        [Test]
        public void multiple_types_of_entity_with_same_key_can_be_added_and_retreived()
        {
            var entity1 = new TestEntity1("woftam", "hello");
            var entity2 = new TestEntity2("woftam", 123);
            Writer.Add(entity1.Id, entity1);
            Writer.Add(entity2.Id, entity2);

            Assert.AreEqual(entity1, Reader.GetByKey<TestEntity1>(entity1.Id));
            Assert.AreEqual(entity2, Reader.GetByKey<TestEntity2>(entity2.Id));
        }



        /*
        [Test]
        public void query_on_empty_repository_returns_no_results()
        {
            Assert.IsEmpty(Reader.Query<TestEntity1>((e) => true));
        }
        */
         
    }
    // ReSharper restore InconsistentNaming
}
