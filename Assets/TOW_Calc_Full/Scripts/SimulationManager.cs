namespace TOW_Calc_Full.Scripts
{
    public class SimulationManager
    {
        private int _simulationCount;
        private int _desiredParallelisation;
        private Battle _battle;
        
        
       
        public SimulationManager(Battle battle,int simulationCount = 10000 , int desiredParallelisation = 1)
        {
            // sanity check
            if (simulationCount <= 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(simulationCount), "Simulation count must be greater than zero");
            }
            if (desiredParallelisation < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(desiredParallelisation), "Parallel count must not be negative. Use 0 for as many as can be done.");
            }
            _battle = battle ?? throw new System.ArgumentNullException(nameof(battle), "Battle cannot be null");
            _simulationCount = simulationCount;
            _desiredParallelisation = desiredParallelisation; // Default to 1 thread if not specified
        }
        
        // run x simulations in y parallel threads
        public SimulationResult RunSimulation()
        {
         SimulationResult aggregatedResult = new SimulationResult(); // can be a local field for the method 

         //  Run _simulationCount simulations and aggregate results on _desiredParallelisation threads
            for (int i = 0; i < _simulationCount; i++)
            {
                // Create a new LocalSimulationContext for each simulation
                LocalSimulationContext localSimulationContext = new LocalSimulationContext(_battle);
                
                // Run the simulation and get the result
                SimulationResult result = localSimulationContext.RunSimulation();
                
                // Aggregate the result into the aggregatedResult
                aggregatedResult.Aggregate(result);
            }
         return aggregatedResult;
        }
    }
}