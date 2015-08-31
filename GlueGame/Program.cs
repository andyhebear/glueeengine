using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GlueEngine.Core;
using System.Runtime.InteropServices;
using Mogre;
using System.IO;

namespace GlueGame
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //try
            //{
                Directory.SetCurrentDirectory(Application.StartupPath);
                GameMain game = new GameMain();
                Engine.Start(game);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "An exception has occured!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    throw;
            //}
        }
    }
}
