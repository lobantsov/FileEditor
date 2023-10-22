﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FileEditor.Classes;
using Xceed.Wpf.Toolkit;

namespace FileEditor
{
    public partial class MainWindow : Window
    {
        private DBManager dbManager;
        private int roleIndex;
        MenuControler menuControler;
        private ChangeDataByModerator ByModerator;

        void VisibilitySetings(object sender)
        {
            GridUserManage.Visibility = Visibility.Collapsed;
            if (sender != null)
                ((Grid)sender).Visibility = Visibility.Visible;
        }
        public MainWindow()
        {
            InitializeComponent();
            dbManager = DBManager.getInstance(
                @"Server=localhost;Database=working_programs_kfkte;Uid=root;Pwd=LAS03312005LAS");
            menuControler = new MenuControler(MainMenu);
            ByModerator = new ChangeDataByModerator(DGUserInfo,new TextBox[]{TBName,TBMail,TBPosition},CBRole);
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
    }
}