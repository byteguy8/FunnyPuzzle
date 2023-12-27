using System.Collections.Generic;

public class IBlockUnits : BlockUnits
{
    public IBlockUnits(int id, int row, int column)
        : base(id, row, column, UnitsUtil.GetBlockColor(UnitsType.IBLOCK)) { }

    public override List<Unit> BuildBlockUnits(int row, int column, int rotation)
    {
        return UnitsUtil.GenerateUnits(UnitsType.IBLOCK, row, column, rotation);
    }
}