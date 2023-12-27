using System.Collections.Generic;

public class UnitsBuilder
{
    private Unit Last;
    private Unit Leader;
    private List<Unit> Units;

    public UnitsBuilder(int row, int column)
    {
        Leader = new Unit(row, column);
        Last = Leader;
        Units = new List<Unit> { Leader };
    }

    public Unit LastUnit()
    {
        return Last;
    }

    public UnitsBuilder ToLeader()
    {
        Last = Leader;
        return this;
    }

    public UnitsBuilder AddTopFrom(Unit unit)
    {
        int row = unit.Row - 1;

        Last = new Unit(row, unit.Column);
        Units.Add(Last);
        return this;
    }

    public UnitsBuilder AddDownFrom(Unit unit)
    {
        int row = unit.Row + 1;

        Last = new Unit(row, unit.Column);
        Units.Add(Last);
        return this;
    }

    public UnitsBuilder AddLeftFrom(Unit unit)
    {
        var column = unit.Column - 1;

        Last = new Unit(unit.Row, column);
        Units.Add(Last);
        return this;
    }

    public UnitsBuilder AddRightFrom(Unit unit)
    {
        var column = unit.Column + 1;

        Last = new Unit(unit.Row, column);
        Units.Add(Last);
        return this;
    }

    public UnitsBuilder AddTop()
    {
        int row = Last.Row - 1;

        Last = new Unit(row, Last.Column);
        Units.Add(Last);
        return this;
    }

    public UnitsBuilder AddBottom()
    {
        int row = Last.Row + 1;

        Last = new Unit(row, Last.Column);
        Units.Add(Last);
        return this;
    }

    public UnitsBuilder AddLeft()
    {
        var column = Last.Column - 1;

        Last = new Unit(Last.Row, column);
        Units.Add(Last);
        return this;
    }

    public UnitsBuilder AddRight()
    {
        var column = Last.Column + 1;

        Last = new Unit(Last.Row, column);
        Units.Add(Last);
        return this;
    }

    public List<Unit> Build()
    {
        return Units;
    }
}