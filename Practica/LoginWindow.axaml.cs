using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Practica;

public partial class LoginWindow : Window
{
    private const string AdminPassword = "0000";
    
    public bool IsAdmin { get; private set; }
    public bool? DialogResult { get; private set; }

    public LoginWindow()
    {
        InitializeComponent();
    }

    private void UserModeButton_Click(object sender, RoutedEventArgs e)
    {
        IsAdmin = false;
        Close(true); // Возвращаем true
    }

    private void AdminModeButton_Click(object sender, RoutedEventArgs e)
    {
        PasswordPanel.IsVisible = true;
    }

    private void AdminLoginButton_Click(object sender, RoutedEventArgs e)
    {
        if (PasswordBox.Text == AdminPassword)
        {
            IsAdmin = true;
            Close(true); // Возвращаем true
        }
        else
        {
            PasswordBox.Text = "";
            PasswordBox.Focus();
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close(false); // Возвращаем false
    }

}