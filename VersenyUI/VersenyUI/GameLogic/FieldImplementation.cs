using System;
using System.Windows;

namespace VersenyUI
{
    public class FieldImplementation
    {
        private Predicate<int[]> condition;
        private int expectedArraySize;

        public FieldImplementation(Predicate<int[]> condition, int expectedArraySize)
        {
            this.condition = condition;
            this.expectedArraySize = expectedArraySize;
        }

        public bool MatchesCondition(int[] rolls)
        {
            if (rolls.Length != expectedArraySize)
            {
                MessageBox.Show("xd");
                return false;
            }

            return condition(rolls);
        }
    }
}