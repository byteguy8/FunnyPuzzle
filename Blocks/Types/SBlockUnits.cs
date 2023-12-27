using System.Collections.Generic;

public class SBlockUnits : BlockUnits
{
    public SBlockUnits(int id, int row, int column)
        : base(id, row, column, UnitsUtil.GetBlockColor(UnitsType.SBLOCK)) { }

    public override List<Unit> BuildBlockUnits(int row, int column, int rotation)
    {
        return UnitsUtil.GenerateUnits(UnitsType.SBLOCK, row, column, rotation);
    }
}