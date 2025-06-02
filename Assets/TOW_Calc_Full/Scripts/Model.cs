using System;
using System.Numerics;

namespace TOW_Calc_Full.Scripts
{
    public class Model
    {
        public Vector2 Position => _position;

        public Model[] BaseContactEnemies => _baseContactEnemies;

        public Statblock[] StatBlocks => _statBlocks;

        public Attack[] Attacks => _attacks;

        public int Armor => _armor;

        public int Regen => _regen;

        public int Ward => _ward;

        public ModelType Type => _type;

        public TroopType TroopType => _troopType;

        public SpecialRule[] SpecialRules => _specialRules;

        public Strategy Strategy => _strategy;


        private Vector2 _position;
        private Model[] _baseContactEnemies;
        private Statblock[] _statBlocks;
        private Attack[] _attacks;
        private int _armor;
        private int _regen;
        private int _ward;
        private ModelType _type;
        private TroopType _troopType;
        private SpecialRule[] _specialRules;
        private Strategy _strategy;

        public void Attack(int initiative, LocalSimulationContext context)
        {
            throw new NotImplementedException("Attack method is not implemented.");
        }

        public Model(Vector2 position, Statblock[] statBlocks, Attack[] attacks, byte armor,
            byte regen, byte ward, ModelType type, TroopType troopType, SpecialRule[] specialRules, Strategy strategy)
        {
            _position = position;
            _statBlocks = statBlocks;
            _attacks = attacks;
            _armor = armor;
            _regen = regen;
            _ward = ward;
            _type = type;
            _troopType = troopType;
            _specialRules = specialRules;
            _strategy = strategy;
        }

        public void CalculateBaseContactEnemies()
        {
            throw new NotImplementedException("CalculateBaseContactEnemies method is not implemented.");
        }
    }
}