using System;
using System.Collections.Generic;
using System.Linq;

namespace TOW_Calc_Full.Scripts
{
    public class ModelDelta
    {
        public bool Killed { get; set; } = false;
        public ModelDelta[] DeltaBaseContactEnemies;
        public ModelModifiers[] Modifiers { get; set; } = Array.Empty<ModelModifiers>();
        public readonly Model GroundTruthModel;
        public readonly LocalSimulationContext Context;
        public readonly UnitDelta ParentUnitDelta;


        public ModelDelta(Model groundTruthModel, LocalSimulationContext context, ModelDelta[] deltaBaseContactEnemies,
            UnitDelta parentUnitDelta)
        {
            GroundTruthModel = groundTruthModel;
            Context = context;
            DeltaBaseContactEnemies = deltaBaseContactEnemies;
            ParentUnitDelta = parentUnitDelta;
        }


        public void Attack(int initiative)
        {
            foreach (Attack attack in GroundTruthModel.Attacks)
            {
                if (attack.Initiative == initiative)
                {
                    ModelDelta target = DeltaBaseContactEnemies
                        .OrderBy(modelDelta => (int) modelDelta.GroundTruthModel.ModelType).FirstOrDefault(); // sorts using the position of the Entry in the Enum // TODO replace with priority

                    if (target != null)
                    {
                        if (target.GroundTruthModel.ModelType == ModelType.Unit)
                        {
                            target = target.ParentUnitDelta.GetNextTarget();
                        }
                        
                        // Target is now either the champion or character, or otherwise it's the unit's "next target"
                    }
                    else
                    {
                        // TODO Handle this, no base contact. might be able to support attack
                    }
                    
                    // TODO Make the attack now


                    // ModelDelta target = null;
                    // // is any of the base contact enenmies valid?
                    // ModelDelta[] validTargets;
                    // foreach (ModelDelta deltaBaseContactEnemy in DeltaBaseContactEnemies)
                    // {
                    //     if (deltaBaseContactEnemy.GroundTruthModel.Type != ModelType.Character)
                    //     {
                    //         deltaBaseContactEnemy.ParentUnitDelta.GetNextTarget()
                    //     }
                    //     // ignore dead targets
                    //     if (deltaBaseContactEnemy.Killed)
                    //     {
                    //         continue;
                    //     }
                    //
                    //     // for now we dont use Strategies, so we always attack non-Characters, then units and only then characters
                    //     if (deltaBaseContactEnemy.GroundTruthModel.Type == ModelType.Character && target != null) // skip characters if we have found another target
                    //         continue;
                    //     target = deltaBaseContactEnemy;
                    // }
                    // // target is null if no target has been found, a Character if only a character can be attacked, or a normal Model 


                    // // if not, can we attack a Unit? If so, ask unit for a target ModelDelta
                    //
                    //
                    // // if there are any models in base contact and alive, attack these, otherwise fall back to attacking unit instead
                    // // TODO Check for priority targets and priority units
                    // // -> add checks to see if there are priority targets in the base contact enemies, and if one unit is preferred for attacking, or if a unit is already killed to a point where no attacks can be made
                    // if (ContactEnemies != null && _baseContactEnemies.Length > 0)
                    // {
                    //     // collect valid targets:
                    //     foreach (Model baseContactEnemy in _baseContactEnemies)
                    //     {
                    //     }

                    //     foreach (Model enemy in
                    //              _baseContactEnemies) // replace all basecontact enemies with only the ones that are a target regarding to the strategy. Or the unit
                    //     {
                    //         // check to see if there is a delta for the enemy in context.ModelDeltaContext,
                    //         
                    //         
                    //                 out ModelDelta enemyDelta)) // if we find it, it might be dead
                    //         {
                    //             if (enemyDelta.Killed)
                    //             {
                    //                 continue; // ignore killed targets
                    //             }
                    //             else
                    //             {
                    //                 // do the attack
                    //             }
                    //         }
                    //         else // the model is not dead for sure
                    //         {
                    //         }
                    //
                    //         // Perform attack logic against each enemy
                    //         // This is a placeholder for the actual attack logic
                    //         Console.WriteLine($"Attacking {enemy} at initiative {initiative}");
                    //     }
                    // }
                    // else
                    // {
                    //     // Fall back to attacking the unit instead
                    //     Console.WriteLine($"No base contact enemies, attacking unit at initiative {initiative}");
                    //    }


                    throw new NotImplementedException("Attack logic is not implemented.");
                }
            }
        }
    }
}