using System.Collections.Generic;

public class TBlockUnits : BlockUnits
{
    public TBlockUnits(int id, int row, int column)
        : base(id, row, column, UnitsUtil.GetBlockColor(UnitsType.TBLOCK)) { }

    public override List<Unit> BuildBlockUnits(int row, int column, int rotation)
    {
        return UnitsUtil.GenerateUnits(UnitsType.TBLOCK, row, column, rotation);
    }
}