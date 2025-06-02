namespace TOW_Calc_Full.Scripts
{
    public class ModelDelta
    {
        private readonly Model _groundTruthModel;
        private readonly LocalSimulationContext _localSimulationContext;
        
        public ModelDelta(Model groundTruthModel, LocalSimulationContext localSimulationContext)
        {
            _groundTruthModel = groundTruthModel;
            _localSimulationContext = localSimulationContext;
        }
        
    }
}