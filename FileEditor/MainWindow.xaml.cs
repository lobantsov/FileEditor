using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using FileEditor.Classes;
using FileEditor.Classes.ClassForComponents;
using FileEditor.Classes.SaveDoc;
using Org.BouncyCastle.Utilities;
using Xceed.Wpf.Toolkit;
using RichTextBox = System.Windows.Controls.RichTextBox;


namespace FileEditor
{
    public partial class MainWindow : Window
    {
        private DBManager dbManager;
        private int roleIndex;
        MenuControler menuControler;
        private ChangeDataByModerator ByModerator;
        private List<RichTextBoxParagraph> TextBoxParagraphs = new List<RichTextBoxParagraph>();
        private int i = 0;
        private SaveDocument saveDocument;

        public MainWindow()
        {
            InitializeComponent();
            dbManager = DBManager.getInstance(
                @"Server=localhost;Database=working_programs_kfkte;Uid=root;Pwd=LAS03312005LAS");
            menuControler = new MenuControler(MainMenu);
            ByModerator = new ChangeDataByModerator(DGUserInfo, new TextBox[] { TBName, TBMail, TBPosition }, CBRole);
            RichTextBoxParagraph.Canvas = Canvas;
            saveDocument = new SaveDocument();
            RichTextBoxParagraph.TextBoxParagraphs = TextBoxParagraphs;
            RichTextBoxParagraph.ComboBox = CBFontSize;
            CBFontSize.Items.Add(12);
            CBFontSize.Items.Add(14);
            CBFontSize.Items.Add(16);
            CBFontSize.SelectedIndex = 0;
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

        private void Canvas_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (var VARIABLE in TextBoxParagraphs)
            {
                VARIABLE.GetAborner().AbornerVisibility(Visibility.Collapsed);
            }
            if (ToggleButtonText.IsChecked == true)
            {
                using (var textBox = new RichTextBoxParagraph())
                {
                    textBox.CreateRichTextBox(e.GetPosition(Canvas));
                    textBox.SetTagRichTextBox(i);
                }
                i++;
            }

            ToggleButtonText.IsChecked = false;
        }

        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            //search
            
        }

        private void CBFontSize_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TextBoxParagraphs.Count > 0) 
            foreach (var VARIABLE in TextBoxParagraphs)
            {
                if (VARIABLE.GetAborner().GetAdornerdVisibility()==Visibility.Visible)
                {
                    if (double.TryParse(CBFontSize.SelectedItem.ToString(), out double fontSize))
                    {
                        VARIABLE.richTextBox.FontSize = fontSize;
                    }
                    break;
                }
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            saveDocument.SaveFile(TextBoxParagraphs
                .SelectMany(paragraph => new[] { paragraph.richTextBox })
                .ToList());
        }

        private void TBRight_OnClick(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Name == "TBRight")
            {
                foreach (var VARIABLE in TextBoxParagraphs)
                {
                    if (VARIABLE.Sender != null)
                    {
                        Canvas.SetLeft(((RichTextBox)VARIABLE.Sender),
                            (Canvas.Width / 2) - ((RichTextBox)VARIABLE.Sender).Width);
                        VARIABLE.TextAlignment = "Right";
                        break;
                    }
                }
            }
            else if(((Button)sender).Name == "TBLeft")
            {
                foreach (var VARIABLE in TextBoxParagraphs)
                {
                    if (VARIABLE.Sender != null)
                    {
                        Canvas.SetLeft(((RichTextBox)VARIABLE.Sender),
                            Canvas.Width - ((RichTextBox)VARIABLE.Sender).Width);
                        VARIABLE.TextAlignment = "Left";
                        break;
                    }
                }
            }
            else
            {
                foreach (var VARIABLE in TextBoxParagraphs)
                {
                    if (VARIABLE.Sender != null)
                        Canvas.SetLeft(((RichTextBox)VARIABLE.Sender),
                            (Canvas.Width / 2) - ((RichTextBox)VARIABLE.Sender).Width / 2);
                    VARIABLE.TextAlignment = "Centure";
                    break;
                }
            }
        }
    }
}
