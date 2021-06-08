using LandConquest.DialogWIndows;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFLocalizeExtension.Engine;

namespace LandConquest
{
    public partial class App : Application
    {
        public App()
        {
            SetupExceptionHandling();
            SetupApplicaionCulture();
        }
        private void App_Exit(object sender, ExitEventArgs e)
        {
            LandConquestYD.YDContext.DeleteConnectionId();
            Environment.Exit(0);
        }

        private void SetupApplicaionCulture()
        {
            var culture = InputLanguageManager.Current.CurrentInputLanguage;
;
            if (culture.Equals("ru") || culture.ToString().Contains("ru"))
            {
                LocalizeDictionary.Instance.Culture = new System.Globalization.CultureInfo("ru");
            }
            else
            {
                LocalizeDictionary.Instance.Culture = new System.Globalization.CultureInfo("en");
            }
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //if (e.Exception.InnerException is SqlException)
            //{
            //    LandConquestDB.DbContext.Reconnect();
            //    return;
            //}
            LandConquestYD.YDContext.DeleteConnectionId();
            string errorMessage = string.Format(" Error: {0}", e.Exception.Message);
            WarningDialogWindow.CallWarningDialogNoResult(e.Exception.Source + errorMessage);
            e.Handled = true;
        }
        private void SetupExceptionHandling()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                //if (e.Exception.InnerException is SqlException)
                //{
                //    LandConquestDB.DbContext.Reconnect();
                //    return;
                //}

                LandConquestYD.YDContext.DeleteConnectionId();
                string errorMessage = string.Format(" Error: {0}", e.Exception.Message);
                WarningDialogWindow.CallWarningDialogNoResult(e.Exception.Source + errorMessage);
                e.SetObserved();
            };
        }
        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;

            //if (e.InnerException is SqlException)
            //{
            //    LandConquestDB.DbContext.Reconnect();
            //    return;
            //}
            LandConquestYD.YDContext.DeleteConnectionId();
            
            string errorMessage = string.Format(" Error: {0}", e.Message);
            WarningDialogWindow.CallWarningDialogNoResult(e.Source + errorMessage);
        }
    }
}
