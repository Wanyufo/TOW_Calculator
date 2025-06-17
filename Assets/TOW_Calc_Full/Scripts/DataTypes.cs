using System;
using System.Threading;

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

    // one needs to add one Attack per A value of the model
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
        public readonly FancyValue Strength;
        public readonly bool StrengthIsAModifier;
        public readonly int AP;
        public readonly int ArmorBane;
        public readonly SpecialRule[] SpecialRules;

        public Weapon(FancyValue strength, bool strengthIsAModifier, int ap, int armorBane,
            SpecialRule[] specialRules)
        {
            Strength = strength;
            StrengthIsAModifier = strengthIsAModifier;
            AP = ap;
            ArmorBane = armorBane;
            SpecialRules = specialRules;
        }
    }

    public readonly struct FancyValue
    {
        private readonly int fixedValue;
        private readonly DiceType[] randomValues;

        public FancyValue(int fixedValue, DiceType[] randomValues = null)
        {
            this.fixedValue = fixedValue;
            this.randomValues = randomValues ?? Array.Empty<DiceType>();
        }

        public static implicit operator int(FancyValue value)
        {
            return value.GetValue();
        }

        private int GetValue()
        {
            int randomValue = 0;
            foreach (DiceType die in randomValues)
            {
                randomValue += die switch
                {
                    DiceType.D3 => ThreadSafeRandom.RollD3(),
                    DiceType.D6 => ThreadSafeRandom.RollD6(),
                    _ => throw new ArgumentOutOfRangeException()
                };
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


    // TODO migrate these classes to a separate file to keep this one DataTypes only
    public static class Matrices
    {
        public static readonly int[,] ToHitMatrix = new int[,]
        {
            //     0  1  2  3  4  5  6  7  8  9  10
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //0
            {0, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5}, //1
            {0, 3, 4, 4, 4, 5, 5, 5, 5, 5, 5}, //2
            {0, 2, 3, 4, 4, 4, 4, 5, 5, 5, 5}, //3
            {0, 2, 3, 3, 4, 4, 4, 4, 4, 5, 5}, //4
            {0, 2, 2, 3, 3, 4, 4, 4, 4, 4, 4}, //5
            {0, 2, 2, 3, 3, 3, 4, 4, 4, 4, 4}, //6
            {0, 2, 2, 2, 3, 3, 3, 4, 4, 4, 4}, //7
            {0, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4}, //8
            {0, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4}, //9
            {0, 2, 2, 2, 2, 3, 3, 3, 3, 3, 4}, //10
        };

        public static readonly int[,] ToWoundMatrix = new int[,]

        {
            //     0  1  2  3  4  5  6  7  8  9  10
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //0
            {0, 4, 5, 6, 6, 6, 6, 7, 7, 7, 7}, //1 // 7+ means usually impossible to wound,
            {0, 3, 4, 5, 6, 6, 6, 6, 7, 7, 7}, //2
            {0, 2, 3, 4, 5, 6, 6, 6, 6, 7, 7}, //3
            {0, 2, 2, 3, 4, 5, 6, 6, 6, 6, 7}, //4
            {0, 2, 2, 2, 3, 4, 5, 6, 6, 6, 6}, //5
            {0, 2, 2, 2, 2, 3, 4, 5, 6, 6, 6}, //6
            {0, 2, 2, 2, 2, 2, 3, 4, 5, 6, 6}, //7
            {0, 2, 2, 2, 2, 2, 2, 3, 4, 5, 6}, //8
            {0, 2, 2, 2, 2, 2, 2, 2, 3, 4, 5}, //9
            {0, 2, 2, 2, 2, 2, 2, 2, 2, 3, 4}, //10
        };
    }

    public static class ThreadSafeRandom
    {
        private static readonly ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));

        public static int RollDie(int faces)
        {
            return random.Value.Next(1, faces + 1);
        }

        public static int RollD6()
        {
            return random.Value.Next(1, 7);
        }

        public static int RollD3()
        {
            return random.Value.Next(1, 4);
        }

        // TODO Add other dice rolls, such as: Aritllery, 2D6, 2D6Kh, 2D6Kl etc
    }
}