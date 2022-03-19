using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VersenyUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Width = Math.Round(SystemParameters.FullPrimaryScreenWidth * 0.665);
            this.Height = Math.Round(SystemParameters.FullPrimaryScreenHeight * 0.665);
            Button playButton = new Button();
            playButton.Content = "Játék";
            playButton.SetValue(Grid.RowProperty, 1);
            playButton.SetValue(Grid.ColumnProperty, 1);
            playButton.Click += new RoutedEventHandler(StartGame);
            mainGrid.Children.Add(playButton);
        }
        private void StartGame(System.Object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();
            Grid gameGrid = new Grid();
            gameGrid.SetValue(Grid.ColumnSpanProperty, 3);
            gameGrid.SetValue(Grid.RowSpanProperty, 2);
            gameGrid.Background = new SolidColorBrush(Colors.White);
            gameGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            gameGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            gameGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            gameGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            gameGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            gameGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(10, GridUnitType.Star) });
            gameGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(15, GridUnitType.Star) });
            gameGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(60, GridUnitType.Star) });
            gameGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(15, GridUnitType.Star) });

            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                Button button = new Button();
                button.Margin = new Thickness(10);
                button.MinHeight = 10;
                button.MinWidth = 10;
                button.Content = random.Next(1, 7);
                button.SetValue(Grid.RowProperty, i);
                button.SetValue(Grid.ColumnProperty, 0);
                gameGrid.Children.Add(button);
            }

            DataGrid table = new DataGrid();
            table.Background = Brushes.White;
            table.SetValue(Grid.RowProperty, 1);
            table.SetValue(Grid.RowSpanProperty, 3);
            table.SetValue(Grid.ColumnProperty, 2);
            gameGrid.Children.Add(table);


            mainGrid.Children.Add(gameGrid);
            ASDA.xdddddddd(10);

            ASD();
        }


        private void ASD()
        {
            MessageBox.Show("XD");
            
        }
    }
}
