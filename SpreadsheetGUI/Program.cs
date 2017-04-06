using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SS
{
    /// <summary>
    /// Keeps track of how many top-level forms are running
    /// </summary>
    class SpreadsheetContext : ApplicationContext
    {
        // Number of open forms
        private int formCount = 0;

        // Singleton ApplicationContext
        private static SpreadsheetContext appContext;

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private SpreadsheetContext()
        {
        }

        /// <summary>
        /// Returns the one ApplicationContext.
        /// </summary>
        public static SpreadsheetContext getAppContext()
        {
            if (appContext == null)
            {
                appContext = new SpreadsheetContext();
            }
            return appContext;
        }

        /// <summary>
        /// Runs the form
        /// </summary>
        public void RunForm(Form form)
        {
            // One more form is running
            formCount++;

            // When this form closes, we want to find out
            form.FormClosed += (o, e) => { if (--formCount <= 0) ExitThread(); };

            // Run the form
            form.Show();
        }

    }


    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start an application context and run one form inside it
            SpreadsheetContext appContext = SpreadsheetContext.getAppContext();
            appContext.RunForm(new SSForm());
            Application.Run(appContext);

        }
    }
}
