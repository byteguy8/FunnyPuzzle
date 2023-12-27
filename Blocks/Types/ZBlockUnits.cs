using System.Collections.Generic;

public class ZBlockUnits : BlockUnits
{
    public ZBlockUnits(int id, int row, int column)
        : base(id, row, column, UnitsUtil.GetBlockColor(UnitsType.TBLOCK)) { }

    public override List<Unit> BuildBlockUnits(int row, int column, int rotation)
    {
        return UnitsUtil.GenerateUnits(UnitsType.ZBLOCK, row, column, rotation);
    }
}