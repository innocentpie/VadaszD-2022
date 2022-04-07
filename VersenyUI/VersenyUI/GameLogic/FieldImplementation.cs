using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;

namespace VersenyUI
{    
    public class Field
    {
        public string Name { get; protected set; } //The name of the field.
        protected int NumberOfDice; //Expected number of dice in this field.
        protected int[] DiceValues { get; set; } //The dice that were given to this field.

        public int Points => //The number of points this field is worth.
            overriddenPointValue == null 
            ? DiceValues.Sum() 
            : (int)overriddenPointValue;

        
        int? overriddenPointValue = null; //Contains the point value that the Field was given.
        //If left null, the point value will just be the sum of all given dice values.

        protected Predicate<int[]> CheckConditionPredicate;
        
        protected bool CheckCondition(int[] dice)
        {
            if (dice.Length != NumberOfDice)
                return false;

            if (CheckConditionPredicate == null)
                return true;

            return CheckConditionPredicate.Invoke(dice);
        }

        public bool AssignValues(int[] _dice)
        {
            if(!CheckCondition(_dice))
                return false;

            DiceValues = _dice;
            return true;
        }

        public Field(string _name, int _numberOfDice, Predicate<int[]> _checkConditionPredicate = null, int? _overriddenPointValue = null)
        {
            Name = _name;
            CheckConditionPredicate = _checkConditionPredicate;
            NumberOfDice = _numberOfDice;
            overriddenPointValue = _overriddenPointValue;
        }
    }
    public static class FieldPredicates
    {
        public static Predicate<int[]> Pár =
            dice => dice.Count(x => x == dice[0]) == 2;

        public static Predicate<int[]> Drill =
            dice => dice.Count(x => x == dice[0]) == 3;

        public static Predicate<int[]> DuplaPár =
            dice =>
            {
                List<int> sorted = dice.ToList();
                sorted.Sort();

                return
                    sorted.Count(x => sorted[0] == x) == 2 &&
                    sorted.Count(x => sorted[2] == x) == 2;
            };
        
        public static Predicate<int[]> KisPóker =
            dice => dice.Count(x => x == dice[0]) == 4;

        public static Predicate<int[]> Full =
            dice =>
            {
                List<int> sorted = dice.ToList();
                sorted.Sort();

                return
                    sorted.Count(x => sorted[0] == x) == 2 &&
                    sorted.Count(x => sorted[2] == x) == 3;
            };

        public static Predicate<int[]> KisSor =
            dice =>
            {
                List<int> sorted = dice.ToList();
                sorted.Sort();

                for (int i = 1; i < 6; i++)
                {
                    if (sorted[i - 1] != i)
                        return false;
                }
                return true;
            };

        public static Predicate<int[]> NagySor =
            dice =>
            {
                List<int> sorted = dice.ToList();
                sorted.Sort();

                for (int i = 2; i < 7; i++)
                {
                    if (sorted[i - 2] != i)
                        return false;
                }
                return true;
            };

        public static Predicate<int[]> NagyPóker =
            dice => dice.Count(x => x == dice[0]) == 5;
    }
}