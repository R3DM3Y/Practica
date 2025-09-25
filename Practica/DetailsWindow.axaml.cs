using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Practica.Models;

namespace Practica
{
    public partial class DetailsWindow : Window
    {
        public PropertyPresenter Property { get; }  // поле для хранения объекта

        public DetailsWindow(PropertyPresenter property) : this()
        {
            Property = property;         // присваиваем
            DataContext = Property;      // биндим данные к окну
        }

        public DetailsWindow()
        {
            InitializeComponent();
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (Property == null) return;

            var phone = "**********";  // пример
            var dealWindow = new DealWindow(Property.Title, phone);
            dealWindow.ShowDialog(this);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }


}