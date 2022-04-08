using System;
using System.Collections.Generic;
using System.Linq;

namespace VersenyUI
{
    public class DiceGame
    {
        public static Field[] InstantiateGameFields => new Field[]
        {
            new Field("Szemét", 5),
            new Field("Pár", 2, FieldPredicates.Pár),
            new Field("Drill", 3, FieldPredicates.Drill),
            new Field("Dupla pár", 4, FieldPredicates.DuplaPár),
            new Field("Kis póker", 4, FieldPredicates.KisPóker),
            new Field("Full", 5, FieldPredicates.Full),
            new Field("Kis sor", 5, FieldPredicates.KisSor, 15),
            new Field("Nagy sor", 5, FieldPredicates.NagySor, 20),
            new Field("Nagy póker", 5, FieldPredicates.NagyPóker, 50)
        };

        public static Field[][] PlayerStates = new Field[][]
        {
            InstantiateGameFields, 
            InstantiateGameFields
        };

        public static void Restart()
        {
            PlayerStates = new Field[][]
            {
                InstantiateGameFields,
                InstantiateGameFields
            };
        }

        public static int[] ReturnApplicableFields(int playerIndex, int[] dice)
        {
            List<int> output = new List<int>();
            for (int i = 0; i < PlayerStates[0].Length; i++)
                if(PlayerStates[playerIndex][i].CheckCondition(dice))
                    output.Add(i);

            return output.ToArray();
        }

        public static int[] ReturnApplicableFieldsCombinations(int playerIndex, int[] dice, out bool zeroed)
        {
            List<int> output = new List<int>();
            for (int i = 0; i < PlayerStates[0].Length; i++)
                if (PlayerStates[playerIndex][i].CheckConditionCombinations(dice) && PlayerStates[playerIndex][i].DiceValues == null)
                    output.Add(i);
                else if(PlayerStates[playerIndex][i].CheckConditionCombinations(dice))
                    Console.WriteLine();
            zeroed = false;
            if (output.Count == 0)
            {
                zeroed = true;
                for (int i = 0; i < PlayerStates[0].Length; i++)
                    if (PlayerStates[playerIndex][i].DiceValues == null)
                        output.Add(i);
            }

            return output.ToArray();
        }
    }
}
