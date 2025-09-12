using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Practica.Models;


namespace Practica;

public partial class MainWindow : Window
{
    private ObservableCollection<PropertyPresenter> Properties = new();
    private bool isFiltersVisible = false;
    public bool IsAdminMode { get; private set; }

    public MainWindow()
    {
        InitializeComponent();
        LoadProperties();
        InitializeEventHandlers();
        UpdateUserInterface();
    }

    private void InitializeEventHandlers()
    {
        MenuButton.Click += MenuButton_Click;
        UserSwitchButton.Click += UserSwitchButton_Click;
        Search.TextChanged += OnSearchTextChanged;
        Sort.SelectionChanged += OnSortChanged;
        PriceFromBox.TextChanged += OnFilterTextChanged;
        PriceToBox.TextChanged += OnFilterTextChanged;
        AreaFromBox.TextChanged += OnFilterTextChanged;
        AreaToBox.TextChanged += OnFilterTextChanged;
        Room1CheckBox.Click += OnFilterCheckBoxChanged;
        Room2CheckBox.Click += OnFilterCheckBoxChanged;
        Room3CheckBox.Click += OnFilterCheckBoxChanged;
        Room4PlusCheckBox.Click += OnFilterCheckBoxChanged;
    }

    private void LoadProperties()
    {
        using var context = new DatabaseContext();
        var properties = context.Properties.ToList();
    
        Properties = new ObservableCollection<PropertyPresenter>(
            properties.Select(p => new PropertyPresenter
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                Area = p.Area,
                Address = p.Address,
                Rooms = p.Rooms,
                Floor = p.Floor,
                TotalFloors = p.TotalFloors,
                PhotoPath = p.PhotoPath
            }));
    
        PropertyList.ItemsSource = Properties;
        UpdateRecordsCounter(Properties.Count, properties.Count);
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyAllFilters();
    }

    private void OnFilterTextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyAllFilters();
    }

    private void OnFilterCheckBoxChanged(object sender, RoutedEventArgs e)
    {
        ApplyAllFilters();
    }

    private void OnSortChanged(object sender, SelectionChangedEventArgs e)
    {
        ApplyAllFilters();
    }

    private void ApplyAllFilters()
    {
        if (Properties == null || !Properties.Any()) 
            return;

        IEnumerable<PropertyPresenter> filtered = Properties;
        
        if (!string.IsNullOrWhiteSpace(Search.Text))
        {
            var searchText = Search.Text.ToLower();
            filtered = filtered.Where(p => 
                (p.Title != null && p.Title.ToLower().Contains(searchText)) ||
                (p.Address != null && p.Address.ToLower().Contains(searchText)));
        }
        
        if (decimal.TryParse(PriceFromBox.Text, out decimal minPrice))
        {
            filtered = filtered.Where(p => p.Price >= minPrice);
        }

        if (decimal.TryParse(PriceToBox.Text, out decimal maxPrice))
        {
            filtered = filtered.Where(p => p.Price <= maxPrice);
        }
        
        if (decimal.TryParse(AreaFromBox.Text, out decimal minArea))
        {
            filtered = filtered.Where(p => p.Area >= minArea);
        }

        if (decimal.TryParse(AreaToBox.Text, out decimal maxArea))
        {
            filtered = filtered.Where(p => p.Area <= maxArea);
        }
        
        var selectedRooms = new List<int>();
        if (Room1CheckBox.IsChecked == true) selectedRooms.Add(1);
        if (Room2CheckBox.IsChecked == true) selectedRooms.Add(2);
        if (Room3CheckBox.IsChecked == true) selectedRooms.Add(3);
        if (Room4PlusCheckBox.IsChecked == true) selectedRooms.AddRange(new[] {4, 5, 6, 7, 8, 9, 10});

        if (selectedRooms.Any())
        {
            filtered = filtered.Where(p => selectedRooms.Contains((int)p.Rooms));
        }
        
        filtered = Sort.SelectedIndex switch
        {
            1 => filtered.OrderByDescending(p => p.Price),
            2 => filtered.OrderBy(p => p.Price),          
            _ => filtered                                 
        };
        
        PropertyList.ItemsSource = new ObservableCollection<PropertyPresenter>(filtered);
        UpdateRecordsCounter(filtered.Count(), Properties.Count);
    }
    
    private void ResetFiltersButton_Click(object sender, RoutedEventArgs e)
    {
        PriceFromBox.Text = "";
        PriceToBox.Text = "";
        AreaFromBox.Text = "";
        AreaToBox.Text = "";
        Room1CheckBox.IsChecked = false;
        Room2CheckBox.IsChecked = false;
        Room3CheckBox.IsChecked = false;
        Room4PlusCheckBox.IsChecked = false;
        Search.Text = "";
        Sort.SelectedIndex = 0;
    
        ApplyAllFilters();
    }

    private void UpdateRecordsCounter(int displayed, int total)
    {
        RecordsCounter.Text = $"Показано: {displayed} из {total}";
    }   

    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        if (isFiltersVisible)
        {
            FiltersPanel.Margin = new Thickness(-250, 0, 0, 0);
            isFiltersVisible = false;
        }
        else
        {
            FiltersPanel.Margin = new Thickness(0, 0, 0, 0);
            isFiltersVisible = true;
        }
    }

    private async void UserSwitchButton_Click(object sender, RoutedEventArgs e)
    {
        if (IsAdminMode)
        {
            // Выход из режима администратора
            IsAdminMode = false;
            UpdateUserInterface();
            LoadProperties();
        }
        else
        {
            // Вход в режим администратора
            var password = await ShowPasswordDialog();
            if (password == "0000")
            {
                IsAdminMode = true;
                UpdateUserInterface();
                LoadProperties();
            }
            else if (!string.IsNullOrEmpty(password))
            {
                // Исправленный вызов MessageBox
                var box = MessageBoxManager.GetMessageBoxStandard(
                    "Ошибка", 
                    "Неверный пароль", 
                    ButtonEnum.Ok);
                await box.ShowAsync();
            }
        }
    }
    
    private async Task<string> ShowPasswordDialog()
    {
        var dialog = new PasswordInputWindow("Введите пароль администратора");
        return await dialog.ShowDialog<string>(this);
    }

    private void UpdateUserInterface()
    {
        if (IsAdminMode)
        {
            UserSwitchButton.Content = "👑"; // Корона для админа
            UserSwitchButton.ToolTip.SetTip(UserSwitchButton, "Режим администратора");
            UserSwitchButton.Tag = "admin"; // Для стиля
        }
        else
        {
            UserSwitchButton.Content = "👤"; // Человечек для пользователя
            UserSwitchButton.ToolTip.SetTip(UserSwitchButton, "Режим пользователя");
            UserSwitchButton.Tag = null; // Убираем стиль
        }
        Title = IsAdminMode ? "Property - Администратор" : "Property - Пользователь";
    }
}

public class PropertyPresenter : Property
{
    public Bitmap? Image
    {
        get
        {
            try
            {
                return new Bitmap(PhotoPath);
            }
            catch
            {
                return null;
            }
        }
    }
    
}