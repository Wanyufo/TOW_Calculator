using System;
using System.Collections.Generic;
using System.Linq;

namespace TOW_Calc_Full.Scripts
{
    public class ModelDelta
    {
        public bool Dead { get; set; } = false;
        public int RemainingWounds { get; set; }
        public ModelDelta[] DeltaBaseContactEnemies;
        public ModelModifiers[] Modifiers { get; set; } = Array.Empty<ModelModifiers>();
        public readonly Model GroundTruthModel;
        public readonly LocalSimulationContext Context;
        public readonly UnitDelta ParentUnitDelta;

        public ModelDelta(Model groundTruthModel, LocalSimulationContext context, ModelDelta[] deltaBaseContactEnemies,
            UnitDelta parentUnitDelta)
        {
            GroundTruthModel = groundTruthModel;
            RemainingWounds = GroundTruthModel.StatBlocks[0].W; // Currently this uses the first stat block W as wounds
            Context = context;
            DeltaBaseContactEnemies = deltaBaseContactEnemies;
            ParentUnitDelta = parentUnitDelta;
        }


        // why is this saying it can be made private? Can a different instance of an object call a private method?
        public void TakeDamage(int amount)
        {
            RemainingWounds -= amount;
            if (RemainingWounds == 0)
            {
                Dead = true;
            }
        }

        // TODO add supporting attacks
        public void Attack(int initiative)
        {
            foreach (Attack attack in GroundTruthModel.Attacks)
            {
                if (attack.Initiative != initiative) continue;
                List<ModelDelta> orderedTargets = DeltaBaseContactEnemies
                    .OrderBy(modelDelta => (int) modelDelta.GroundTruthModel.ModelType)
                    .ToList(); // sorts by priority using the position of the Entry in the Enum
                ModelDelta target = null;

                for (int targetIndex = 0; targetIndex < orderedTargets.Count; targetIndex++)
                {
                    target = orderedTargets[targetIndex];

                    if (!target.Dead)
                    {
                        break; // we found a valid target!
                    }

                    // Model is dead, if unit model, we can also just target a general unit model, but only if there is no unit models alive in basecontact
                    if (target.GroundTruthModel.ModelType == ModelType.Unit)
                    {
                        ModelDelta previousTarget = target;
                        target = orderedTargets.FirstOrDefault(modelDelta =>
                            modelDelta.GroundTruthModel.ModelType == ModelType.Unit && !modelDelta.Dead);
                        if (target == null) // there is no unit model alive in base contact
                        {
                            target = previousTarget.ParentUnitDelta.GetNextUnitModel();
                        }

                        if (target != null) // either we found some models in base contact or we got one from the Unit
                        {
                            break;
                        }
                    }

                    target = null;
                }

                if (target == null)
                {
                    break; // if we cant find a target for one attack, we dont need to check any of the other attacks from this model anymore
                }

                // to hit
                int toHit = Matrices.ToHitMatrix[this.GroundTruthModel.StatBlocks[attack.StatblockIndex].WS,
                    target.GroundTruthModel.StatBlocks[0].WS];
                if (!RollVsDifficulty(toHit).success) continue; // failed toHit, next attack

                // to wound
                int attackStrength = attack.Weapon.Strength;
                if (attack.Weapon.StrengthIsAModifier)
                {
                    attackStrength += this.GroundTruthModel.StatBlocks[attack.StatblockIndex].S;
                }

                int toWound = Matrices.ToWoundMatrix[attackStrength, target.GroundTruthModel.StatBlocks[0].T];
                (bool toWoundSuccess, bool toWoundCrit) = RollVsDifficulty(toWound);
                if (!toWoundSuccess) continue; // failed to wound, next attack

                // armor
                int armorSave = target.GroundTruthModel.Armor + attack.Weapon.AP;
                if (toWoundCrit) armorSave += attack.Weapon.ArmorBane;
                if (RollVsDifficulty(armorSave).success) continue; // the target succeeded the save, next attack

                // ward
                int wardSave = target.GroundTruthModel.Ward;
                if (RollVsDifficulty(wardSave).success) continue; // target succeeded the save, next attack

                // regen
                int regenSave = target.GroundTruthModel.Regen;
                if (RollVsDifficulty(regenSave).success)
                {
                    // TODO notify that a wound was saved by a regen. combat result relevant
                    continue; // target succeeded the save, next attack
                }
                // if we reached this point, the enemy suffers a wound.

                target.TakeDamage(1); // update for multiple wounds etc


                throw new NotImplementedException("Attack logic is not implemented.");
            }
        }

        private static (bool success, bool crit) RollVsDifficulty(int difficulty)
        {
            int roll = ThreadSafeRandom.RollD6();
            // success?
            if (roll >= difficulty)
            {
                // crit?
                if (roll == 6)
                {
                    return (true, true);
                }

                return (true, false);
            }

            return (false, false);
        }
    }
}