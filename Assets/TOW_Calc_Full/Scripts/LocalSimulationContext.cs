using System;

namespace TOW_Calc_Full.Scripts
{
    public class LocalSimulationContext
    {
        private int _currentInitiative;
        private BattleDelta _battle;

        public LocalSimulationContext(BattleDelta battle)
        {
            _battle = battle;
            _currentInitiative = 0;
        }

        public SimulationResult RunSimulation()
        {
            SimulationResult simulationResult = new SimulationResult();

            UnitDelta[] allUnits = new UnitDelta[_battle.SideAUnits.Length + _battle.SideBUnits.Length];
            Array.Copy(_battle.SideAUnits, 0, allUnits, 0, _battle.SideAUnits.Length);
            Array.Copy(_battle.SideBUnits, 0, allUnits, _battle.SideAUnits.Length, _battle.SideBUnits.Length);
            // for initiative 10 to 0, call all models to perform their actions
            for (int initiative = 10; initiative >= 0; initiative--)
            {
                _currentInitiative = initiative;

                // for all the models in all the units, call their PerformAction method
                PerformActions(allUnits);
                CleanupDeadModels(allUnits);
            }

            return simulationResult;
        }

        private void CleanupDeadModels(UnitDelta[] allUnits)
        {
            foreach (UnitDelta unit in allUnits)
            {
                unit.Cleanup();
            }
            
            throw new System.NotImplementedException();
        }

        private void PerformActions(UnitDelta[] allUnits)
        {
            // Iterate through each unit in the side
            foreach (UnitDelta unit in allUnits)
            {
                // Iterate through each model in the unit
                foreach (ModelDelta model in unit.Models)
                {
                    // Perform action for the model at the current initiative
                    model.Attack(_currentInitiative);
                }
            }
        }
    }
}