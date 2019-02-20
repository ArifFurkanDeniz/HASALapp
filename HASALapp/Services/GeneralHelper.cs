using System;
using Xamarin.Forms;

namespace HASALapp.Services
{
    public static class GeneralHelper
    {
      
            public static bool IsNotFirstLogin
        {
            get;
            set;
        }

        public static string NotificationToken
        {
            get;
            set;
        }

        public static bool IsModal(this ContentPage page)
        {
            for (int i = 0; i < page.Navigation.ModalStack.Count; i++)
            {
                if (page == page.Navigation.ModalStack[i])
                    return true;
            }
            return false;
        }
    }
}
