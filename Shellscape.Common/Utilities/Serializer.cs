﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Shellscape.Utilities {

	/// <summary>
	/// Utility class. Serialization Helper
	/// </summary>
	public static class Serializer {

		/// <summary>
		/// Serializes the specified instance to XML using the <see cref="XmlSerializer"/>
		/// </summary>
		/// <typeparam name="T">Type to be serialized</typeparam>
		/// <param name="instance">Instance to be serialized</param>
		/// <returns>XML string</returns>
		public static string Serialize<T>(T instance) {
			if (instance == null) {
				throw new ArgumentNullException("instance");
			}

			StringBuilder builder = new StringBuilder();

			using (XmlWriter xmlWriter = XmlTextWriter.Create(builder))
			using (XmlDictionaryWriter writer = XmlDictionaryWriter.CreateDictionaryWriter(xmlWriter)) {
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				serializer.Serialize(writer, instance);
			}

			return builder.ToString();

		}

		/// <summary>
		/// Serializes the specified instance to XML using the <see cref="DataContractSerializer"/>
		/// </summary>
		/// <typeparam name="T">Type to be serialized</typeparam>
		/// <param name="instance">Instance to be serialized</param>
		/// <returns>XML string</returns>
		public static string SerializeContract<T>(T instance) {
			if (instance == null) {
				throw new ArgumentNullException("instance");
			}

			StringBuilder builder = new StringBuilder();

			var ser = new DataContractSerializer(typeof(T));
			var settings = new XmlWriterSettings { Indent = true };

			using (XmlWriter xw = XmlWriter.Create(builder, settings)) {
				ser.WriteObject(xw, instance);
			}

			return builder.ToString();
		}

		/// <summary>
		/// Deserializes the specified XML string to the specified type, using the <see cref="XmlSerializer"/>
		/// </summary>
		/// <typeparam name="T">Type to deserialize to</typeparam>
		/// <param name="xml">XML</param>
		/// <returns>Instance of the specified type</returns>
		public static T Deserialize<T>(string xml) {
			if (string.IsNullOrEmpty(xml))
				throw new ArgumentNullException("xml");

			T instance;

			using (XmlReader xmlReader = XmlReader.Create(new StringReader(xml)))
			using (XmlDictionaryReader reader = XmlDictionaryReader.CreateDictionaryReader(xmlReader)) {
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				instance = (T)serializer.Deserialize(reader);
			}

			return instance;
		}

		/// <summary>
		/// Deserializes the specified XML string to the specified type, using the <see cref="DataContractSerializer"/>
		/// </summary>
		/// <typeparam name="T">Type to deserialize to</typeparam>
		/// <param name="xml">XML</param>
		/// <returns>Instance of the specified type</returns>
		public static T DeserializeContract<T>(string xml) {
			if (string.IsNullOrEmpty(xml))
				throw new ArgumentNullException("xml");

			T instance;

			using (XmlReader xmlReader = XmlReader.Create(new StringReader(xml)))
			using (XmlDictionaryReader reader = XmlDictionaryReader.CreateDictionaryReader(xmlReader)) {
				var ser = new DataContractSerializer(typeof(T));
				instance = (T)Convert.ChangeType(ser.ReadObject(reader), typeof(T));
			}

			return instance;
		}

		/// <summary>
		/// Determines if the instance is XML serializable
		/// </summary>
		/// <param name="check">Object to be checked</param>
		/// <returns>True if the object type supports XML serialization</returns>
		public static bool IsXmlSerializable(object check) {
			if (check == null) {
				throw new ArgumentNullException("check");
			}

			Type checkType = check.GetType();

			return checkType.IsSerializable;
		}

	}

}
