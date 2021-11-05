using System.Windows;

namespace LandConquest.Logic
{
    public static class WindowCloserLogic
    {
        public static void CloseCurrentWindow(object tag)
        {
            if(tag == null)
            { return; }

            var windows = Application.Current.Windows;

            foreach(Window window in windows)
            {
                if (window.Tag == tag)
                {
                    window.Close();
                }
            }
        }

        public static object GenerateWindowTag()
        {
            System.Random random = new System.Random();
            return random.Next(10, 99);
        }
    }
}
