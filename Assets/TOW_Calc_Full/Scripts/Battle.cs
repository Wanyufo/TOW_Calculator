namespace TOW_Calc_Full.Scripts
{
    public class Battle
    {
        public Battle(Unit[] sideAUnits, Unit[] sideBUnits)
        {
            SideAUnits = sideAUnits;
            SideBUnits = sideBUnits;
        }

        public Unit[] SideAUnits { get; }
        public Unit[] SideBUnits { get; }
    }
}