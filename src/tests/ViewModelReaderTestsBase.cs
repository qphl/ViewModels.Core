using System;
using System.Linq;
using CR.ViewModels.Core;
using CR.ViewModels.Core.Exceptions;
using NUnit.Framework;

namespace CR.ViewModels.Tests
{
    // ReSharper disable InconsistentNaming
    public abstract class ViewModelRepositoryTestFixture
    {
        protected IViewModelReader Reader { get; set; }
        protected IViewModelWriter Writer { get; set; }

        #region Adding & getting by id

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
            Writer.Add(entity.Id, entity);

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

        #endregion

        #region Querying

        [Test]
        public void query_on_empty_repository_returns_no_results()
        {
            Assert.IsEmpty(Reader.Query<TestEntity1>((e) => true));
        }

        [Test]
        public void query_with_no_matches_returns_no_results()
        {
            var entity1 = new TestEntity1("woftam1", "string1");
            var entity2 = new TestEntity1("woftam2", "string2");
            Writer.Add(entity1.Id, entity1);
            Writer.Add(entity2.Id, entity2);

            Assert.IsEmpty(Reader.Query<TestEntity1>(e => e.Id == "Not There"));
        }

        [Test]
        public void query_returns_correct_results()
        {
            var entity1 = new TestEntity1("woftam1", "no match");
            var entity2 = new TestEntity1("woftam2", "match this");
            var entity3 = new TestEntity1("woftam3", "no match");
            var entity4 = new TestEntity1("woftam4", "match this");
            Writer.Add(entity1.Id, entity1);
            Writer.Add(entity2.Id, entity2);
            Writer.Add(entity3.Id, entity3);
            Writer.Add(entity4.Id, entity4);

            var results = Reader.Query<TestEntity1>(e => e.Field1 == "match this").ToList();
            Assert.Contains(entity2, results);
            Assert.Contains(entity4, results);
            Assert.AreEqual(2, results.Count);
        }

        [Test]
        public void query_with_matches_in_different_type_returns_no_results()
        {
            var entity1 = new TestEntity1("woftam1", "string1");
            var entity2 = new TestEntity1("woftam2", "string2");
            Writer.Add(entity1.Id, entity1);
            Writer.Add(entity2.Id, entity2);

            Assert.IsEmpty(Reader.Query<TestEntity2>(e => e.Id == "woftam1"));
        }

        #endregion

        #region Deleting

        [Test]
        public void deleting_from_an_empty_repository_throws_an_entity_not_found_exception()
        {
            Assert.Throws<EntityNotFoundException>(() => Writer.Delete<TestEntity1>("someKey"));
        }

        [Test]
        public void deleting_an_entity_that_is_not_in_the_repository_will_throw_an_entity_not_found_exception()
        {
            var entity1 = new TestEntity1("woftam2", "string2");
            Writer.Add(entity1.Id, entity1);

            Assert.Throws<EntityNotFoundException>(() => Writer.Delete<TestEntity1>("someKey"));
        }

        [Test]
        public void deleted_entity_cannot_be_retreived_by_id()
        {
            var entity1 = new TestEntity1("woftam2", "string2");
            Writer.Add(entity1.Id, entity1);
            Writer.Delete<TestEntity1>(entity1.Id);

            Assert.IsNull(Reader.GetByKey<TestEntity1>(entity1.Id));
        }

        [Test]
        public void deleted_entity_cannot_be_queried()
        {
            var entity1 = new TestEntity1("woftam2", "string2");
            Writer.Add(entity1.Id, entity1);
            Writer.Delete<TestEntity1>(entity1.Id);

            Assert.IsEmpty(Reader.Query<TestEntity1>(e => e.Field1 == "string2"));
        }

        [Test]
        public void deleting_entity_leaves_other_entities_intact()
        {
            var entity1 = new TestEntity1("woftam1", "string1");
            var entity2 = new TestEntity1("woftam2", "string2");
            Writer.Add(entity1.Id, entity1);
            Writer.Add(entity2.Id, entity2);
            Writer.Delete<TestEntity1>(entity1.Id);

            Assert.AreEqual(entity2, Reader.GetByKey<TestEntity1>(entity2.Id));
        }

        [Test]
        public void deleting_an_entity_twice_throws_an_entity_not_found_exception()
        {
            var entity1 = new TestEntity1("woftam", "string2");
            Writer.Add(entity1.Id, entity1);
            Writer.Delete<TestEntity1>(entity1.Id);
            Assert.Throws<EntityNotFoundException>(() => Writer.Delete<TestEntity1>(entity1.Id));
        }

        [Test]
        public void entity_can_be_retreived_after_deleting_and_re_adding()
        {
            var entity1 = new TestEntity1("woftam", "string2");
            Writer.Add(entity1.Id, entity1);
            Writer.Delete<TestEntity1>(entity1.Id);
            Writer.Add(entity1.Id, entity1);

            Assert.AreEqual(entity1, Reader.GetByKey<TestEntity1>(entity1.Id));
        }

        [Test]
        public void deleting_an_entity_with_wrong_type_throws_an_entity_not_found_exception()
        {
            var entity1 = new TestEntity1("woftam", "string2");
            Writer.Add(entity1.Id, entity1);

            Assert.Throws<EntityNotFoundException>(() => Writer.Delete<TestEntity2>(entity1.Id));
        }

        [Test]
        public void deleting_entity_leaves_entities_of_other_types_with_same_key_intact()
        {
            var entity1 = new TestEntity1("woftam1", "string1");
            var entity2 = new TestEntity2("woftam1", 123);
            Writer.Add(entity1.Id, entity1);
            Writer.Add(entity2.Id, entity2);
            Writer.Delete<TestEntity1>(entity1.Id);

            Assert.AreEqual(entity2, Reader.GetByKey<TestEntity2>(entity2.Id));
        }

        [Test]
        public void deleting_with_a_null_key_throws_an_argument_null_exception()
        {
            Assert.Throws<ArgumentNullException>(() => Writer.Delete<TestEntity1>(null));
        }

        [Test]
        public void deleting_with_an_empty_key_throws_an_argument_exception()
        {
            Assert.Throws<ArgumentException>(() => Writer.Delete<TestEntity1>(""));
        }

        #endregion

        #region Updating

        [Test]
        public void update_with_null_key_throws_an_argument_null_exception()
        {
            Assert.Throws<ArgumentNullException>(() => Writer.Update<TestEntity1>(null, e => { }));
        }

        [Test]
        public void update_with_empty_key_throws_an_argument_exception()
        {
            Assert.Throws<ArgumentException>(() => Writer.Update<TestEntity1>("", e => { }));
        }

        [Test]
        public void update_with_null_action_throws_an_argument_null_exception()
        {
            Assert.Throws<ArgumentNullException>(() => Writer.Update<TestEntity1>("something", null));
        }

        [Test]
        public void updating_with_an_empty_repository_throws_an_entity_not_found_exception()
        {
            Assert.Throws<EntityNotFoundException>(() => Writer.Update<TestEntity1>("someKey", e => { }));
        }

        [Test]
        public void updating_an_entity_that_is_not_in_the_repository_will_throw_an_entity_not_found_exception()
        {
            var entity1 = new TestEntity1("woftam2", "string2");
            Writer.Add(entity1.Id, entity1);

            Assert.Throws<EntityNotFoundException>(() => Writer.Update<TestEntity1>("someKey", e => { }));
        }

        [Test]
        public void updating_an_entity_applies_the_update_action()
        {
            var action = new Action<TestEntity1>(e => e.Field1 = "newvalue");
            const string id = "id";
            var entity1 = new TestEntity1(id, "initialValue");
            var expected = new TestEntity1(id, "initialValue");
            action(expected);
            
            Writer.Add(entity1.Id, entity1);
            Writer.Update(entity1.Id, action);
            var actual = Reader.GetByKey<TestEntity1>(entity1.Id);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void updating_an_entity_leaves_others_untouched()
        {
            var action = new Action<TestEntity1>(e => e.Field1 = "newvalue");
            const string id = "id";
            var entity1 = new TestEntity1(id, "initialValue");
            var entity2 = new TestEntity1("id2", "initialValue2");
            var expected = new TestEntity1("id2", "initialValue2");

            Writer.Add(entity1.Id, entity1);
            Writer.Add(entity2.Id, entity2);

            Writer.Update(entity1.Id, action);
            var actual = Reader.GetByKey<TestEntity1>(entity2.Id);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void updating_an_entity_leaves_entities_of_other_types_with_same_key_intact()
        {
            var action = new Action<TestEntity1>(e => e.Id = "newvalue");
            var entity1 = new TestEntity1("id", "initialValue");
            var entity2 = new TestEntity2("id", 111);
            var expected = new TestEntity2("id", 111);

            Writer.Add(entity1.Id, entity1);
            Writer.Add(entity2.Id, entity2);

            Writer.Update(entity1.Id, action);
            var actual = Reader.GetByKey<TestEntity2>(entity2.Id);
            Assert.AreEqual(expected, actual);
        }

        #endregion

    }
    // ReSharper restore InconsistentNaming
}
