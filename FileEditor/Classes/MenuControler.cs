using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FileEditor.Classes
{
    internal class MenuControler
    {
        public Menu MainMenu;

        public MenuControler(Menu mainMenu)
        {
            MainMenu = mainMenu;
        }
        public void VisibileFalseALL(bool login = false)
        {
            foreach (var VARIABLE in MainMenu.Items)
            {
                ((MenuItem)VARIABLE).Visibility=Visibility.Collapsed;
                if(login&& ((MenuItem)VARIABLE).Name== "MILogin")
                    ((MenuItem)VARIABLE).Visibility=Visibility.Visible;
            }
        }
        public void SetVisibilityForMenuItemsByRole(int role)
        {
            VisibileFalseALL();
            foreach (var items in MainMenu.Items)
            {
                if (((MenuItem)items).Tag != null &&
                    Convert.ToInt32(((MenuItem)items).Tag) <= role) 
                {
                    ((MenuItem)items).Visibility=Visibility.Visible;
                }
            }
        }
    }
}
