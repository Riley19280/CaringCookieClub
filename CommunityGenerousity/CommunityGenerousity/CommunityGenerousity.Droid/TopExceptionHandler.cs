using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Net.Mail;
using System.Net;

namespace CaringCookieClub
{
	public class TopExceptionHandler
	{
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
		public static void Main()
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

		}

		static void MyHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception e = (Exception)args.ExceptionObject;
			Console.WriteLine("EXCEPTION CAUGHT : \n\n\n" + e.Message+ "\n\n\n\n"+e.StackTrace);

			#if !DEBUG
			sendMail("MyHandler caught : " + e.Message + "\n\n\n" + e.StackTrace + "\n\n\n\n" + e.Source + e.TargetSite);
			#endif
		}
		static	void sendMail(string message)
		{
			string smtpAddress = "smtp.mail.yahoo.com";
			int portNumber = 587;
			bool enableSSL = true;

			string emailFrom = "glitterunicorn@rocketmail.com";
			string password = "glitterunicorn@rocketmail.com";
			string emailTo = "glitterunicorn@rocketmail.com";

			using (MailMessage mail = new MailMessage())
			{
				mail.From = new MailAddress(emailFrom);
				mail.To.Add(emailTo);
				mail.Subject ="CCC CRASH REPORT";
				mail.Body = message;
				mail.IsBodyHtml = false;
				// Can set to false, if you are sending pure text.

				using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
				{
					smtp.Credentials = new NetworkCredential(emailFrom, password);
					smtp.EnableSsl = enableSSL;
					smtp.Send(mail);
				}
			}
		}
	}
}