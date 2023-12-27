using System.Collections.Generic;

public class JBlockUnits : BlockUnits
{
    public JBlockUnits(int id, int row, int column)
        : base(id, row, column, UnitsUtil.GetBlockColor(UnitsType.JBLOCK)) { }

    public override List<Unit> BuildBlockUnits(int row, int column, int rotation)
    {
        return UnitsUtil.GenerateUnits(UnitsType.JBLOCK, row, column, rotation);
    }
}