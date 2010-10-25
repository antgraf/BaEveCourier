using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WindowEntity;

namespace Courier
{
	public partial class Eve
	{
		[TestFixture]
		public class EveOperationsManualTest
		{
			#region Test settings

			private const string pEvePath = @"C:\Games\EveOnlineBot\eve.exe";
			//private const string pEveLogin = "user";
			//private const string pEvePassword = "******";
			private const string pEveLogin = "Invant";
			private const string pEvePassword = "Mypass1";
			
			#endregion

			private Eve pEve = new Eve(null);

			private void AttachToEve()
			{
				WindowsMan.ResetWindows();
				pEve.EveWindow = WindowsMan.AttachTo(Eve.pEveProcessName);
				if(pEve.EveWindow == null)
				{
					throw new NullReferenceException("Cannot attach to Eve.");
				}
			}

			[Test]
			public void Launch()
			{
				pEve.Launch(pEvePath);
			}

			[Test]
			public void DoLogin()
			{
				AttachToEve();
				pEve.DoLogin(pEveLogin, pEvePassword);
			}

			[Test]
			public void Close()
			{
				AttachToEve();
				pEve.Close();
			}
		}
	}
}
