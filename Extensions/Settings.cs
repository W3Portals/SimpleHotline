using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace SimpleHotline
{
	public class Settings
	{
		private static XDocument doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/settings.xml"));

		private static string Get(string key) {
			return doc.Descendants(key).FirstOrDefault().Value;
		}

		private static void Set(string key, string value) {
			doc.Root.SetElementValue(key, value);
			doc.Save(HttpContext.Current.Server.MapPath("~/App_Data/settings.xml"));
		}

		public static string GreetingUrl {
			get {
				return Get("greetingUrl");
			}
			set {
				Set("greetingUrl", value);
			}
		}
		public static string PIN {
			get {
				return Get("pin");
			}
		}
		public static string VoicemailEmailFromAddress {
			get {
				return Get("voicemailEmailFromAddress");
			}
		}
		public static string VoicemailEmailToAddress {
			get {
				return Get("voicemailEmailToAddress");
			}
		}
	}
}
