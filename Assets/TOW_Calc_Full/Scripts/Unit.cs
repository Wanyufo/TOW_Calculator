
using System.Collections.Generic;

namespace TOW_Calc_Full.Scripts
{
    public class Unit
    {
        public Model[] Models { get; }
        public bool Standard { get; }
        public bool Musician { get; }
        
        public Unit(Model[] models, bool standard, bool musician)
        {
            Models = models;
            Standard = standard;
            Musician = musician;
        }
        
    }
}