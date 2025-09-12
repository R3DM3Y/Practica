using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Practica;

public partial class LoginWindow : Window
{
    private const string AdminPassword = "0000";
    
    public bool IsAdmin { get; private set; }
    public bool IsAuthenticated { get; private set; }

    public LoginWindow()
    {
        InitializeComponent();
    }

    private void UserModeButton_Click(object sender, RoutedEventArgs e)
    {
        IsAdmin = false;
        IsAuthenticated = true;
        Close();
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
            IsAuthenticated = true;
            Close();
        }
        else
        {
            var errorMsg = new Window
            {
                Title = "Ошибка",
                Content = new TextBlock { Text = "Неверный пароль!" },
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            errorMsg.ShowDialog(this);
        }
    }
}