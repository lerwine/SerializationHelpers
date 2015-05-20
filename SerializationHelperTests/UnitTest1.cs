using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SerializationHelpers;
using System.Linq;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SerializationHelperTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SerializeDBNullTestMethod()
        {
            DBNull target = DBNull.Value;
            XDocument actual = SerializationUtility.SerializeObjectToXDocument(target);
            XElement xElement = actual.Element(XName.Get("DBNull", "http://schemas.datacontract.org/2004/07/System"));
            Assert.IsNotNull(xElement);
            Assert.IsTrue(xElement.IsEmpty);
        }
        
        [TestMethod]
        public void SerializeArrayTestMethod1()
        {
            object[] target = null;
            XDocument actual = SerializationUtility.SerializeObjectToXDocument(target);
            XElement xElement = actual.Element(XName.Get("anyType", "http://schemas.microsoft.com/2003/10/Serialization/"));
            Assert.IsNotNull(xElement);
            XAttribute attribute = xElement.Attribute(XName.Get("nil", "http://www.w3.org/2001/XMLSchema-instance"));
            Assert.IsNotNull(attribute);
            Assert.AreEqual(attribute.Value, "true");
            Assert.IsTrue(xElement.IsEmpty);

            target = new object[3];
            target[0] = "first";
            target[1] = 2;
            target[2] = null;

            actual = SerializationUtility.SerializeObjectToXDocument(target);
            xElement = actual.Element(XName.Get("ArrayOfanyType", "http://schemas.microsoft.com/2003/10/Serialization/Arrays"));
            Assert.IsNotNull(xElement);
            Assert.IsFalse(xElement.IsEmpty);
        }

        [TestMethod]
        public void SerializeArrayTestMethod2()
        {
            int?[] target = null;
            XDocument actual = SerializationUtility.SerializeObjectToXDocument(target);
            XElement xElement = actual.Element(XName.Get("anyType", "http://schemas.microsoft.com/2003/10/Serialization/"));
            Assert.IsNotNull(xElement);
            XAttribute attribute = xElement.Attribute(XName.Get("nil", "http://www.w3.org/2001/XMLSchema-instance"));
            Assert.IsNotNull(attribute);
            Assert.AreEqual(attribute.Value, "true");
            Assert.IsTrue(xElement.IsEmpty);

            target = new int?[3];
            target[0] = 1;
            target[1] = 2;
            target[2] = null;

            actual = SerializationUtility.SerializeObjectToXDocument(target);
            xElement = actual.Element(XName.Get("ArrayOfNullableOfint", "http://schemas.datacontract.org/2004/07/System"));
            Assert.IsNotNull(xElement);
            Assert.IsFalse(xElement.IsEmpty);
        }

        [TestMethod]
        public void SerializeHashTableTestMethod()
        {
            Hashtable target = null;
            XDocument actual = SerializationUtility.SerializeObjectToXDocument(target);
            XElement xElement = actual.Element(XName.Get("anyType", "http://schemas.microsoft.com/2003/10/Serialization/"));
            Assert.IsNotNull(xElement);
            XAttribute attribute = xElement.Attribute(XName.Get("nil", "http://www.w3.org/2001/XMLSchema-instance"));
            Assert.IsNotNull(attribute);
            Assert.AreEqual(attribute.Value, "true");
            Assert.IsTrue(xElement.IsEmpty);

            target = new Hashtable();
            target.Add("first", "one");
            target.Add("second", 2);
            target.Add(3, 3.0);

            actual = SerializationUtility.SerializeObjectToXDocument(target);
            xElement = actual.Element(XName.Get("ArrayOfKeyValueOfanyTypeanyType", "http://schemas.microsoft.com/2003/10/Serialization/Arrays"));
            Assert.IsNotNull(xElement);
            Assert.IsFalse(xElement.IsEmpty);
        }

        [TestMethod]
        public void SerializeDictionaryMethod2()
        {
            Dictionary<Guid, object> target = null;
            XDocument actual = SerializationUtility.SerializeObjectToXDocument(target);
            XElement xElement = actual.Element(XName.Get("anyType", "http://schemas.microsoft.com/2003/10/Serialization/"));
            Assert.IsNotNull(xElement);
            XAttribute attribute = xElement.Attribute(XName.Get("nil", "http://www.w3.org/2001/XMLSchema-instance"));
            Assert.IsNotNull(attribute);
            Assert.AreEqual(attribute.Value, "true");
            Assert.IsTrue(xElement.IsEmpty);

            target = new Dictionary<Guid, object>();
            target.Add(Guid.Empty, "one");
            target.Add(Guid.NewGuid(), 2);
            target.Add(Guid.NewGuid(), 3.0);

            actual = SerializationUtility.SerializeObjectToXDocument(target);
            xElement = actual.Element(XName.Get("ArrayOfKeyValueOfguidanyType", "http://schemas.microsoft.com/2003/10/Serialization/Arrays"));
            Assert.IsNotNull(xElement);
            Assert.IsFalse(xElement.IsEmpty);
        }

        [TestMethod]
        public void SerializeDictionaryMethod3()
        {
            Dictionary<Guid, string> target = null;
            XDocument actual = SerializationUtility.SerializeObjectToXDocument(target);
            XElement xElement = actual.Element(XName.Get("anyType", "http://schemas.microsoft.com/2003/10/Serialization/"));
            Assert.IsNotNull(xElement);
            XAttribute attribute = xElement.Attribute(XName.Get("nil", "http://www.w3.org/2001/XMLSchema-instance"));
            Assert.IsNotNull(attribute);
            Assert.AreEqual(attribute.Value, "true");
            Assert.IsTrue(xElement.IsEmpty);

            target = new Dictionary<Guid, string>();
            target.Add(Guid.Empty, "one");
            target.Add(Guid.NewGuid(), "two");
            target.Add(Guid.NewGuid(),"three");

            actual = SerializationUtility.SerializeObjectToXDocument(target);
            xElement = actual.Element(XName.Get("ArrayOfKeyValueOfguidstring", "http://schemas.microsoft.com/2003/10/Serialization/Arrays"));
            Assert.IsNotNull(xElement);
            Assert.IsFalse(xElement.IsEmpty);
        }

        [TestMethod]
        public void SerializeStringTestMethod()
        {
            string target = null;
            XDocument actual = SerializationUtility.SerializeObjectToXDocument(target);
            XElement xElement = actual.Element(XName.Get("anyType", "http://schemas.microsoft.com/2003/10/Serialization/"));
            Assert.IsNotNull(xElement);
            XAttribute attribute = xElement.Attribute(XName.Get("nil", "http://www.w3.org/2001/XMLSchema-instance"));
            Assert.IsNotNull(attribute);
            Assert.AreEqual(attribute.Value, "true");
            Assert.IsTrue(xElement.IsEmpty);

            target = "";
            actual = SerializationUtility.SerializeObjectToXDocument(target);
            xElement = actual.Element(XName.Get("string", "http://schemas.microsoft.com/2003/10/Serialization/"));
            Assert.IsNotNull(xElement);
            Assert.IsFalse(xElement.IsEmpty);
            Assert.AreEqual(xElement.Value, target);
            Assert.AreEqual(0, xElement.Elements().Count());
        }

        [TestMethod]
        public void SerializeExceptionTestMethod()
        {
            Exception target = null;
            XDocument actual = SerializationUtility.SerializeObjectToXDocument(target);
            XElement xElement = actual.Element(XName.Get("anyType", "http://schemas.microsoft.com/2003/10/Serialization/"));
            Assert.IsNotNull(xElement);
            XAttribute attribute = xElement.Attribute(XName.Get("nil", "http://www.w3.org/2001/XMLSchema-instance"));
            Assert.IsNotNull(attribute);
            Assert.AreEqual(attribute.Value, "true");
            Assert.IsTrue(xElement.IsEmpty);

            try
            {
                throw new ArgumentNullException("test");
            }
            catch (Exception exc)
            {
                target = exc;
            }

            actual = SerializationUtility.SerializeObjectToXDocument(target);

            try
            {
                Exception e1;
                try
                {
                    throw new ArgumentNullException("test");
                }
                catch (Exception exc)
                {
                    e1 = exc;
                }
                try
                {
                    try
                    {
                        throw new InvalidOperationException("This is the inner");
                    }
                    catch (Exception exc)
                    {
                        throw new Exception("This is the outer", exc);
                    }
                }
                catch (Exception e2)
                {
                    throw new AggregateException(e1, e2);
                }
            }
            catch (Exception e)
            {
                target = e;
            }
            actual = SerializationUtility.SerializeObjectToXDocument(target);
        }
    }
}
