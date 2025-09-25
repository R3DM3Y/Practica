using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Practica;

public partial class DeleteWindow : Window
{
    public DeleteWindow()
    {
        InitializeComponent();
    }

    public DeleteWindow(string propertyTitle) : this()
    {
        MessageText.Text = $"Вы уверены, что хотите удалить объект:\n\"{propertyTitle}\"?\n\nЭто действие нельзя отменить!";
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        Close(true); // Возвращаем true при удалении
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close(false); // Возвращаем false при отмене
    }
}