using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CalculationManager : MonoBehaviour
{
    [SerializeField] private GameObject _barPrefab;

    // singleton instance
    public static CalculationManager Instance { get; private set; }

    // for final combat result calc
    private int _myUnitSize;
    private int _enemyUnitSize;

    // initial fighting rank size
    private int _myFightingRankSize;
    private int _enemyFightingRankSize;

    // running tally of deaths, used to get amount of attacking models during the round

    private StatBlock[] _myUnitStats;
    private StatBlock[] _enemyUnitStats;

    static int _rounds = 100000;
    List<CombatResult> _combatResults = new List<CombatResult>();

    private void Start()
    {
        Stopwatch stopwatch = new Stopwatch();

        SetupMockStats();

        stopwatch.Start();

        for (int i = 0; i < _rounds; i++)
        {
            _combatResults.Add(SimulateCombat());
        }

        stopwatch.Stop();
        Debug.Log("Time elapsed: " + stopwatch.ElapsedMilliseconds + "ms");

        // do statistics:
        int myWins = _combatResults.Count(result => result.result > 0);
        int enemyWins = _combatResults.Count(result => result.result < 0);
        int draws = _combatResults.Count(result => result.result == 0);

        double averageDamageInflicted = _combatResults.Average(result => result.DamageCaused);
        double averageDamageTaken = _combatResults.Average(result => result.DamageSuffered);
        averageDamageTaken = Math.Round(averageDamageTaken, 2);
        averageDamageInflicted = Math.Round(averageDamageInflicted, 2);

        int maxDamageInflicted = _combatResults.Max(result => result.DamageCaused);
        int maxDamageSuffered = _combatResults.Max(result => result.DamageSuffered);


        int[] damageInflictedHistogram = new int[maxDamageInflicted + 1];
        int[] damageSufferedHistogram = new int[maxDamageSuffered + 1];

        foreach (CombatResult result in _combatResults)
        {
            damageInflictedHistogram[result.DamageCaused]++;
            damageSufferedHistogram[result.DamageSuffered]++;
        }

        double[] DamageInflictedInPercent = new double[damageInflictedHistogram.Length];
        // noralize the histograms
        for (int i = 0; i < damageInflictedHistogram.Length; i++)
        {
            DamageInflictedInPercent[i] = (damageInflictedHistogram[i] / (double) _rounds) * 100f;
        }

        Debug.Log("damage inflicted histogram" + string.Join(",", DamageInflictedInPercent));


        // plot the histograms
        for (int i = 0; i < DamageInflictedInPercent.Length; i++)
        {
            CreatePlotPoint(i, DamageInflictedInPercent[i]);
        }


        Debug.Log($"Average damage inflicted: {averageDamageInflicted}, Average damage taken: {averageDamageTaken}");
        Debug.Log($"My wins: {myWins}, Enemy wins: {enemyWins}, Draws: {draws}");
    }

    // TODO create a "createPlot" method that takes a list of values and creates a plot
    private void CreatePlotPoint(int index, double value)
    {
        // todo sacle width to amount of data points, or decide on a fixed max width
        // summarize the values above the max width 
        double scaledValue = value / 10;
        GameObject plotBar = Instantiate(_barPrefab);
        GameObject bar = plotBar.transform.Find("Bar").gameObject;
        bar.transform.localScale = new Vector3(1, (float) scaledValue, 1);
        bar.transform.position = new Vector3(0.5f + index * 1.1f, (float) scaledValue / 2f, 0);
        Canvas canvas = plotBar.transform.Find("Canvas").GetComponent<Canvas>();
        // set Canvas hight to value
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(1, (float) scaledValue);
        // set canvas position according to index
        canvas.transform.position =
            new Vector3(0.5f + index * 1.1f, (float) scaledValue / 2f, 0); // TODO precalc the values

        canvas.transform.Find("Index").GetComponent<TMP_Text>().text = index.ToString();
        canvas.transform.Find("Value").GetComponent<TMP_Text>().text = Math.Round(value, 2).ToString();
    }

    // ====================================================================================================
    // ## simulate a single combat Combat ##

    private CombatResult SimulateCombat()
    {
        // for each initiative value (10 to 1) (impact at 10, stomps at 1)
        // for each side that has attacks, for all attacks
        // lost possible attacks / count attacks
        // to hit (auto hit for impact, stomps)
        // to wound 
        // armor save (AP & Bane)
        // ward save
        // regen save
        // remove models that died


        int myTotalWoundsCaused = 0;
        int myTotalWoundsSuffered = 0;
        int myTotalDamageInflicted = 0;
        int myTotalDamageTaken = 0;
        int myCurrentFightingRankSize = _myFightingRankSize;
        int enemyCurrentFightingRankSize = _enemyFightingRankSize;

        for (int currentInit = 10; currentInit >= 1; currentInit--)
        {
            (int myWoundsCaused, int myDamageInflicted) =
                CalculateCombatResult(_myUnitStats, _enemyUnitStats, currentInit, myCurrentFightingRankSize);
            (int myWoundsSuffered, int myDamageTaken) =
                CalculateCombatResult(_enemyUnitStats, _myUnitStats, currentInit, enemyCurrentFightingRankSize);

            // update Statistics
            myTotalDamageInflicted += myDamageInflicted;
            myTotalDamageTaken += myDamageTaken;
            myTotalWoundsCaused += myWoundsCaused;
            myTotalWoundsSuffered += myWoundsSuffered;

            // reduce front Rank
            myCurrentFightingRankSize =
                Math.Max((myCurrentFightingRankSize - myDamageTaken), 0); // TODO adapt for multiple wounds unit
            enemyCurrentFightingRankSize =
                Math.Max((enemyCurrentFightingRankSize - myDamageInflicted), 0); // TODO adapt for multiple wounds unit
        } // end for

        CombatResult result = new CombatResult()
        {
            WoundsSuffered = myTotalWoundsSuffered,
            WoundsCaused = myTotalWoundsCaused,
            result = myTotalWoundsCaused - myTotalWoundsSuffered,

            DamageSuffered = myTotalDamageTaken,
            DamageCaused = myTotalDamageInflicted,

            MyDamageSufferedInPercent = ((float) myTotalDamageTaken / _myUnitSize) * 100,
            MyDamageCausedInPercent = ((float) myTotalDamageInflicted / _enemyUnitSize) * 100,
        };

        // Debug.Log($"My damage inflicted: {myTotalDamageInflicted}, My damage taken: {myTotalDamageTaken}");

        // calculate combat result
        return result;
    }

    struct CombatResult
    {
        // models that suffered wounds
        public int WoundsSuffered;
        public int WoundsCaused;

        // relevant for combat result
        public int DamageSuffered;
        public int DamageCaused;
        public int result; // positive for win, negative for loss, 0 for draw

        // damage dealt in % of enemy unit total wounds
        public float MyDamageSufferedInPercent;
        public float MyDamageCausedInPercent;

        // models that died TODO, once multiple wounds are implemented
        // public int _myCasualties;
        // public int _enemyCasualties;
    }

    private (int woundsCaused, int damageInflicted) CalculateCombatResult(StatBlock[] attackerUnit,
        StatBlock[] defenderUnit, int currentInitiative, int attackerFightingRankSize)
    {
        int damageInflicted = 0;
        int woundsCaused = 0;
        StatBlock defenderStatBlock = new StatBlock()
        {
            // todo Extend for multiple stat blocks
            WS = defenderUnit[0].WS,
            T = defenderUnit[0].T,
            Armor = defenderUnit[0].Armor,
            Ward = defenderUnit[0].Ward,
            Regen = defenderUnit[0].Regen,
        };

        foreach (StatBlock attackerStatBlock in attackerUnit.Where(block => block.I == currentInitiative))
        {
            // to hit
            // weapon skill vs weapon skill -> chance, roll that times A * currentFightingRankSize
            int numberOfAttacks =
                attackerStatBlock.A *
                attackerFightingRankSize; //TODO adapt for different A values with different fighting rank conditions

            // Debug.Log($"Attacker attacks: {numberOfAttacks}, fighting rank size: {attackerFightingRankSize}");
            // for number of attacks times:
            int toHit = _toHitMatrix[attackerStatBlock.WS][defenderStatBlock.WS];
            int successfulHits = CalculateSuccessfulRolls(numberOfAttacks, toHit);
            // Debug.Log($"Successful hits: {successfulHits}");

            // to wound
            int toWound = _toWoundMatrix[attackerStatBlock.S][defenderStatBlock.T];
            (int successfulWounds, int criticalToWound) = CalculateSuccessfulRollsWithCrit(successfulHits, toWound);
            // Debug.Log($"Successful wounds: {successfulWounds}, Critical wounds: {criticalToWound}");

            // armor save
            int armorSave = defenderStatBlock.Armor;
            int apArmor = armorSave + attackerStatBlock.AP;
            int baneArmor = apArmor + attackerStatBlock.Bane;

            int successfulAPSaves = CalculateSuccessfulRolls((successfulWounds - criticalToWound), apArmor);
            int successfulBaneSaves = CalculateSuccessfulRolls(criticalToWound, baneArmor);
            int totalSuccessfulSaves = successfulAPSaves + successfulBaneSaves;
            int failedArmorSaves = successfulWounds - totalSuccessfulSaves;
            // Debug.Log($"Failed armor saves: {failedArmorSaves}");

            // ward save
            int wardSave = defenderStatBlock.Ward;
            // Debug.Log("Ward save: " + wardSave);
            int successfulWardSaves = CalculateSuccessfulRolls(failedArmorSaves, wardSave);
            int failedWardSaves = failedArmorSaves - successfulWardSaves;
            // Debug.Log($"Failed ward saves: {failedWardSaves}");

            // regen save
            int regenSave = defenderStatBlock.Regen;
            int successfulRegenSaves = CalculateSuccessfulRolls(failedWardSaves, regenSave);
            int failedRegenSaves = failedWardSaves - successfulRegenSaves;
            // Debug.Log($"Failed regen saves: {failedRegenSaves}");

            woundsCaused += failedWardSaves; // for combat result, some might have been regenerated
            damageInflicted += failedRegenSaves; //actual damage inflicted
        } // end loop

        return (woundsCaused, damageInflicted);
    }

    private (int successes, int crits) CalculateSuccessfulRollsWithCrit(int numberOfRolls, int difficulty)
    {
        if (difficulty > 6)
        {
            return (0, 0);
        }

        int successfulHits = 0;
        int crits = 0;
        for (int i = 0; i < numberOfRolls; i++) // TODO modifiers and rerolls
        {
            int roll = UnityEngine.Random.Range(1, 7);

            // Debug.Log($"Rolled: {roll} was: {difficulty}+");
            if (roll >= difficulty)
            {
                //   Debug.Log("Success!");
                successfulHits++;
                if (roll == 6)
                {
                    crits++;
                }

               
            }
            // else
            // {
            //     Debug.Log("Fail!");
            // }
        }

        // Debug.Log("Rolled: " + numberOfRolls + " times, successful hits: " + successfulHits + ", crits: " + crits);
        return (successfulHits, crits);
    }

    private int CalculateSuccessfulRolls(int numberOfRolls, int difficulty)
    {
        (int successes, int _) = CalculateSuccessfulRollsWithCrit(numberOfRolls, difficulty);
        return successes;
    }


    private int[][] _toHitMatrix = new[]
    {
        //     0  1  2  3  4  5  6  7  8  9  10
        new[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //0
        new[] {0, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5}, //1
        new[] {0, 3, 4, 4, 4, 5, 5, 5, 5, 5, 5}, //2
        new[] {0, 2, 3, 4, 4, 4, 4, 5, 5, 5, 5}, //3
        new[] {0, 2, 3, 3, 4, 4, 4, 4, 4, 5, 5}, //4
        new[] {0, 2, 2, 3, 3, 4, 4, 4, 4, 4, 4}, //5
        new[] {0, 2, 2, 3, 3, 3, 4, 4, 4, 4, 4}, //6
        new[] {0, 2, 2, 2, 3, 3, 3, 4, 4, 4, 4}, //7
        new[] {0, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4}, //8
        new[] {0, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4}, //9
        new[] {0, 2, 2, 2, 2, 3, 3, 3, 3, 3, 4}, //10
    };

    private int[][] _toWoundMatrix = new[]
    {
        //     0  1  2  3  4  5  6  7  8  9  10
        new[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //0
        new[]
        {
            0, 4, 5, 6, 6, 6, 6, 7, 7, 7, 7
        }, //1 // 7+ means impossible to wound, TODO make a check for certain attacks (if the can wound)
        new[] {0, 3, 4, 5, 6, 6, 6, 6, 7, 7, 7}, //2
        new[] {0, 2, 3, 4, 5, 6, 6, 6, 6, 7, 7}, //3
        new[] {0, 2, 2, 3, 4, 5, 6, 6, 6, 6, 7}, //4
        new[] {0, 2, 2, 2, 3, 4, 5, 6, 6, 6, 6}, //5
        new[] {0, 2, 2, 2, 2, 3, 4, 5, 6, 6, 6}, //6
        new[] {0, 2, 2, 2, 2, 2, 3, 4, 5, 6, 6}, //7
        new[] {0, 2, 2, 2, 2, 2, 2, 3, 4, 5, 6}, //8
        new[] {0, 2, 2, 2, 2, 2, 2, 2, 3, 4, 5}, //9
        new[] {0, 2, 2, 2, 2, 2, 2, 2, 2, 3, 4}, //10
    };


    private void SetupMockStats()
    {
        _myUnitSize = 20;
        _enemyUnitSize = 30;
        _myFightingRankSize = 10;
        _enemyFightingRankSize = 10;

        _myUnitStats = new StatBlock[1];
        _myUnitStats[0].M = 3;
        _myUnitStats[0].WS = 4;
        _myUnitStats[0].BS = 3;
        _myUnitStats[0].S = 5;
        _myUnitStats[0].T = 4;
        _myUnitStats[0].W = 1;
        _myUnitStats[0].I = 1;
        _myUnitStats[0].A = 1;
        _myUnitStats[0].Ld = 7;
        _myUnitStats[0].Armor = 5;
        _myUnitStats[0].Regen = 7;
        _myUnitStats[0].Ward = 7;
        _myUnitStats[0].AP = 2;
        _myUnitStats[0].Bane = 1;

        _enemyUnitStats = new StatBlock[1];

        _enemyUnitStats[0].M = 4;
        _enemyUnitStats[0].WS = 3;
        _enemyUnitStats[0].BS = 3;
        _enemyUnitStats[0].S = 4;
        _enemyUnitStats[0].T = 3;
        _enemyUnitStats[0].W = 1;
        _enemyUnitStats[0].I = 3;
        _enemyUnitStats[0].A = 1;
        _enemyUnitStats[0].Ld = 7;
        _enemyUnitStats[0].Armor = 6;
        _enemyUnitStats[0].Regen = 7;
        _enemyUnitStats[0].Ward = 7;
        _enemyUnitStats[0].AP = 1;
        _enemyUnitStats[0].Bane = 1;
    }


    private void SingletonAwake()
    {
        // do awake stuff here
    }

// ====================================================================================================

// ensure that there is only one instance of this class
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SingletonAwake();
        }
        else
        {
            Destroy(this);
        }
    }
}


struct StatBlock
{
    public int M;
    public int WS;
    public int BS;
    public int S;
    public int T;
    public int W;
    public int I;
    public int A;

    public int Ld;

    // is the save number (4+ save, 5+ save, etc), 2 to 7, 7 is no save
    public int Armor;
    public int Regen;

    public int Ward;

    // is a positive value
    public int AP;

    // is a positive value
    public int Bane;

    // can be positive or negative
    public int InitiativeMod;
}