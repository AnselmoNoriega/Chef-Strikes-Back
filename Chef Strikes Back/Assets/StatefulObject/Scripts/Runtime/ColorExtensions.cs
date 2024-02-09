using UnityEngine;


/// <summary>
/// Extends the Unity Color class
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Returns the color with half the amount of alpha
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Color WithHalfAlpha(this Color color)
    {
        color.a *= 0.5f;
        return color;
    }

    /// <summary>
    /// Returns the color with a quarter amount of alpha
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Color WithQuarterAlpha(this Color color)
    {
        color.a *= 0.25f;
        return color;
    }
}
