namespace TOW_Calc_Full.Scripts
{
    public class UnitDelta
    {
        public bool Standard;
        public bool Musician;
        public Unit GroundTruthUnit;
        public ModelDelta[] Models;

        public ModelDelta GetNextTarget()
        {
            // get the next Model. null if no normal model can be targeted
            throw new System.NotImplementedException();
        }
    }
}