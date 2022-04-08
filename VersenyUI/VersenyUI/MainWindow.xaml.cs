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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VersenyUI
{
    public partial class MainWindow : Window
    {
        private const int ROLL_TIME_MS = 3000;
        private const int ROLL360_COUNT_MAIN_AXIS = 3;
        private const int ROLL360_COUNT_RANDOM_AXIS = 5;


        public Random r = new Random();

        Grid gameGrid;
        Grid tableGrid;


        private DiceVisual[] playerDice;
        private DiceVisual[] botDice;

        private Label[][] playerDiceLabels;
        private Label[][] botDiceLabels;

        private Label[] playerFieldPointLabels;
        private Label[] botFieldPointLabels;

        public MainWindow()
        {
            InitializeComponent();

            InitializeDice(out playerDice, out botDice);
            InitializeFieldGrid(playerGrid, out playerDiceLabels, out playerFieldPointLabels);
            InitializeFieldGrid(botGrid, out botDiceLabels, out botFieldPointLabels);
        }

        private void InitializeFieldGrid(Grid grid, out Label[][] diceValues, out Label[] fieldResultLabels)
        {
            diceValues = new Label[9][];
            fieldResultLabels = new Label[9];

            for (int i = 0; i < 9; i++)
            {
                Label label = new Label();
                label.FontSize = 20;
                grid.Children.Add(label);
                Grid.SetRow(label, i);
                Grid.SetColumn(label, 6);
                fieldResultLabels[i] = label;

                diceValues[i] = new Label[5];
                for (int j = 0; j < 5; j++)
                {
                    Label rollLabel = new Label();
                    rollLabel.FontSize = 20;
                    grid.Children.Add(rollLabel);
                    Grid.SetRow(rollLabel, i);
                    Grid.SetColumn(rollLabel, j + 1);
                    diceValues[i][j] = rollLabel;
                }
            }
        }

        private void ShowButtonsForApplicable(int[] rolls, int playerIdx)
        {
            bool zeroed = false;

            var grid = playerIdx == 0 ? playerGrid : botGrid;
            var applicable = DiceGame.ReturnApplicableFieldsCombinations(playerIdx, rolls, out zeroed);
            var rollsCopy = new int[rolls.Length];
            rolls.CopyTo(rollsCopy, 0);

            List<Button> thisRoundButtons = new List<Button>();
            foreach (var item in applicable)
            {
                var btn = new Button();
                grid.Children.Add(btn);
                thisRoundButtons.Add(btn);
                btn.Content = "Rögzít";
                Grid.SetRow(btn, item);
                Grid.SetColumn(btn, 7);

                btn.Click += (s, e) =>
                {
                    if (!zeroed)
                        DiceGame.PlayerStates[playerIdx][item].AssignValuesBestCombination(rollsCopy);
                    else
                        DiceGame.PlayerStates[playerIdx][item].AssignAllZero();
                    SetRollLabels(playerIdx, rollsCopy, item);
                    DeleteRoundButtons();
                    rollDiceButton.IsEnabled = true;
                    // Game ended
                    if (DiceGame.PlayerStates[0].Count(x => x.DiceValues == null) == 0)
                        EndGame();

                    void DeleteRoundButtons()
                    {
                        for (int i = 0; i < thisRoundButtons.Count; i++)
                        {
                            var bt = thisRoundButtons[i];
                            grid.Children.Remove(bt);
                            bt = null;
                        }
                    }

                };
            }
        }

        private void EndGame()
        {
            rollDiceButton.IsEnabled = false;
            rollDiceButton.Visibility = Visibility.Hidden;

            newGameButton.IsEnabled = true;
            newGameButton.Visibility = Visibility.Visible;

            int playerP = DiceGame.PlayerStates[0].Sum(x => x.Points);
            int botP = DiceGame.PlayerStates[1].Sum(x => x.Points);

            playerFinalPointLabel.Content = playerP;
            botFinalPointLabel.Content = botP;

            string s;
            if (playerP > botP)
                s = "Te nyertél!";
            else if (playerP == botP)
                s = "Döntetlen!";
            else
                s = "Vesztettél!";

            wonLabel.Content = s;

            finalPointAITitle.Visibility = Visibility.Visible;
            finalPointTitle.Visibility = Visibility.Visible;
            finalPointPlayerTitle.Visibility = Visibility.Visible;
            wonLabel.Visibility = Visibility.Visible;
        }

        private void SelectForBot(int[] rolls, int playerIdx)
        {
            bool zeroed = false;

            var grid = playerIdx == 0 ? playerGrid : botGrid;
            var applicable = DiceGame.ReturnApplicableFieldsCombinations(playerIdx, rolls, out zeroed);

            if (!zeroed)
            {
                var bestFieldIdx = applicable.OrderByDescending(x => DiceGame.PlayerStates[playerIdx][x].WouldBePointsCombination(rolls)).First();

                DiceGame.PlayerStates[playerIdx][bestFieldIdx].AssignValuesBestCombination(rolls);
                SetRollLabels(playerIdx, rolls, bestFieldIdx);
            }
            else
            {
                var field = DiceGame.PlayerStates[playerIdx][applicable[0]];
                field.AssignAllZero();
                SetRollLabels(playerIdx, field.DiceValues, applicable[0]);
            }

        }

        private void SetRollLabels(int playerIdx, int[] rolls, int fieldIdx)
        {
            Label[] labelRow = (playerIdx == 0 ? playerDiceLabels : botDiceLabels)[fieldIdx];
            var field = DiceGame.PlayerStates[playerIdx][fieldIdx];
            var diceVals = field.DiceValues;
            for (int i = 0; i < diceVals.Length; i++)
            {
                labelRow[i].Content = diceVals[i].ToString();
            }

            Label labelPoint = (playerIdx == 0 ? playerFieldPointLabels : botFieldPointLabels)[fieldIdx];
            labelPoint.Content = field.Points.ToString();

        }

        private async void RollDice(object sender, EventArgs e)
        {
            rollDiceButton.IsEnabled = false;

            var playerRolls = new int[] { r.Next(1, 7), r.Next(1, 7), r.Next(1, 7), r.Next(1, 7), r.Next(1, 7) };
            var botRolls = new int[] { r.Next(1, 7), r.Next(1, 7), r.Next(1, 7), r.Next(1, 7), r.Next(1, 7) };

            Task[] tasks = new Task[]
            {
                RollBotDice(botRolls),
                RollPlayerDice(playerRolls)
            };
            await Task.WhenAll(tasks);

            ShowButtonsForApplicable(playerRolls, 0);
            SelectForBot(botRolls, 1);
        }

        private async void NewGame(object sender, EventArgs e)
        {
            // Reset dice
            Task[] tasks = new Task[]
            {
                RollToOne(playerDice),
                RollToOne(botDice)
            };
            await Task.WhenAll(tasks);



            rollDiceButton.IsEnabled = true;
            rollDiceButton.Visibility = Visibility.Visible;

            newGameButton.IsEnabled = false;
            newGameButton.Visibility = Visibility.Hidden;

            DiceGame.Restart();

            // Clear labels
            foreach (var item in playerDiceLabels)
                foreach (var itemi in item)
                    itemi.Content = "";
            foreach (var item in botDiceLabels)
                foreach (var itemi in item)
                    itemi.Content = "";

            foreach (var item in playerFieldPointLabels)
                item.Content = "";
            foreach (var item in botFieldPointLabels)
                item.Content = "";

            playerFinalPointLabel.Content = "";
            botFinalPointLabel.Content = "";

            finalPointAITitle.Visibility = Visibility.Hidden;
            finalPointTitle.Visibility = Visibility.Hidden;
            finalPointPlayerTitle.Visibility = Visibility.Hidden;
            wonLabel.Visibility = Visibility.Hidden;
        }

        private async Task RollToOne(DiceVisual[] dv)
        {
            List<Task> rollTasks = new List<Task>();
            for (int i = 0; i < botDice.Length; i++)
            {
                if (dv[i].CurrentSide != 1)
                    rollTasks.Add(dv[i].RollToAnimate(1, ROLL_TIME_MS, ROLL360_COUNT_MAIN_AXIS, ROLL360_COUNT_RANDOM_AXIS));
            }
            await Task.WhenAll(rollTasks);
        }

        /// <summary>
        /// Initiates the dice visuals
        /// <br>
        /// Call when initializing window
        /// </summary>
        private void InitializeDice(out DiceVisual[] playerDice, out DiceVisual[] botDice)
        {
            Vector3D scale = new Vector3D(.085, .085, .085);
            playerDice = new DiceVisual[]
            {
                new DiceVisual(viewport3D, new Vector3D(-.4, .05, 0), scale),
                new DiceVisual(viewport3D, new Vector3D(-.2, .05, 0), scale),
                new DiceVisual(viewport3D, new Vector3D(-.5, -.05, 0), scale),
                new DiceVisual(viewport3D, new Vector3D(-.3, -.05, 0), scale),
                new DiceVisual(viewport3D, new Vector3D(-.1, -.05, 0), scale)
            };

            botDice = new DiceVisual[]
            {
                new DiceVisual(viewport3D, new Vector3D(.2, .05, 0), scale),
                new DiceVisual(viewport3D, new Vector3D(.4, .05, 0), scale),
                new DiceVisual(viewport3D, new Vector3D(.1, -.05, 0), scale),
                new DiceVisual(viewport3D, new Vector3D(.3, -.05, 0), scale),
                new DiceVisual(viewport3D, new Vector3D(.5, -.05, 0), scale)
            };
        }

        /// <summary>
        /// Rolls the player's dice
        /// </summary>
        /// <param name="results">The desired sides on the dice</param>
        private async Task RollPlayerDice(int[] results)
        {
            List<Task> rolls = new List<Task>();
            for (int i = 0; i < playerDice.Length; i++)
            {
                rolls.Add(playerDice[i].RollToAnimate(results[i], ROLL_TIME_MS, ROLL360_COUNT_MAIN_AXIS, ROLL360_COUNT_RANDOM_AXIS));
            }
            await Task.WhenAll(rolls);
        }

        /// <summary>
        /// Rolls the bot's dice
        /// </summary>
        /// <param name="results">The desired sides on the dice</param>
        private async Task RollBotDice(int[] results)
        {
            List<Task> rolls = new List<Task>();
            for (int i = 0; i < botDice.Length; i++)
            {
                rolls.Add(botDice[i].RollToAnimate(results[i], ROLL_TIME_MS, ROLL360_COUNT_MAIN_AXIS, ROLL360_COUNT_RANDOM_AXIS));
            }
            await Task.WhenAll(rolls);
        }
    }
}
