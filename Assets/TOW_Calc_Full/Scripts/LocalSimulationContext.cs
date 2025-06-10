using System.Collections.Generic;

namespace TOW_Calc_Full.Scripts
{
    public class LocalSimulationContext
    {
        public Dictionary<Model, ModelDelta> ModelDeltaContext => _modelDeltaContext;

        private int _currentInitiative;
        private Dictionary<Model, ModelDelta> _modelDeltaContext = new Dictionary<Model, ModelDelta>();
        private Battle _battle;

        public LocalSimulationContext(Battle battle)
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

        private void PerformActionsForSide(Unit[] side)
        {
            // Iterate through each unit in the side
            foreach (Unit unit in side)
            {
                // Iterate through each model in the unit
                foreach (Model model in unit.Models)
                {
                    // Perform action for the model at the current initiative
                    model.Attack(_currentInitiative, this);
                }
            }
        }
    }
}