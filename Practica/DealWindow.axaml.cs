using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Practica
{
    public partial class DealWindow : Window
    {
        // Публичный конструктор без параметров для Avalonia
        public DealWindow()
        {
            InitializeComponent();
        }

        // Конструктор с параметрами для ручного создания окна
        public DealWindow(string propertyTitle, string phone) : this()
        {
            // Проверяем, что элементы уже созданы
            if (TitleTextBlock != null)
                TitleTextBlock.Text = propertyTitle ?? "";

            if (PhoneTextBlock != null)
                PhoneTextBlock.Text = phone ?? "";
        }
        
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}



