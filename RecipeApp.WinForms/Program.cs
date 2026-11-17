using System;
using System.Windows.Forms;

namespace RecipeApp.WinForms
{
    /// <summary>
    /// Точка входа для приложения Windows Forms.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}