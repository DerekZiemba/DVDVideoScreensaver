﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVDScreenSaver {
	static class Program {


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
      System.Diagnostics.Process myProcess = System.Diagnostics.Process.GetCurrentProcess();
      myProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
      Application.Run(new DVDScreenSaver());
		}


	}
}
