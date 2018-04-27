using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            Writer.Add(entity.Identifier, entity);

            Assert.AreEqual(entity, Reader.GetByKey<TestEntity1>(entity.Identifier));
        }

        [Test]
        public void get_key_not_in_repository_returns_null()
        {
            var entity = new TestEntity1("woftam", "hello");
            Writer.Add(entity.Identifier, entity);

            Assert.IsNull(Reader.GetByKey<TestEntity1>("notWoftam"));
        }

        [Test]
        public void multiple_entities_can_be_added_and_retreived()
        {
            var entity1 = new TestEntity1("woftam", "hello");
            var entity2 = new TestEntity1("woftam2", "hello2");
            Writer.Add(entity1.Identifier, entity1);
            Writer.Add(entity2.Identifier, entity2);

            Assert.AreEqual(entity1, Reader.GetByKey<TestEntity1>(entity1.Identifier));
            Assert.AreEqual(entity2, Reader.GetByKey<TestEntity1>(entity2.Identifier));
        }

        [Test]
        public void adding_duplicate_key_throws_duplicate_key_exception()
        {
            var entity1 = new TestEntity1("woftam", "hello");
            var entity2 = new TestEntity1("woftam", "hello2");
            Writer.Add(entity1.Identifier, entity1);

            Assert.Throws<DuplicateKeyException>(() => Writer.Add(entity2.Identifier, entity2));
        }

        [Test]
        public void get_from_type_not_in_repository_returns_null()
        {
            var entity = new TestEntity1("woftam", "hello");
            Writer.Add(entity.Identifier, entity);

            Assert.IsNull(Reader.GetByKey<TestEntity2>("woftam"));
        }

        [Test]
        public void retreiving_key_with_wrong_type_returns_null()
        {
            var entity1 = new TestEntity1("woftam", "hello");
            var entity2 = new TestEntity2("woftam2", 123);
            Writer.Add(entity1.Identifier, entity1);
            Writer.Add(entity2.Identifier, entity2);

            Assert.IsNull(Reader.GetByKey<TestEntity2>("woftam"));
        }

        [Test]
        public void multiple_types_of_entity_with_same_key_can_be_added_and_retreived()
        {
            var entity1 = new TestEntity1("woftam", "hello");
            var entity2 = new TestEntity2("woftam", 123);
            Writer.Add(entity1.Identifier, entity1);
            Writer.Add(entity2.Identifier, entity2);

            Assert.AreEqual(entity1, Reader.GetByKey<TestEntity1>(entity1.Identifier));
            Assert.AreEqual(entity2, Reader.GetByKey<TestEntity2>(entity2.Identifier));
        }

        #endregion

        #region Querying

        [Test]
        public void query_on_empty_repository_returns_no_results()
        {
            Assert.IsEmpty(Reader.Query<TestEntity1>().Where(e => e.Field1 == null || e.Field1 != null).ToList());
        }

        [Test]
        public void query_with_no_matches_returns_no_results()
        {
            var entity1 = new TestEntity1("woftam1", "string1");
            var entity2 = new TestEntity1("woftam2", "string2");
            Writer.Add(entity1.Identifier, entity1);
            Writer.Add(entity2.Identifier, entity2);

            Assert.IsEmpty(Reader.Query<TestEntity1>().Where(e => e.Identifier == "Not There"));
        }

        [Test]
        public void query_returns_correct_results()
        {
            var entity1 = new TestEntity1("woftam1", "no match");
            var entity2 = new TestEntity1("woftam2", "match this");
            var entity3 = new TestEntity1("woftam3", "no match");
            var entity4 = new TestEntity1("woftam4", "match this");
            Writer.Add(entity1.Identifier, entity1);
            Writer.Add(entity2.Identifier, entity2);
            Writer.Add(entity3.Identifier, entity3);
            Writer.Add(entity4.Identifier, entity4);

            var results = Reader.Query<TestEntity1>().Where(e => e.Field1 == "match this").ToList();
            Assert.Contains(entity2, results);
            Assert.Contains(entity4, results);
            Assert.AreEqual(2, results.Count);
        }

        [Test]
        public void query_with_matches_in_different_type_returns_no_results()
        {
            var entity1 = new TestEntity1("woftam1", "string1");
            var entity2 = new TestEntity1("woftam2", "string2");
            Writer.Add(entity1.Identifier, entity1);
            Writer.Add(entity2.Identifier, entity2);

            Assert.IsEmpty(Reader.Query<TestEntity2>().Where(e => e.Identifier == "woftam1"));
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
            Writer.Add(entity1.Identifier, entity1);

            Assert.Throws<EntityNotFoundException>(() => Writer.Delete<TestEntity1>("someKey"));
        }

        [Test]
        public void deleted_entity_cannot_be_retreived_by_id()
        {
            var entity1 = new TestEntity1("woftam2", "string2");
            Writer.Add(entity1.Identifier, entity1);
            Writer.Delete<TestEntity1>(entity1.Identifier);

            Assert.IsNull(Reader.GetByKey<TestEntity1>(entity1.Identifier));
        }

        [Test]
        public void deleted_entity_cannot_be_queried()
        {
            var entity1 = new TestEntity1("woftam2", "string2");
            Writer.Add(entity1.Identifier, entity1);
            Writer.Delete<TestEntity1>(entity1.Identifier);

            Assert.IsEmpty(Reader.Query<TestEntity1>().Where(e => e.Field1 == "string2"));
        }

        [Test]
        public void deleting_entity_leaves_other_entities_intact()
        {
            var entity1 = new TestEntity1("woftam1", "string1");
            var entity2 = new TestEntity1("woftam2", "string2");
            Writer.Add(entity1.Identifier, entity1);
            Writer.Add(entity2.Identifier, entity2);
            Writer.Delete<TestEntity1>(entity1.Identifier);

            Assert.AreEqual(entity2, Reader.GetByKey<TestEntity1>(entity2.Identifier));
        }

        [Test]
        public void deleting_an_entity_twice_throws_an_entity_not_found_exception()
        {
            var entity1 = new TestEntity1("woftam", "string2");
            Writer.Add(entity1.Identifier, entity1);
            Writer.Delete<TestEntity1>(entity1.Identifier);
            Assert.Throws<EntityNotFoundException>(() => Writer.Delete<TestEntity1>(entity1.Identifier));
        }

        [Test]
        public void entity_can_be_retreived_after_deleting_and_re_adding()
        {
            var entity1 = new TestEntity1("woftam", "string2");
            Writer.Add(entity1.Identifier, entity1);
            Writer.Delete<TestEntity1>(entity1.Identifier);
            Writer.Add(entity1.Identifier, entity1);

            Assert.AreEqual(entity1, Reader.GetByKey<TestEntity1>(entity1.Identifier));
        }

        [Test]
        public void deleting_an_entity_with_wrong_type_throws_an_entity_not_found_exception()
        {
            var entity1 = new TestEntity1("woftam", "string2");
            Writer.Add(entity1.Identifier, entity1);

            Assert.Throws<EntityNotFoundException>(() => Writer.Delete<TestEntity2>(entity1.Identifier));
        }

        [Test]
        public void deleting_entity_leaves_entities_of_other_types_with_same_key_intact()
        {
            var entity1 = new TestEntity1("woftam1", "string1");
            var entity2 = new TestEntity2("woftam1", 123);
            Writer.Add(entity1.Identifier, entity1);
            Writer.Add(entity2.Identifier, entity2);
            Writer.Delete<TestEntity1>(entity1.Identifier);

            Assert.AreEqual(entity2, Reader.GetByKey<TestEntity2>(entity2.Identifier));
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
            Writer.Add(entity1.Identifier, entity1);

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
            
            Writer.Add(entity1.Identifier, entity1);
            Writer.Update(entity1.Identifier, action);
            var actual = Reader.GetByKey<TestEntity1>(entity1.Identifier);

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

            Writer.Add(entity1.Identifier, entity1);
            Writer.Add(entity2.Identifier, entity2);

            Writer.Update(entity1.Identifier, action);
            var actual = Reader.GetByKey<TestEntity1>(entity2.Identifier);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void updating_an_entity_leaves_entities_of_other_types_with_same_key_intact()
        {
            var action = new Action<TestEntity1>(e => e.Identifier = "newvalue");
            var entity1 = new TestEntity1("id", "initialValue");
            var entity2 = new TestEntity2("id", 111);
            var expected = new TestEntity2("id", 111);

            Writer.Add(entity1.Identifier, entity1);
            Writer.Add(entity2.Identifier, entity2);

            Writer.Update(entity1.Identifier, action);
            var actual = Reader.GetByKey<TestEntity2>(entity2.Identifier);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Deleting with predicate

        //null predicate

        [Test]
        public void delete_where_on_empty_repository_does_nothing()
        {
            Assert.DoesNotThrow(() => Writer.DeleteWhere<TestEntity1>(e => e.Field1 == null || e.Field1 != null));
        }

        [Test]
        public void delete_where_deletes_matching_entities()
        {
            var entity1 = new TestEntity1("id1", "match");
            var entity2 = new TestEntity1("id2", "no match");
            var entity3 = new TestEntity1("id3", "no match");
            var entity4 = new TestEntity1("id4", "match");
            Writer.Add(entity1.Identifier, entity1);
            Writer.Add(entity2.Identifier, entity2);
            Writer.Add(entity3.Identifier, entity3);
            Writer.Add(entity4.Identifier, entity4);

            Expression<Func<TestEntity1, bool>> predicate = e => e.Field1.Equals("match");
            Writer.DeleteWhere(predicate);

            Assert.IsEmpty(Reader.Query<TestEntity1>().Where(predicate));
        }

        [Test]
        public void delete_where_leaves_non_matching_entities_unchanged()
        {
            var entity1 = new TestEntity1("id1", "match");
            var entity2 = new TestEntity1("id2", "no match");
            var entity3 = new TestEntity1("id3", "no match");
            var entity4 = new TestEntity1("id4", "match");
            Writer.Add(entity1.Identifier, entity1);
            Writer.Add(entity2.Identifier, entity2);
            Writer.Add(entity3.Identifier, entity3);
            Writer.Add(entity4.Identifier, entity4);

            Expression<Func<TestEntity1, bool>> predicate = e => e.Field1.Equals("match");
            Writer.DeleteWhere(predicate);

            var stuffLeft = Reader.Query<TestEntity1>().Where(e => !e.Field1.Equals("match")).ToList();

            Assert.Contains(entity2, stuffLeft);
            Assert.Contains(entity3, stuffLeft);
            Assert.AreEqual(2,stuffLeft.Count);

        }

        [Test]
        public void delete_where_with_null_predicate_throws_an_argument_null_exception()
        {
            Assert.Throws<ArgumentNullException>(() => Writer.DeleteWhere<TestEntity1>(null));
        }

        [Test]
        public void deleting_entities_leaves_matching_entities_of_other_types_intact()
        {
            var entity1 = new TestEntity1("id", "initialValue");
            var entity2 = new TestEntity2("id", 111);
            var expected = new TestEntity2("id", 111);

            Writer.Add(entity1.Identifier, entity1);
            Writer.Add(entity2.Identifier, entity2);

            Writer.DeleteWhere<TestEntity1>(e => e.Identifier.Equals("id"));

            var actual = Reader.GetByKey<TestEntity2>(entity2.Identifier);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Updating with predicate

        [Test]
        public void update_where_on_empty_repository_does_nothing()
        {
            Assert.DoesNotThrow(() => Writer.UpdateWhere<TestEntity1>(e => e.Field1 == null || e.Field1 != null, e => { }));
        }

        [Test]
        public void update_where_with_null_predicate_throws_an_argument_null_exception()
        {
            Assert.Throws<ArgumentNullException>(() => Writer.UpdateWhere<TestEntity1>(null, e => { }));
        }

        [Test]
        public void update_where_with_null_action_throws_an_argument_null_exception()
        {
            Assert.Throws<ArgumentNullException>(() => Writer.UpdateWhere<TestEntity1>(e => e.Field1 == null || e.Field1 != null, null));
        }

        [Test]
        public void update_where_updates_matching_entities()
        {
            var entity1 = new TestEntity1("id1", "match");
            var entity2 = new TestEntity1("id2", "no match");
            var entity3 = new TestEntity1("id3", "no match");
            var entity4 = new TestEntity1("id4", "match");
            Writer.Add(entity1.Identifier, entity1);
            Writer.Add(entity2.Identifier, entity2);
            Writer.Add(entity3.Identifier, entity3);
            Writer.Add(entity4.Identifier, entity4);

            Expression<Func<TestEntity1, bool>> predicate = e => e.Field1.Equals("match");
            Action<TestEntity1> update = e => e.Field1 = "new";
            Writer.UpdateWhere(predicate, update);

            var expected1 = new TestEntity1("id1", "new");
            var expected4 = new TestEntity1("id4", "new");

            var actual1 = Reader.GetByKey<TestEntity1>(entity1.Identifier);
            var actual4 = Reader.GetByKey<TestEntity1>(entity4.Identifier);

            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected4, actual4);
        }

        [Test]
        public void update_where_leaves_non_matching_entities_unchanged()
        {
            var entity1 = new TestEntity1("id1", "match");
            var entity2 = new TestEntity1("id2", "no match");
            var entity3 = new TestEntity1("id3", "no match");
            var entity4 = new TestEntity1("id4", "match");
            Writer.Add(entity1.Identifier, entity1);
            Writer.Add(entity2.Identifier, entity2);
            Writer.Add(entity3.Identifier, entity3);
            Writer.Add(entity4.Identifier, entity4);

            Expression<Func<TestEntity1, bool>> predicate = e => e.Field1.Equals("match");
            Action<TestEntity1> update = e => e.Field1 = "new";
            Writer.UpdateWhere(predicate, update);

            var expected2 = new TestEntity1("id2", "no match");
            var expected3 = new TestEntity1("id3", "no match");

            var actual2 = Reader.GetByKey<TestEntity1>(entity2.Identifier);
            var actual3 = Reader.GetByKey<TestEntity1>(entity3.Identifier);

            Assert.AreEqual(expected2, actual2);
            Assert.AreEqual(expected3, actual3);
        }

        [Test]
        public void update_where_leaves_matching_entities_of_other_types_intact()
        {
            var entity1 = new TestEntity1("id", "initialValue");
            var entity2 = new TestEntity2("id", 111);
            var expected = new TestEntity2("id", 111);

            Writer.Add(entity1.Identifier, entity1);
            Writer.Add(entity2.Identifier, entity2);

            Writer.UpdateWhere<TestEntity1>(e => e.Identifier.Equals("id"), e => e.Identifier = "new");

            var actual = Reader.GetByKey<TestEntity2>(entity2.Identifier);
            Assert.AreEqual(expected, actual);
        }

        private const int LARGE_LIST_SIZE = 5000;

        [Test]
        public void update_where_updates_matching_entities_on_large_lists()
        {
            var expectedList = new List<TestEntity1>();

            for (int i = 0; i < LARGE_LIST_SIZE; i = i + 2)
            {
                var matchEntity = new TestEntity1("id" + i, "match");
                var expectedEntity = new TestEntity1("id" + i, "new");
                var nomatchEntity = new TestEntity1("id" + (i + 1), "no match");
                Writer.Add(matchEntity.Identifier, matchEntity);
                Writer.Add(nomatchEntity.Identifier, nomatchEntity);
                expectedList.Add(expectedEntity);
            }

            Expression<Func<TestEntity1, bool>> predicate = e => e.Field1.Equals("match");
            Action<TestEntity1> update = e => e.Field1 = "new";
            Writer.UpdateWhere(predicate, update);

            foreach (var expected in expectedList)
            {
                var actual = Reader.GetByKey<TestEntity1>(expected.Identifier);
                Assert.AreEqual(expected, actual, "comparison failed for item " + expected.Identifier);
            }
        }

        [Test]
        public void delete_where_deletes_matching_entities_on_large_lists()
        {
            var expectedList = new List<TestEntity1>();

            for (int i = 0; i < LARGE_LIST_SIZE; i = i + 2)
            {
                var matchEntity = new TestEntity1("id" + i, "match");
                var expectedEntity = new TestEntity1("id" + i, "new");
                var nomatchEntity = new TestEntity1("id" + (i + 1), "no match");
                Writer.Add(matchEntity.Identifier, matchEntity);
                Writer.Add(nomatchEntity.Identifier, nomatchEntity);
                expectedList.Add(expectedEntity);
            }

            Expression<Func<TestEntity1, bool>> predicate = e => e.Field1.Equals("match");
            Writer.DeleteWhere(predicate);

            foreach (var expected in expectedList)
            {
                Assert.IsNull(Reader.GetByKey<TestEntity1>(expected.Identifier));
            }
        }

        [Test]
        public void skip_and_take_paging_on_query_returns_all_expected_entities()
        {
            var expectedList = new List<TestEntity1>();

            for (int i = 0; i < LARGE_LIST_SIZE; i = i + 2)
            {
                var matchEntity = new TestEntity1("id" + i, "match");
                var expectedEntity = new TestEntity1("id" + i, "match");
                var nomatchEntity = new TestEntity1("id" + (i + 1), "no match");
                Writer.Add(matchEntity.Identifier, matchEntity);
                Writer.Add(nomatchEntity.Identifier, nomatchEntity);
                expectedList.Add(expectedEntity);
            }

            List<TestEntity1> resultChunk;
            var allResults = new List<TestEntity1>();
            int resultsToSkip = 0;

            do
            {
                resultChunk =
                    Reader.Query<TestEntity1>()
                           .Where(e => e.Field1 == "match")
                           .Skip(resultsToSkip)
                           .Take(10)
                           .ToList();
                resultsToSkip += resultChunk.Count;
                allResults.AddRange(resultChunk);
            } while (resultChunk.Count > 0);

            foreach(var entity in expectedList)
                Assert.Contains(entity,allResults);

            Assert.AreEqual(expectedList.Count, allResults.Count);
        }

        [Test]
        public void orderby_thenby_returns_list_in_correct_order()
        {
            var expectedList = new List<TestEntity2>();

            expectedList.Add(new TestEntity2("a",1));
            expectedList.Add(new TestEntity2("a",2));
            expectedList.Add(new TestEntity2("b",1));
            expectedList.Add(new TestEntity2("b",2));
            expectedList.Add(new TestEntity2("c",1));
            expectedList.Add(new TestEntity2("c",2));

            Writer.Add("id2", new TestEntity2("a",2));
            Writer.Add("id3", new TestEntity2("b",1));
            Writer.Add("id4", new TestEntity2("b",2));
            Writer.Add("id5", new TestEntity2("c",1));
            Writer.Add("id6", new TestEntity2("c",2));
            Writer.Add("id1", new TestEntity2("a",1));

            var results = Reader.Query<TestEntity2>().OrderBy(e => e.Identifier).ThenBy(e => e.Field1).ToList();

            Assert.IsTrue(results.SequenceEqual(expectedList));
        }

        #endregion

    }
    // ReSharper restore InconsistentNaming
}
