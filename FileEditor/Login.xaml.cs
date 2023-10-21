﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FileEditor
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private Style reg;
        private EmailService service = new EmailService();
        private string password;
        private string passwprdAgain;
        private bool mailStatus = false, passwordStatus = false;
        private DBManager dbManager;
        private string email;
        private bool passwordCheck = false;
        private bool SwitchWindows = true;

        public Login()
        {
            InitializeComponent();
            reg = BTRegistration.Style;
            dbManager = DBManager.getInstance(
                @"Server=localhost;Database=working_programs_kfkte;Uid=root;Pwd=LAS03312005LAS");
        }
        private void VisibilitySetings(object sender, bool modeButton = false)
        {
            SPLogin.Visibility = Visibility.Collapsed;
            SPRegistration.Visibility = Visibility.Collapsed;
            GridButtons.Visibility = Visibility.Collapsed;
            SPForgotPassword.Visibility = Visibility.Collapsed;
            SPConfirmReg.Visibility = Visibility.Collapsed;
            SPSetNewPassword.Visibility = Visibility.Collapsed;

            ((StackPanel)sender).Visibility = Visibility.Visible;

            if (modeButton)
            {
                GridButtons.Visibility = Visibility.Visible;
            }

        }
        private void BTRegistration_OnClick(object sender, RoutedEventArgs e)
        {
            VisibilitySetings(SPRegistration, true);

            BTLogin.Style = (Style)FindResource("MaterialDesignFlatButton");
            BTRegistration.Style = reg;
        }
        private void BTLogin_OnClick(object sender, RoutedEventArgs e)
        {
            VisibilitySetings(SPLogin, true);


            BTLogin.Style = reg;
            BTRegistration.Style = (Style)FindResource("MaterialDesignFlatButton");
        }
        private void LBForgotpasswor_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            VisibilitySetings(SPForgotPassword);
        }
        private void LBRememberPassword_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            VisibilitySetings(SPLogin, true);
        }
        private async void BTRegist_OnClick(object sender, RoutedEventArgs e)
        {
            if (!dbManager.ExistRecord("UserTable", TBEmailReg.Text))
            {
                email = TBEmailReg.Text.Trim().ToLower();
                if (password != passwprdAgain)
                {
                    TBPasswordAgainReg.ToolTip = "Password uncorect";
                    TBPasswordAgainReg.Foreground = Brushes.DarkRed;
                    passwordStatus = false;
                }
                else
                {
                    TBPasswordAgainReg.ToolTip = string.Empty;
                    TBPasswordAgainReg.Foreground = Brushes.Black;
                    passwordStatus = true;
                }

                if ((!email.Contains('@') || !email.Contains('.')))
                {
                    TBEmailReg.ToolTip = "Email uncorect";
                    TBEmailReg.Foreground = Brushes.DarkRed;
                    mailStatus = false;
                }
                else
                {
                    TBEmailReg.ToolTip = string.Empty;
                    TBEmailReg.Foreground = Brushes.Black;
                    mailStatus = true;
                }

                if (mailStatus && passwordStatus && TBLoginReg.Text != string.Empty &&
                    TBPositionReg.Text != string.Empty && TBPasswordReg.Password != string.Empty)
                {
                    VisibilitySetings(SPConfirmReg);
                    await service.SendMessageAsync(email);
                }
            }
            else
            {
                MessageBox.Show("Такий користувач вже існує");
            }
        }
        private void TBPasswordAgainReg_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            passwprdAgain = TBPasswordAgainReg.Password.Trim();
            if (password != passwprdAgain)
            {
                ((PasswordBox)sender).Foreground = Brushes.DarkRed;
            }
            else
            {
                ((PasswordBox)sender).Foreground = Brushes.Black;
            }
        }
        private void TBPasswordReg_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            password = TBPasswordReg.Password.Trim();
        }
        private void BTConfirmMailKey_OnClick(object sender, RoutedEventArgs e)
        {
            if (service.CheckPasswords(TBKeyConfirm.Text))
            {
                if (SwitchWindows)
                {

                    dbManager.InsertDataWithBinary("usertable",
                        new[] { "FullName", "PositionInCollege", "Mail" },
                        new string[] { TBLoginReg.Text, TBPositionReg.Text, email },
                        "UserPassword",
                        service.HashPassword(password));
                    MessageBox.Show("Ви успішно зареєструвались, тепер увійдіть у свторений акаунт");
                    VisibilitySetings(SPLogin, true);
                }
                else
                {
                    SwitchWindows = true;
                    VisibilitySetings(SPSetNewPassword);
                }
            }
            else
            {
                TBKeyConfirm.Foreground = Brushes.DarkRed;
            }
        }
        private void TBKeyConfirm_OnTextInput(object sender, TextCompositionEventArgs e)
        {
            TBKeyConfirm.Foreground = Brushes.Black;
        }
        private void BTRunLogin_OnClick(object sender, RoutedEventArgs e)
        {
            if (dbManager.ExistRecord("UserTable", TBEmail.Text))
            {
                var Record = dbManager.selectAll("UserTable", TBEmail.Text);
                var password = service.HashPassword(TBPassword.Password);
                for (int i = 0; i < password.Length-1; i++)
                {
                    if (password[i] == ((byte[])Record[^1])[i])
                    {
                        passwordCheck = true;
                    }
                    else
                    {
                        passwordCheck = false;
                    }
                }

                if (passwordCheck)
                {
                    MessageBox.Show("Ви успішно авторизувались");
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    TBPassword.Foreground= Brushes.DarkRed;
                }
            }
            else
            {
                MessageBox.Show("Ви не зареєстровані");
            }
        }
        private async void BTResetPassword_OnClick(object sender, RoutedEventArgs e)
        {
            if (dbManager.ExistRecord("UserTable", TBEmailForgotPassword.Text))
            {
                SPForgotPassword.Visibility = Visibility.Collapsed;
                SPConfirmReg.Visibility = Visibility.Visible;
                SwitchWindows = false;
                await service.SendMessageAsync(TBEmailForgotPassword.Text.ToLower());
            }
        }
        private void BTConfirnUpdataPassword_OnClick(object sender, RoutedEventArgs e)
        {
            if (TBNewPassword.Password == TBNewPasswordAgain.Password)
            {
                if (dbManager.UpdateFieldValueBinary("UserTable", "UserPassword",
                        service.HashPassword(TBNewPassword.Password),
                        $"Mail = '{TBEmailForgotPassword.Text}'"))
                {
                    MessageBox.Show("Пароль успішно змінений");
                    VisibilitySetings(SPLogin,true);
                }
            }
        }
        private void TBPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ((PasswordBox)sender).Foreground = Brushes.Black;
        }
    }
}
