using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Practica;

public partial class PasswordInputWindow : Window
{
    public PasswordInputWindow()
    {
        InitializeComponent();
    }

    public PasswordInputWindow(string title) : this()
    {
        Title = title;
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        var passwordBox = this.FindControl<TextBox>("PasswordBox");
        Close(passwordBox.Text);
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close(null);
    }
}