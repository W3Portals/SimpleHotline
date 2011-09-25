using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace SimpleHotline.Extensions
{
	public static class ExtensionMethods
	{
		public static Dictionary<string, object> ToDictionary(this object o) {
			var dict = new Dictionary<string, object>();
			if (o != null) {
				PropertyDescriptorCollection props = TypeDescriptor.GetProperties(o);
				foreach (PropertyDescriptor prop in props) {
					object val = prop.GetValue(o);
					if (val != null) {
						dict.Add(prop.Name, val);
					}
				}
			}
			return dict;
		}
	}
}
