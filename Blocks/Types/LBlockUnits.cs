using System.Collections.Generic;

public class LBlockUnits : BlockUnits
{
    public LBlockUnits(int id, int row, int column)
        : base(id, row, column, UnitsUtil.GetBlockColor(UnitsType.LBLOCK)) { }

    public override List<Unit> BuildBlockUnits(int row, int column, int rotation)
    {
        return UnitsUtil.GenerateUnits(UnitsType.LBLOCK, row, column, rotation);
    }
}