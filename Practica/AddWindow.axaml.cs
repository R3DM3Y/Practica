using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Practica.Models;
using System;
using System.IO;

namespace Practica;

public partial class AddWindow : Window
{
    public Property NewProperty { get; private set; }

    public AddWindow()
    {
        InitializeComponent();
        NewProperty = new Property();
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (!ValidateInputs())
            return;

        NewProperty.Title = TitleBox.Text;
        NewProperty.Description = DescriptionBox.Text;
        NewProperty.Address = AddressBox.Text;
        NewProperty.Price = decimal.Parse(PriceBox.Text);
        NewProperty.Area = decimal.Parse(AreaBox.Text);
        NewProperty.Rooms = int.Parse(RoomsBox.Text);
        NewProperty.Floor = int.Parse(FloorBox.Text);
        NewProperty.TotalFloors = int.Parse(TotalFloorsBox.Text);
        NewProperty.PhotoPath = PhotoPathBox.Text;

        Close(true);
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close(false);
    }

    private bool ValidateInputs()
    {
        if (string.IsNullOrWhiteSpace(TitleBox.Text))
        {
            ShowError("Название не может быть пустым!");
            TitleBox.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(AddressBox.Text))
        {
            ShowError("Адрес не может быть пустым!");
            AddressBox.Focus();
            return false;
        }

        if (!decimal.TryParse(PriceBox.Text, out decimal price) || price < 0)
        {
            ShowError("Цена должна быть положительным числом!");
            PriceBox.Focus();
            return false;
        }

        if (!decimal.TryParse(AreaBox.Text, out decimal area) || area < 0)
        {
            ShowError("Площадь должна быть положительным числом!");
            AreaBox.Focus();
            return false;
        }

        if (!int.TryParse(RoomsBox.Text, out int rooms) || rooms <= 0)
        {
            ShowError("Количество комнат должно быть положительным числом!");
            RoomsBox.Focus();
            return false;
        }

        if (!int.TryParse(FloorBox.Text, out int floor) || floor < 0)
        {
            ShowError("Этаж должен быть неотрицательным числом!");
            FloorBox.Focus();
            return false;
        }

        if (!int.TryParse(TotalFloorsBox.Text, out int totalFloors) || totalFloors < 0)
        {
            ShowError("Всего этажей должно быть неотрицательным числом!");
            TotalFloorsBox.Focus();
            return false;
        }

        if (!string.IsNullOrWhiteSpace(PhotoPathBox.Text) && !File.Exists(PhotoPathBox.Text))
        {
            ShowError("Указанный файл изображения не найден!");
            PhotoPathBox.Focus();
            return false;
        }

        return true;
    }

    private async void ShowError(string message)
    {
        var errorWindow = new Window
        {
            Title = "Ошибка",
            Width = 300,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new TextBlock
            {
                Text = message,
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Margin = new Avalonia.Thickness(20),
                FontSize = 14
            }
        };

        await errorWindow.ShowDialog(this);
    }
}
