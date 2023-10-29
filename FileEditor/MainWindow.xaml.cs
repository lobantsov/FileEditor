using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using FileEditor.Classes;
using FileEditor.Classes.ClassForComponents;
using Org.BouncyCastle.Utilities;
using Xceed.Wpf.Toolkit;


namespace FileEditor
{
    public partial class MainWindow : Window
    {
        private DBManager dbManager;
        private int roleIndex;
        MenuControler menuControler;
        private ChangeDataByModerator ByModerator;
        private int i = 0;
        private RichTextBoxParagraph TextBoxParagraph;

        public MainWindow()
        {
            InitializeComponent();
            dbManager = DBManager.getInstance(
                @"Server=localhost;Database=working_programs_kfkte;Uid=root;Pwd=LAS03312005LAS");
            menuControler = new MenuControler(MainMenu);
            ByModerator = new ChangeDataByModerator(DGUserInfo, new TextBox[] { TBName, TBMail, TBPosition }, CBRole);
            TextBoxParagraph = new RichTextBoxParagraph();
            RichTextBoxParagraph.Canvas = Canvas;

        }


        void VisibilitySetings(object sender)
        {
            GridUserManage.Visibility = Visibility.Collapsed;
            if (sender != null)
                ((Grid)sender).Visibility = Visibility.Visible;
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Owner = this;
            if (login.ShowDialog() == true)
            {
                var tmpList = dbManager.selectAll("UserTable", login.TBEmail.Text);
                roleIndex = (int)tmpList[1];
                menuControler.SetVisibilityForMenuItemsByRole(roleIndex);
            }
        }

        private void MILogount_OnClick(object sender, RoutedEventArgs e)
        {
            menuControler.VisibileFalseALL(true);
            VisibilitySetings(null);
        }

        private void MIManageRole_OnClick(object sender, RoutedEventArgs e)
        {
            VisibilitySetings(GridUserManage);
            ByModerator.Reconect();
        }

        private void BTDeleteUser_OnClick(object sender, RoutedEventArgs e)
        {
            ByModerator.DeleteRecord("UserTable");
            ByModerator.Reconect();
            ByModerator.ClearText();
        }

        private void TBSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ByModerator.SearchInGrid(TBSearch.Text);
        }

        void ToggleButtonsOfLikeRadioButtons(object sender)
        {
            switch (((ToggleButton)sender).Tag.ToString())
            {
                case "1":
                    ToggleButtonCenture.IsChecked = false;
                    ToggleButtonLeft.IsChecked = false;
                    ToggleButtonRight.IsChecked = false;
                    ((ToggleButton)sender).IsChecked = true;
                    break;


                case "2":
                    ToggleButtonSimplList.IsChecked = false;
                    ToggleButtonNumberList.IsChecked = false;
                    ((ToggleButton)sender).IsChecked = true;
                    break;
            }
        }

        private void ToggleButtonRight_OnChecked(object sender, RoutedEventArgs e)
        {
            ToggleButtonsOfLikeRadioButtons(sender);
        }

        private void Canvas_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ToggleButtonText.IsChecked == true)
            {
                TextBoxParagraph.CreateRichTextBox(e.GetPosition(Canvas));
            }

            ToggleButtonText.IsChecked = false;
        }
    }

}
