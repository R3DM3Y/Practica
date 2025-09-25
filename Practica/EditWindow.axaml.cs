using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Practica.Models;
using System;
using System.IO;

namespace Practica;

public partial class EditWindow : Window
{
    private Property _property;

    public EditWindow()
    {
        InitializeComponent();
    }

    public EditWindow(Property property) : this()
    {
        _property = property;
        FillFields();
    }

    private void FillFields()
    {
        TitleBox.Text = _property.Title;
        DescriptionBox.Text = _property.Description ?? "";
        AddressBox.Text = _property.Address;
        PriceBox.Text = _property.Price.ToString();
        AreaBox.Text = _property.Area.ToString();
        RoomsBox.Text = _property.Rooms.ToString();
        FloorBox.Text = _property.Floor.ToString();
        TotalFloorsBox.Text = _property.TotalFloors.ToString();
        PhotoPathBox.Text = _property.PhotoPath ?? "";
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (!ValidateInputs())
            return;

        _property.Title = TitleBox.Text;
        _property.Description = DescriptionBox.Text;
        _property.Address = AddressBox.Text;
        _property.Price = decimal.Parse(PriceBox.Text);
        _property.Area = decimal.Parse(AreaBox.Text);
        _property.Rooms = int.Parse(RoomsBox.Text);
        _property.Floor = int.Parse(FloorBox.Text);
        _property.TotalFloors = int.Parse(TotalFloorsBox.Text);
        _property.PhotoPath = PhotoPathBox.Text;

        Close(true); // успешно сохранили
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
