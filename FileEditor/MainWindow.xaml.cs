using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FileEditor.Classes;
using FileEditor.Classes.ClassForComponents;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;
using RichTextBox = System.Windows.Controls.RichTextBox;

namespace FileEditor
{
    public partial class MainWindow : Window
    {
        private DBManager dbManager;
        private int roleIndex;
        MenuControler menuControler;
        private ChangeDataByModerator ByModerator;
        private List<ParagraphSetings> ParagraphSetingsList = new List<ParagraphSetings>();
        private int i = 0;

        public MainWindow()
        {
            InitializeComponent();
            dbManager = DBManager.getInstance(
                @"Server=localhost;Database=working_programs_kfkte;Uid=root;Pwd=LAS03312005LAS");
            menuControler = new MenuControler(MainMenu);
            ByModerator = new ChangeDataByModerator(DGUserInfo, new TextBox[] { TBName, TBMail, TBPosition }, CBRole);
            RTBEditText.Style = null;
            ParagraphSetings.FlowDocument = FlowDocument;
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

        private void FlowDocument_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ToggleButtonText.IsChecked == true)
            {
                using (ParagraphSetings paragraphSetings = new ParagraphSetings())
                {
                    paragraphSetings.CreateParagraph(new StringBuilder("Тут може бути ваш текст"));
                    paragraphSetings.paragraph.Name="Parag_"+i.ToString();
                    ParagraphSetingsList.Add(paragraphSetings);
                }

                i++;
            }

            string qwe = string.Empty;
            foreach (var VARIABLE in FlowDocument.Blocks)
            {
                qwe += VARIABLE.Name+"  ";
            }

            Title = qwe;
            ToggleButtonText.IsChecked = false;
        }
    }
}
