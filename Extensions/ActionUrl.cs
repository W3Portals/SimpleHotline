using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleHotline.Extensions
{
	public class ActionUrl
	{
		public static string Main {
			get {
				return string.Format("http://{0}/", HttpContext.Current.Request.Url.Host);
			}
		}
		public static string RecordVoicemail {
			get {
				return string.Format("http://{0}/RecordVoicemail", HttpContext.Current.Request.Url.Host);
			}
		}
		public static string Greeting {
			get {
				return string.Format("http://{0}/Greeting", HttpContext.Current.Request.Url.Host);
			}
		}
		public static string RecordGreeting {
			get {
				return string.Format("http://{0}/RecordGreeting", HttpContext.Current.Request.Url.Host);
			}
		}
        public static string Transcribed
        {
            get
            {
                return string.Format("http://{0}/Transcribed", HttpContext.Current.Request.Url.Host);
            }
        }
	}
}
