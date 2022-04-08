namespace VersenyUI
{
    public class Player
    {

        public string Name { get; private set; }
        public int[] dices;

        public Player()
        { 
        
        }
        public Player(string name)
        {
            this.Name = name;
            this.dices = new int[9];
            for (int i = 0; i < dices.Length; i++)
            {
                dices[i] = 0;
            }
        }

        public void AddDice(int value, int position)
        {
            dices[position] = value;
        }
    }
}