using System;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace TOW_Calc_Full.Scripts
{
    public class MockUnityScene
    {
        public Unit[] GetSideAUnits()
        {
            Model[] models1 = new Model[10];
            // create 10 models in a 2x5 grid

            for (int i = 0; i < models1.Length / 2; i++)
            {
                models1[i] = MockModel1(new Vector2(i, 0));
            }
            for (int i = models1.Length / 2; i < models1.Length; i++)
            {
                models1[i] = MockModel1(new Vector2(i - models1.Length / 2, 1));
            }

            // Place all models of unit 2 directly adjacent to the right of unit 1
            Model[] models2 = new Model[5];
            for (int i = 0; i < models2.Length; i++)
            {
                // Instead of i + 10, use i + 5 so B starts at (5,1) and is adjacent to A
                models2[i] = MockModel2(new Vector2(i + models1.Length / 2, 1));
            }

            Unit unit1 = new Unit(models1, true, true);
            Unit unit2 = new Unit(models2, true, true);
            return new[] { unit1, unit2 };
        }

// Units C and D adjacent
        public Unit[] GetSideBUnits()
        {
            Model[] models1 = new Model[10];
            // create 10 models in a 2x5 grid

            for (int i = 0; i < models1.Length / 2; i++)
            {
                models1[i] = MockModel1(new Vector2(i, 2));
            }
            for (int i = models1.Length / 2; i < models1.Length; i++)
            {
                models1[i] = MockModel1(new Vector2(i - models1.Length / 2, 3));
            }

            // Place all models of unit 2 directly adjacent to the right of unit 1
            Model[] models2 = new Model[5];
            for (int i = 0; i < models2.Length; i++)
            {
                // Instead of i + 10, use i + 5 so D starts at (5,2) and is adjacent to C
                models2[i] = MockModel2(new Vector2(i + models1.Length / 2, 2));
            }

            Unit unit1 = new Unit(models1, true, true);
            Unit unit2 = new Unit(models2, true, true);
            return new[] { unit1, unit2 };
        }


        /// <summary>
        /// Logs a visual representation of the units' positions on a grid, with labeled axes.
        /// Each model of unit1 will be represented by 'A', unit2 by 'B', etc. Empty cells are '.'
        /// This version aggregates output in a string, compatible with Unity Debug.Log().
        /// </summary>
        public void LogUnitPositions(Unit[] units)
        {
            // Gather positions and determine grid size
            List<(char symbol, Vector2 pos)> modelPositions = new List<(char, Vector2)>();
            char[] unitSymbols = new char[] {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'};

            int maxX = 0, maxY = 0;

            for (int u = 0; u < units.Length; u++)
            {
                var models = units[u].Models;
                char symbol = unitSymbols[u % unitSymbols.Length];
                foreach (var model in models)
                {
                    Vector2 pos = model.Position;
                    modelPositions.Add((symbol, pos));
                    if (pos.X > maxX) maxX = (int) pos.X;
                    if (pos.Y > maxY) maxY = (int) pos.Y;
                }
            }

            // Build and initialize grid
            char[,] grid = new char[maxY + 1, maxX + 1];
            for (int y = 0; y <= maxY; y++)
            for (int x = 0; x <= maxX; x++)
                grid[y, x] = '.';

            foreach (var (symbol, pos) in modelPositions)
            {
                grid[(int) pos.Y, (int) pos.X] = symbol;
            }

            // Aggregate output in a string
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // X axis labels
            sb.Append("    "); // Padding for Y axis
            for (int x = 0; x <= maxX; x++)
            {
                sb.Append(x.ToString().PadLeft(2) + " ");
            }

            sb.AppendLine();

            // Grid with Y axis labels
            for (int y = 0; y <= maxY; y++)
            {
                sb.Append(y.ToString().PadLeft(2) + "  "); // Y axis label
                for (int x = 0; x <= maxX; x++)
                {
                    sb.Append(grid[y, x] + "  ");
                }

                sb.AppendLine();
            }

            // To Unity Console
            Debug.Log(sb.ToString());
        }

        public Battle GetBattle()
        {
            // Mock a battle with two sides, each with two units
            Unit[] sideAUnits = GetSideAUnits();
            Unit[] sideBUnits = GetSideBUnits();
            return new Battle(sideAUnits, sideBUnits);
        }


        private Model MockModel1(Vector2 position = default)
        {
            return new Model(
                position: position,
                statBlocks: new[] {MockStatblock1()},
                attacks: new[] {MockAttack1()},
                armor: 2,
                regen: 0,
                ward: 0,
                modelType: ModelType.Unit,
                troopType: TroopType.HeavyInfantry,
                specialRules: Array.Empty<SpecialRule>(),
                strategy: Strategy.None
            );
        }

        private Model MockModel2(Vector2 position = default)
        {
            return new Model(
                position: position,
                statBlocks: new[] {MockStatblock2()},
                attacks: new[] {MockAttack2()},
                armor: 2,
                regen: 0,
                ward: 0,
                modelType: ModelType.Unit,
                troopType: TroopType.HeavyInfantry,
                specialRules: Array.Empty<SpecialRule>(),
                strategy: Strategy.None
            );
        }

        private Attack MockAttack1()
        {
            // Mock an attack with some basic stats
            return new Attack
            (
                statblockIndex: 0,
                weapon: new Weapon
                (
                    attackCount: new FancyValue(1),
                    strength: new FancyValue(3),
                    strengthIsAModifier: true,
                    ap: 0,
                    armorBane: 0,
                    specialRules: Array.Empty<SpecialRule>()
                ),
                initiative: 4
            );
        }

        private Attack MockAttack2()
        {
            // Mock an attack with some basic stats
            return new Attack
            (
                statblockIndex: 0,
                weapon: new Weapon
                (
                    attackCount: new FancyValue(2),
                    strength: new FancyValue(1),
                    strengthIsAModifier: false,
                    ap: 1,
                    armorBane: 0,
                    specialRules: Array.Empty<SpecialRule>()
                ),
                initiative: 3
            );
        }


        private Statblock MockStatblock1()
        {
            // Mock a statblock with some basic stats
            return new Statblock
            (
                M: 3,
                WS: 3,
                BS: 3,
                S: 3,
                T: 3,
                W: 1,
                I: 3,
                A: 1,
                Ld: 7
            );
        }

        private Statblock MockStatblock2()
        {
            // Mock a statblock with some basic stats
            return new Statblock
            (
                M: 2,
                WS: 4,
                BS: 3,
                S: 3,
                T: 4,
                W: 1,
                I: 2,
                A: 1,
                Ld: 9
            );
        }

        private Statblock MockStatblock3()
        {
            // Mock a statblock with some basic stats
            return new Statblock
            (
                M: 6,
                WS: 2,
                BS: 2,
                S: 2,
                T: 2,
                W: 1,
                I: 6,
                A: 5,
                Ld: 5
            );
        }

        private Statblock MockStatblock4()
        {
            // Mock a statblock with some basic stats
            return new Statblock
            (
                M: 5,
                WS: 6,
                BS: 4,
                S: 4,
                T: 4,
                W: 3,
                I: 3,
                A: 3,
                Ld: 10
            );
        }
    }
}