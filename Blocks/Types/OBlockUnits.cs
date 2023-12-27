using System.Collections.Generic;

public class OBlockUnits : BlockUnits
{
    public OBlockUnits(int id, int row, int column)
        : base(id, row, column, UnitsUtil.GetBlockColor(UnitsType.OBLOCK)) { }

    public override List<Unit> BuildBlockUnits(int row, int column, int rotation)
    {
        return UnitsUtil.GenerateUnits(UnitsType.OBLOCK, row, column, rotation);
    }
}