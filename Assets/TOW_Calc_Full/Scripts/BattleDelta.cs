namespace TOW_Calc_Full.Scripts
{
    public class BattleDelta
    {
        public BattleDelta(UnitDelta[] sideAUnits, UnitDelta[] sideBUnits)
        {
            SideAUnits = sideAUnits;
            SideBUnits = sideBUnits;
        }

        public UnitDelta[] SideAUnits { get; }
        public UnitDelta[] SideBUnits { get; }
    }
}