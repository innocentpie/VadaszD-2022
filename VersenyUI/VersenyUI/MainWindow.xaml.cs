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

        public List<string> dobasok = new List<string>();

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
            dobasok.Add("asdasdasd");
            dobasok.Add("asdasd");
            dobasok.Add("asda");
            dobasok.Add("asddasd");
        }
        private void StartGame(System.Object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();
            mainGrid.RowDefinitions.Clear();
            mainGrid.ColumnDefinitions.Clear();
            Grid gameGrid = new Grid();
            gameGrid.Background = new SolidColorBrush(Colors.White);
            gameGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            gameGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) });
            gameGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            gameGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            gameGrid.Children.Add(addTableGrid());
            //gameGrid.Children.Add(addViewPort());
            //gameGrid.Children.Add(addPlayerScores());

            mainGrid.Children.Add(gameGrid);
        }

        private UIElement addPlayerScores()
        {
            throw new NotImplementedException();
        }

        private UIElement addViewPort()
        {
            throw new NotImplementedException();
        }

        private Grid addTableGrid()
        {
            Grid grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(4, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            Random random = new Random();
            for (int i = 0; i < dobasok.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                Button button = new Button();
                button.Margin = new Thickness(2);
                button.MinHeight = 10;
                button.MinWidth = 10;
                button.Content = random.Next(1, 7);
                button.SetValue(Grid.RowProperty, i);
                button.SetValue(Grid.ColumnProperty, 1);
                grid.Children.Add(button);
            }

            DataGrid dataGrid = new DataGrid();

            dataGrid.ItemsSource = null;
            dataGrid.Items.Refresh();
            dataGrid.Margin = new Thickness(20);
            dataGrid.SetValue(Grid.RowSpanProperty, dobasok.Count);


            grid.Background = new SolidColorBrush(Colors.Blue);

            grid.Children.Add(dataGrid);
            grid.SetValue(Grid.RowProperty, 1);

            return grid;
        }
    }
}
