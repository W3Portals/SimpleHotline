using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.ComponentModel;
using SimpleHotline.Extensions;
using System.Net.Mail;
using System.Net;

namespace SimpleHotline.Controllers
{
	public class HomeController : Controller
	{
        Models.HotlineDataContext db = new Models.HotlineDataContext();

		public ActionResult Index(string CallGuid, string Caller, string CallStatus) {
			
            Log(CallGuid, "Call from " + Caller + ": " + CallStatus);
			var doc = new XDocument();
			var response = new XElement("Response");
            //var say = Verb("Say", "Hello, leave a message after the beep");

            response.Add(Verb("Say", "Hello, leave a message after the beep"));
            response.Add(Verb("Record", "", new
            {
                maxLength = 250,
                action = ActionUrl.RecordVoicemail,
                transcribe = "true", 
                transcribeCallback=ActionUrl.Transcribed
            }));

            response.Add(Verb("Say", "Good Bye."));
            response.Add(Verb("Hangup", ""));
            doc.Add(response);
			return new XmlResult(doc);
		}

        public ActionResult RecordVoicemail(string CallGuid, string RecordingUrl, string Caller)
        {
            Log(CallGuid, "Recorded voicemail: " + RecordingUrl);
          
            var doc = new XDocument();
            var response = new XElement("Response");
            response.Add(Verb("Say", "Thank you"));
            doc.Add(response);
            return new XmlResult(doc);
        }

        public ActionResult Transcribed(string CallGuid, string RecordingUrl, string Caller, string TranscriptionText, string TranscriptionStatus)
        {
            Log(CallGuid, "Recorded voicemail: " + RecordingUrl);

                var call = new Models.Episode();
                call.Name = Caller;
                call.Filename = RecordingUrl;
                call.Downloads = 1;
                call.Description = TranscriptionText;
                call.ShowId = 1;
                call.Created = DateTime.Now;
                call.Updated = DateTime.Now;
                db.Episodes.InsertOnSubmit(call);
                db.SubmitChanges();

                Log(CallGuid, "msg:" + TranscriptionText);

                var doc = new XDocument();
                var response = new XElement("Response");
                doc.Add(response);
                return new XmlResult(doc);
            //  return new EmptyResult();
        }

		public ActionResult Greeting(string CallGuid, string Digits) {
			var doc = new XDocument();
			var response = new XElement("Response");

			if (Digits != Settings.PIN) {
				response.Add(Verb("Say", "Invalid pin number. Please try again."));
				response.Add(Verb("Gather", "", new { action = ActionUrl.Greeting, method = "POST", finishOnKey = "#" }));
				Log(CallGuid, "Failed attempt to log in (wrong PIN)");
			}
			else {
				response.Add(Verb("Say", "Record your greeting after the tone. Hang up to save the greeting or press a key to start over."));
				response.Add(Verb("Record", "", new { maxLength = 120, action = ActionUrl.RecordGreeting, method = "POST" }));
				Log(CallGuid, "Successful log in, prompted to record greeting");
			}

			doc.Add(response);
			return new XmlResult(doc);
		}

		public ActionResult RecordGreeting(string CallGuid, string RecordingUrl, string Digits) {
			if (Digits != "hangup") {
				Log(CallGuid, "Restarted greeting recording");

				var doc = new XDocument();
				var response = new XElement("Response");
				response.Add(Verb("Say", "Record your greeting after the tone. Hang up to save the greeting or press a key to start over."));
				response.Add(Verb("Record", "", new { maxLength = 120, action = ActionUrl.RecordGreeting, method = "POST" }));
				doc.Add(response);
				return new XmlResult(doc);
			}

			Settings.GreetingUrl = RecordingUrl;
			Log(CallGuid, "New greeting saved: " + RecordingUrl);

			return Content(Settings.GreetingUrl);
		}

		private XElement Verb(string verb, string value) {
			return Verb(verb, value, null);
		}

		private XElement Verb(string verb, string value, object paramObject) {
			var element = new XElement(verb, value);
			foreach (var item in paramObject.ToDictionary()) {
				element.Add(new XAttribute(item.Key, item.Value));
			}

			return element;
		}

		private void Log(string callId, string message) {
			string path = Server.MapPath("~/App_Data/log.txt");
			string formatted = string.Format("{0}\t{1}\t{2}{3}", callId, DateTime.Now.ToString(), message, Environment.NewLine);
			System.IO.File.AppendAllText(path, formatted);
		}
	}
}
