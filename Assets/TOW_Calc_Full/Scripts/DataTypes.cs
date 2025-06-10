using System;
using UnityEditor;

namespace TOW_Calc_Full.Scripts
{
    public struct Statblock
    {
        public readonly int M;
        public readonly int WS;
        public readonly int BS;
        public readonly int S;
        public readonly int T;
        public readonly int W;
        public readonly int I;
        public readonly int A;
        public readonly int Ld;

        public Statblock(int M, int WS, int BS, int S, int T, int W, int I, int A, int Ld)
        {
            this.M = M;
            this.WS = WS;
            this.BS = BS;
            this.S = S;
            this.T = T;
            this.W = W;
            this.I = I;
            this.A = A;
            this.Ld = Ld;
        }
    }

    public struct Attack
    {
        public readonly int StatblockIndex;
        public readonly Weapon Weapon;
        public readonly int Initiative;

        public Attack(int statblockIndex, Weapon weapon, int initiative)
        {
            StatblockIndex = statblockIndex;
            Weapon = weapon;
            Initiative = initiative;
        }
    }

    public struct Weapon
    {
        public readonly FancyValue AttackCount;
        public readonly FancyValue Strength;
        public readonly bool FixedStrength;
        public readonly int AP;
        public readonly int ArmorBane;
        public readonly SpecialRule[] SpecialRules;

        public Weapon(FancyValue attackCount, FancyValue strength, bool fixedStrength, int ap, int armorBane,
            SpecialRule[] specialRules)
        {
            AttackCount = attackCount;
            Strength = strength;
            FixedStrength = fixedStrength;
            AP = ap;
            ArmorBane = armorBane;
            SpecialRules = specialRules;
        }
    }

    public struct FancyValue
    {
        private int fixedValue;
        private DiceType[] randomValues;

        public FancyValue(int fixedValue, DiceType[] randomValues = null)
        {
            this.fixedValue = fixedValue;
            this.randomValues = randomValues ?? Array.Empty<DiceType>();
        }

        public int GetValue()
        {
            int randomValue = 0;
            foreach (DiceType die in randomValues)
            {
                switch (die)
                {
                    case DiceType.D3:
                        randomValue += UnityEngine.Random.Range(1, 4);
                        break;
                    case DiceType.D6:
                        randomValue += UnityEngine.Random.Range(1, 7);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return fixedValue + randomValue;
        }
    }


    public enum DiceType
    {
        D3,
        D6,
    }

    public enum TroopType
    {
        LightInfantry,
        HeavyInfantry,
    }

    public enum ModelType
    {
        Champion,
        Unit,
        Character,
    }

    public enum SpecialRule
    {
        None
    }

    public enum Strategy
    {
        Default 
    }

    public enum ModelModifiers
    {
        
    }
}