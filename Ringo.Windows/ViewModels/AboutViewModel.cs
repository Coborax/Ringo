using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ringo.Windows.ViewModels
{
    public class AboutViewModel : Screen
    {
        private string _licenses = "";

        public AboutViewModel()
        {
            _licenses = Properties.Resources.licenses;
        }

        public string Licenses
        {
            get
            {
                return _licenses;
            }
        }
    }
}
