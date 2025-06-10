using System;
using System.Linq;
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
            // check if the Model has any attacks at this initiative
            foreach (Attack attack in _attacks)
            {
                if (attack.Initiative == initiative)
                { 
                    bool hasAttacked = false;
                    
                    // if there are any models in base contact and alive, attack these, otherwise fall back to attacking unit instead
                    // TODO Check for priority targets and priority units
                    // -> add checks to see if there are priority targets in the base contact enemies, and if one unit is preferred for attacking, or if a unit is already killed to a point where no attacks can be made
                    if (_baseContactEnemies != null && _baseContactEnemies.Length > 0)
                    {
                        
                        // collect valid targets:
                        foreach (Model baseContactEnemy in _baseContactEnemies)
                        {
                            

                        }

                        foreach (Model enemy in
                                 _baseContactEnemies) // replace all basecontact enemies with only the ones that are a target regarding to the strategy. Or the unit
                        {
                            // check to see if there is a delta for the enemy in context.ModelDeltaContext,
                            if (context.ModelDeltaContext.TryGetValue(enemy,
                                    out ModelDelta enemyDelta)) // if we find it, it might be dead
                            {
                                if (enemyDelta.Killed)
                                {
                                    continue; // ignore killed targets
                                }
                                else
                                {
                                    // do the attack
                                }
                            }
                            else // the model is not dead for sure
                            {
                            }

                            // Perform attack logic against each enemy
                            // This is a placeholder for the actual attack logic
                            Console.WriteLine($"Attacking {enemy} at initiative {initiative}");
                        }
                    }
                    else
                    {
                        // Fall back to attacking the unit instead
                        Console.WriteLine($"No base contact enemies, attacking unit at initiative {initiative}");
                    }


                    throw new NotImplementedException("Attack logic is not implemented.");
                }
            }
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