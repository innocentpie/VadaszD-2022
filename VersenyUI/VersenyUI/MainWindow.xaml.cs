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
        public Random random = new Random();
        Player player = new Player();
        public List<Player> players = new List<Player>();
        public List<string> dobasTipusok = new List<string>(9);
        public List<string> dobas = new List<string>();

        public List<Button> buttons = new List<Button>();


        public int activePlayer = 0;


        public int theDice = 0;


        Grid gameGrid;
        Grid tableGrid;



        public MainWindow()
        {
            InitializeComponent();


            /*this.Width = Math.Round(SystemParameters.FullPrimaryScreenWidth * 0.665);
            this.Height = Math.Round(SystemParameters.FullPrimaryScreenHeight * 0.665);
            Button playButton = new Button();
            playButton.Content = "Játék";
            playButton.SetValue(Grid.RowProperty, 1);
            playButton.SetValue(Grid.ColumnProperty, 1);
            playButton.Click += new RoutedEventHandler(StartGame);
            mainGrid.Children.Add(playButton);

            players.Add(new Player("Player1"));
            players.Add(new Player("Bot"));

            dobasTipusok.Add("Szemét");
            dobasTipusok.Add("Pár");
            dobasTipusok.Add("Drill");
            dobasTipusok.Add("Dupla pár");
            dobasTipusok.Add("Kis póker");
            dobasTipusok.Add("Full");
            dobasTipusok.Add("Kis sor");
            dobasTipusok.Add("Nagy sor");
            dobasTipusok.Add("Nagy póker");*/
        }
        private void StartGame(System.Object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();
            mainGrid.RowDefinitions.Clear();
            mainGrid.ColumnDefinitions.Clear();
            gameGrid = new Grid();
            gameGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            gameGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) });
            gameGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            gameGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            tableGrid = addTableGrid();
            gameGrid.Children.Add(tableGrid);



            gameGrid.Children.Add(addViewPort());
            gameGrid.Children.Add(addPlayerScores());

            mainGrid.Children.Add(gameGrid);
        }

        private UIElement addPlayerScores()
        {
            Grid grid = new Grid();

            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            Border border = new Border();
            border.Background = new SolidColorBrush(Colors.White);

            border.CornerRadius = new CornerRadius(0,0,6,6);

            Label scoreTable = new Label();
            scoreTable.FontSize = 18;
            border.HorizontalAlignment = HorizontalAlignment.Center;
            border.VerticalAlignment = VerticalAlignment.Center;
            for (int i = 0; i < players.Count; i++)
            {
                scoreTable.Content += i == players.Count-1 ? $"{players[i].Name}" : $"{players[i].Name} vs ";
            }

            border.Child = scoreTable;
            border.Margin = new Thickness(0);

            grid.Children.Add(border);
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(3, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) });
            grid.Background = new SolidColorBrush(Colors.Red);
            grid.SetValue(Grid.RowProperty, 1);
            grid.SetValue(Grid.ColumnProperty, 1);

            Button diceButton = new Button();
            diceButton.Content = "Dobás";
            diceButton.SetValue(Grid.RowProperty, 1);
            diceButton.Click += DiceButton_Click;
            grid.Children.Add(diceButton);

            return grid;
        }

        private void DiceButton_Click(object sender, RoutedEventArgs e)
        {
            theDice = random.Next(1, 7);
            MessageBox.Show($"The Dice: {theDice}");
        }

        private UIElement addViewPort()
        {
            // majd ide jön a ViewPort.
            Label l = new Label();
            l.Background = new SolidColorBrush(Colors.Aquamarine);
            l.SetValue(Grid.ColumnSpanProperty, 2);
            return l;
        }

        private Grid addTableGrid()
        {
            Grid grid = new Grid();
            grid.SetValue(Grid.RowProperty, 1);
            // Nevek legenerálása
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            for (int i = 0; i < players.Count; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                Label label = new Label();
                label.Content = players[i].Name;
                label.SetValue(Grid.ColumnProperty, i + 1);
                label.VerticalAlignment = VerticalAlignment.Center;
                label.HorizontalAlignment = HorizontalAlignment.Center;
                grid.Children.Add(label);
            }
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            // Értékek neveinek legenerálása illetve a gombok
            for (int i = 0; i < dobasTipusok.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                Label label = new Label();
                label.SetValue(Grid.RowProperty, i + 1);
                label.Content = dobasTipusok[i].ToString();
                grid.Children.Add(label);


                for (int j = 0; j < players.Count; j++)
                {
                    Label label2 = new Label();
                    label2.Content = players[j].dices[i];
                    label2.SetValue(Grid.ColumnProperty, j+1);
                    label2.SetValue(Grid.RowProperty, i + 1);
                    label2.VerticalAlignment = VerticalAlignment.Center;
                    label2.HorizontalAlignment = HorizontalAlignment.Center;
                    grid.Children.Add(label2);
                }
                
                // Gombok a táblába való betételhez
                Button button = new Button();
                button.Margin = new Thickness(5);
                button.MinHeight = 10;
                button.MinWidth = 10;
                button.Content = "Belerak";
                button.SetValue(Grid.RowProperty, i + 1);
                button.SetValue(Grid.ColumnProperty, players.Count+1);
                button.Click += Button_Click;
                buttons.Add(button);
                grid.Children.Add(button);
            }

            return grid;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (theDice != 0)
            { 
                players[activePlayer].AddDice(theDice, Convert.ToInt32(button.GetValue(Grid.RowProperty))-1);
            }
            else
            {
                MessageBox.Show("Először dobj a kockával.");
            }
            activePlayer = ++activePlayer % 3;
            RefreshTable();
        }
        public void RefreshTable()
        {
            gameGrid.Children.Remove(tableGrid);
            tableGrid = addTableGrid();
            gameGrid.Children.Add(tableGrid);
        }
    }
}
