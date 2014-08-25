using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtoBuf.Wcf.Channels.Infrastructure;
using ProtoBuf.Wcf.Channels.Serialization;
using ProtoBuf.Wcf.Tests.Models;

namespace ProtoBuf.Wcf.Tests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void SimpleModelTest()
        {
            var model = new TestModelSimple()
            {
                TestProperty1 = "Eureka!!!",
                TestProperty2 = 2,
                OtherFieldInfo = new DateTime(1985, 6, 15),
                Ints = new List<int>() {1,2}
            };

            var ser = ObjectBuilder.GetSerializer();

            var result = ser.Serialize(model);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.IsNotNull(result.MetaData);

            Assert.IsTrue(result.Data.Length > 0, "Serialized data length must be greater than 0, serialization failed.");

            var counterpart = ser.Deserialize<Models.Counterparts.TestModelSimple>(result.Data, result.MetaData);

            Assert.AreEqual(model.TestProperty1, counterpart.TestProperty1);
            Assert.AreEqual(model.TestProperty2, counterpart.TestProperty2);
            Assert.AreEqual(model.OtherFieldInfo, counterpart.OtherFieldInfo);

            Assert.IsNotNull(counterpart.Ints);
            Assert.AreEqual(model.Ints.Count, counterpart.Ints.Length);
            
            for (var i = 0; i < model.Ints.Count; i++)
            {
                Assert.AreEqual(model.Ints[i], counterpart.Ints[i]);
            }
        }

        [TestMethod]
        public void SimpleInheritanceTest()
        {
            var model = new ChildModelSimple()
            {
                TestBase1 = "1",
                TestBase2 = "2",
                TestChildProperty1 = "3",
                TestChildProperty2 = "4"
            };

            var ser = ObjectBuilder.GetSerializer();

            var result = ser.Serialize(model);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.IsNotNull(result.MetaData);

            Assert.IsTrue(result.Data.Length > 0, "Serialized data length must be greater than 0, serialization failed.");

            var counterpart = ser.Deserialize<Models.Counterparts.ChildModelSimple>(result.Data, result.MetaData);

            Assert.AreEqual(model.TestBase1, counterpart.TestBase1);
            Assert.AreEqual(model.TestBase2, counterpart.TestBase2);
            Assert.AreEqual(model.TestChildProperty1, counterpart.TestChildProperty1);
            Assert.AreEqual(model.TestChildProperty2, counterpart.TestChildProperty2);
        }

        [TestMethod]
        public void TestComplexModel()
        {
            var childModelSimple = new ChildModelSimple()
            {
                TestBase1 = "1",
                TestBase2 = "2",
                TestChildProperty1 = "3",
                TestChildProperty2 = "4"
            };

            var simpleModel = new TestModelSimple()
            {
                TestProperty1 = "Eureka!!!",
                TestProperty2 = 2,
                OtherFieldInfo = new DateTime(1985, 6, 15)
            };

            var shouldNotBeCarriedForward = new ShouldNotBeCarriedForward()
                {
                    Blah = "Blah"
                };

            var list = new List<ListItem>(){
                new ListItem() { Item = "Hello" },
                new ListItem() { Item = "Hello2" }
            };

            var complex = new TestModelComplex()
                {
                    BaseModel = childModelSimple,
                    TestModelSimple = simpleModel,
                    ShouldNotBeCarriedForward = shouldNotBeCarriedForward,
                    ListItems = list
                };

            var ser = ObjectBuilder.GetSerializer();

            var result = ser.Serialize(complex);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.IsNotNull(result.MetaData);
            
            Assert.IsTrue(result.Data.Length > 0, "Serialized data length must be greater than 0, serialization failed.");

            var counterpart = ser.Deserialize<Models.Counterparts.TestModelComplex>(result.Data, result.MetaData);

            Assert.IsNotNull(counterpart, "no data could be deserialized.");

            Assert.IsNotNull(counterpart.BaseModel, "Base model was not serialized correctly, inheritance test failed.");

            Assert.IsNotNull(counterpart.TestModelSimple, "Test model simple was not deserialized correctly.");

            Assert.IsNull(counterpart.ShouldNotBeCarriedForward, "ShouldNotBeCarriedForward does not have the datamember attribute and should not have been serialized/ deserialized, this is a security issue.");

            Assert.IsNotNull(counterpart.ListItems, "list of items was not deserialized correctly.");

            var child = counterpart.BaseModel as Models.Counterparts.ChildModelSimple;

            Assert.IsNotNull(child, "Child was not of type child, inheritance test failed.");

            Assert.AreEqual(childModelSimple.TestBase1, child.TestBase1);
            Assert.AreEqual(childModelSimple.TestBase2, child.TestBase2);
            Assert.AreEqual(childModelSimple.TestChildProperty1, child.TestChildProperty1);
            Assert.AreEqual(childModelSimple.TestChildProperty2, child.TestChildProperty2);
            
            Assert.AreEqual(simpleModel.TestProperty1, counterpart.TestModelSimple.TestProperty1);
            Assert.AreEqual(simpleModel.TestProperty2, counterpart.TestModelSimple.TestProperty2);
            Assert.AreEqual(simpleModel.OtherFieldInfo, counterpart.TestModelSimple.OtherFieldInfo);

            Assert.AreEqual(list.Count, counterpart.ListItems.Count, "Count of list items did not match.");

            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] != null)
                {
                    Assert.IsNotNull(counterpart.ListItems[i]);

                    Assert.AreEqual(list[i].Item, counterpart.ListItems[i].Item, 
                        "List item did not match, index: " + i);
                }
                else
                {
                    Assert.IsNull(counterpart.ListItems[i]);
                }
            }
        }
         
        [TestMethod]
        public void TestMetaDataSerialization()
        {
            var metaData = new TypeMetaData();

            metaData.StoreFieldNumber("blah/", "myName", "someField", 2);

            var serializer = ObjectBuilder.GetSerializer();

            var result = serializer.Serialize(metaData);

            Assert.IsTrue(result.Data.Length > 0, "Meta data was not successfully serialized. The resultant byte count was zero.");

            var deserialized = serializer.Deserialize<TypeMetaData>(result.Data);

            Assert.IsNotNull(deserialized, "Deserialization was not succesfull, the resultant object was null.");

            int? fieldNumber;
            var success = deserialized.GetFieldNumber("blah/", "myName", "someField", out fieldNumber);

            Assert.IsTrue(success, "The expected field number was not found in the deserialized data. MetaData deserialization failed.");

            Assert.AreEqual(2, fieldNumber.Value, "The expected field number was not found in the deserialized data. MetaData deserialization failed.");
        }
    }
}
