using LandConquest.DialogWIndows;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace LandConquest
{
    public partial class App : Application
    {
        public App()
        {
            SetupExceptionHandling();
        }
        private void App_Exit(object sender, ExitEventArgs e)
        {
            LandConquestYD.YDContext.DeleteConnectionId();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            LandConquestYD.YDContext.DeleteConnectionId();
            string errorMessage = string.Format(" Error: {0}", e.Exception.Message);
            WarningDialogWindow.CallWarningDialogNoResult(e.Exception.HResult.ToString() + errorMessage);
            e.Handled = true;
        }
        private void SetupExceptionHandling()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                LandConquestYD.YDContext.DeleteConnectionId();
                string errorMessage = string.Format(" Error: {0}", e.Exception.Message);
                WarningDialogWindow.CallWarningDialogNoResult(e.Exception.HResult.ToString() + errorMessage);
                e.SetObserved();
            };
        }
        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            LandConquestYD.YDContext.DeleteConnectionId();
            Exception e = (Exception)args.ExceptionObject;
            string errorMessage = string.Format(" Error: {0}", e.Message);
            WarningDialogWindow.CallWarningDialogNoResult(e.HResult.ToString() + errorMessage);
        }
    }
}
