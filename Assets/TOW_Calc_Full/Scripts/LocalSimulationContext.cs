using System.Collections.Generic;

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
            // for initiative 10 to 0, call all models to perform their actions
            for (int initiative = 10; initiative >= 0; initiative--)
            {
                _currentInitiative = initiative;

                // for all the models in all the units, call their PerformAction method
                PerformActionsForSide(_battle.SideAUnits);
                PerformActionsForSide(_battle.SideBUnits);
            }

            return simulationResult;
        }

        private void PerformActionsForSide(UnitDelta[] side)
        {
            // Iterate through each unit in the side
            foreach (UnitDelta unit in side)
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