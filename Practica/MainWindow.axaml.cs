using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Practica.Models;

namespace Practica;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    private ObservableCollection<PropertyPresenter> Properties = new();
    private bool isFiltersVisible = false;

    private bool _eventHandlersInitialized = false;

    private bool _loginWindowOpen = false;
    private bool _addWindowOpen = false;   

    private bool _isAdminMode;
    public bool IsAdminMode
    {
        get => _isAdminMode;
        set
        {
            if (_isAdminMode != value)
            {
                _isAdminMode = value;
                OnPropertyChanged(nameof(IsAdminMode));
                UpdateUserInterface();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public MainWindow()
    {
        InitializeComponent();
        InitializeEventHandlers();
    }

    public async void OpenLoginWindowOnStart(bool isStartup = false)
    {
        if (_loginWindowOpen) return;
        _loginWindowOpen = true;

        var loginWindow = new LoginWindow();
        var result = await loginWindow.ShowDialog<bool?>(this);

        _loginWindowOpen = false;

        if (result == true)
        {
            IsAdminMode = loginWindow.IsAdmin;
            LoadProperties();
        }
        else if (isStartup)
        {
            Close();
        }
    }


    private void UpdateUserInterface()
    {
        Title = IsAdminMode ? "Property - Режим администратора" : "Property - Режим пользователя";

        if (UserSwitchButton != null)
        {
            if (IsAdminMode)
            {
                UserSwitchButton.Content = "👑";
                ToolTip.SetTip(UserSwitchButton, "Режим администратора");
                UserSwitchButton.Classes.Add("admin-mode");
            }
            else
            {
                UserSwitchButton.Content = "👤";
                ToolTip.SetTip(UserSwitchButton, "Режим пользователя");
                UserSwitchButton.Classes.Remove("admin-mode");
            }
        }
    }

    private void InitializeEventHandlers()
    {
        if (_eventHandlersInitialized) return;
        _eventHandlersInitialized = true;

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

        if (AddButton != null)
            AddButton.Click += AddButton_Click;
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
                PhotoPath = p.PhotoPath,
                IsAdmin = IsAdminMode
            }));

        PropertyList.ItemsSource = Properties;
        UpdateRecordsCounter(Properties.Count, properties.Count);
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e) => ApplyAllFilters();
    private void OnFilterTextChanged(object sender, TextChangedEventArgs e) => ApplyAllFilters();
    private void OnFilterCheckBoxChanged(object sender, RoutedEventArgs e) => ApplyAllFilters();
    private void OnSortChanged(object sender, SelectionChangedEventArgs e) => ApplyAllFilters();

    private void ApplyAllFilters()
    {
        if (Properties == null || !Properties.Any()) return;

        IEnumerable<PropertyPresenter> filtered = Properties;

        if (!string.IsNullOrWhiteSpace(Search.Text))
        {
            var searchText = Search.Text.ToLower();
            filtered = filtered.Where(p =>
                (p.Title != null && p.Title.ToLower().Contains(searchText)) ||
                (p.Address != null && p.Address.ToLower().Contains(searchText)));
        }

        if (decimal.TryParse(PriceFromBox.Text, out decimal minPrice))
            filtered = filtered.Where(p => p.Price >= minPrice);

        if (decimal.TryParse(PriceToBox.Text, out decimal maxPrice))
            filtered = filtered.Where(p => p.Price <= maxPrice);

        if (decimal.TryParse(AreaFromBox.Text, out decimal minArea))
            filtered = filtered.Where(p => p.Area >= minArea);

        if (decimal.TryParse(AreaToBox.Text, out decimal maxArea))
            filtered = filtered.Where(p => p.Area <= maxArea);

        var selectedRooms = new List<int>();
        if (Room1CheckBox.IsChecked == true) selectedRooms.Add(1);
        if (Room2CheckBox.IsChecked == true) selectedRooms.Add(2);
        if (Room3CheckBox.IsChecked == true) selectedRooms.Add(3);
        if (Room4PlusCheckBox.IsChecked == true) selectedRooms.AddRange(new[] { 4, 5, 6, 7, 8, 9, 10 });

        if (selectedRooms.Any())
            filtered = filtered.Where(p => selectedRooms.Contains((int)p.Rooms));

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

    private void UserSwitchButton_Click(object sender, RoutedEventArgs e)
    {
        OpenLoginWindowOnStart(isStartup: false);
    }

    private async void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (!IsAdminMode) return;

        try
        {
            if (sender is Button button && button.Tag is PropertyPresenter selectedProperty)
            {
                var editWindow = new EditWindow(selectedProperty);
                var result = await editWindow.ShowDialog<bool>(this);

                if (result)
                {
                    using var context = new DatabaseContext();
                    var dbProperty = context.Properties.FirstOrDefault(p => p.Id == selectedProperty.Id);
                    if (dbProperty != null)
                    {
                        dbProperty.Title = selectedProperty.Title;
                        dbProperty.Description = selectedProperty.Description;
                        dbProperty.Price = selectedProperty.Price;
                        dbProperty.Area = selectedProperty.Area;
                        dbProperty.Address = selectedProperty.Address;
                        dbProperty.Rooms = selectedProperty.Rooms;
                        dbProperty.Floor = selectedProperty.Floor;
                        dbProperty.TotalFloors = selectedProperty.TotalFloors;
                        dbProperty.PhotoPath = selectedProperty.PhotoPath;

                        await context.SaveChangesAsync();
                        LoadProperties();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при редактировании: {ex.Message}");
        }
    }

    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (!IsAdminMode) return;

        try
        {
            if (sender is Button button && button.Tag is PropertyPresenter selectedProperty)
            {
                var deleteWindow = new DeleteWindow(selectedProperty.Title);
                var result = await deleteWindow.ShowDialog<bool>(this);

                if (result)
                {
                    using var context = new DatabaseContext();
                    var dbProperty = context.Properties.FirstOrDefault(p => p.Id == selectedProperty.Id);
                    if (dbProperty != null)
                    {
                        context.Properties.Remove(dbProperty);
                        await context.SaveChangesAsync();
                        LoadProperties();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении: {ex.Message}");
        }
    }

    private async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        if (!IsAdminMode || _addWindowOpen) return;

        _addWindowOpen = true;
        try
        {
            var addWindow = new AddWindow();
            var result = await addWindow.ShowDialog<bool>(this);

            if (result)
            {
                using var context = new DatabaseContext();
                context.Properties.Add(addWindow.NewProperty);
                await context.SaveChangesAsync();
                LoadProperties();
            }
        }
        finally
        {
            _addWindowOpen = false;
        }
    }
    
    private async void DetailsButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is PropertyPresenter selectedProperty)
        {
            Console.WriteLine($"DEBUG: {selectedProperty.Title}, {selectedProperty.Address}, {selectedProperty.Price}, {selectedProperty.Area}, {selectedProperty.Rooms}, {selectedProperty.Floor}/{selectedProperty.TotalFloors}");

            var detailsWindow = new DetailsWindow(selectedProperty);
            await detailsWindow.ShowDialog(this);
        }
    }


}

public class PropertyPresenter : Property
{
    public bool IsAdmin { get; set; }
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
