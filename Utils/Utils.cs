using System;
using Godot;

public class Utils
{
    public static Rect2 CreateRect(Rect2 from, float wPercentage, float hPercentage)
    {
        if (from.Size.X <= 0)
        {
            throw new ArgumentException("Illegal width value");
        }

        if (from.Size.Y <= 0)
        {
            throw new ArgumentException("Illegal height value");
        }

        var w = from.Size.X * (wPercentage / 100f);
        var h = from.Size.Y * (hPercentage / 100f);
        var x = from.Position.X + (from.Size.X / 2f) - (w / 2f);
        var y = from.Position.Y + (from.Size.Y / 2f) - (h / 2f);

        return new Rect2(new Vector2(x, y), new Vector2(w, h));
    }

    public static Label CreateLabelAboveRect(Rect2 rect, string text)
    {
        var label = new Label();

        label.Text = text;
        label.Theme.Set("font_size", 32);
        label.Position = rect.Position;

        return label;
    }
}