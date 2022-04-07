using System;
using System.Collections.Generic;
using System.Linq;

namespace VersenyUI
{
    public class DiceGame
    {
        public Field[] GameFields = new Field[] 
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
    }
}
