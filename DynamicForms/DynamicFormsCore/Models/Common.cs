using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;


namespace DynamicFormsCore.Models
{
    //public class Common
    //{
    //}

	[XmlRoot(ElementName = "item")]
	public class Item
	{
		[XmlElement(ElementName = "text")]
		public string Text { get; set; }

		[XmlElement(ElementName = "value")]
		public string Value { get; set; }

		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }

		[XmlAttribute(AttributeName = "selected")]
		public string Selected { get; set; }
	}

	[XmlRoot(ElementName = "itemList")]
	public class ItemList
	{
		[XmlElement(ElementName = "item")]
        public List<Item> Items { get; set; }

        [XmlAttribute(AttributeName = "group")]
		public string Group { get; set; }
	}

	[XmlRoot(ElementName = "control")]
	public class Control
	{
		[XmlElement(ElementName = "itemList")]
		public ItemList ItemList { get; set; }

		[XmlAttribute(AttributeName = "attr1")]
		public string Attr1 { get; set; }
	}


}

namespace DynamicFormsCore.Common
{
    #region XML Serialization
    //https://www.c-sharpcorner.com/UploadFile/8b7513/xml-serialization-and-deserialization-in-C-Sharp/
    public class SerializeDeserialize<T>
    {
        StringBuilder sbData;
        StringWriter swWriter;
        XmlDocument xDoc;
        XmlNodeReader xNodeReader;
        XmlSerializer xmlSerializer;

        public SerializeDeserialize()
        {
            sbData = new StringBuilder();
        }

        public string SerializeData(T data)
        {
            XmlSerializer employeeSerializer = new XmlSerializer(typeof(T));
            swWriter = new StringWriter(sbData);
            employeeSerializer.Serialize(swWriter, data);
            return sbData.ToString();
        }

        public T DeserializeData(string dataXML)
        {
            xDoc = new XmlDocument();
            xDoc.LoadXml(dataXML);
            xNodeReader = new XmlNodeReader(xDoc.DocumentElement);
            xmlSerializer = new XmlSerializer(typeof(T));
            var employeeData = xmlSerializer.Deserialize(xNodeReader);
            T deserializedEmployee = (T)employeeData;
            return deserializedEmployee;
        }
    }
    #endregion

    public enum FormInputType
    {       
        Image,     
        Date,
        Text,
        CheckBox,
        CheckBoxList,
        Number,
        File,
        Radio,
        Password,
        Time,
        URL,
        Color,
        Hidden,
        Email,
        Range,
        DateTimeLocal,
        Month,
        Tel,
        Week,
        SelectList
    }
}