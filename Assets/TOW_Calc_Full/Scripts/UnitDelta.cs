namespace TOW_Calc_Full.Scripts
{
    public class UnitDelta
    {
        public bool Standard;
        public bool Musician;
        public Unit GroundTruthUnit;
        public ModelDelta[] Models;


        // TODO check when the characters get targeted? Only in shooting right? 
        /// <summary>
        /// Returns the next valid target for when attacking the Unit models.
        /// This is the front rank, then the back rank.
        /// Finally the Champion.
        /// </summary>
        /// <returns>The next model that is to be targeted. null if there is no targetable model</returns>
        /// <exception cref="NotImplementedException"></exception>
        public ModelDelta GetNextUnitModel()
        {
            throw new System.NotImplementedException();
        }

        // remove all the dead models. and replace their links with one to the unit or sth
        public void Cleanup()
        {
            foreach (ModelDelta model in Models)
            {
                if (model.Dead)
                {
                    throw new System.NotImplementedException();
                    // TODO remove the model, but find all the references to it in DeltaBaseContactEnemies lists and replace them with a unit's next target or a placeholder that signifies "Unit"
                }
            }
        }
    }
}