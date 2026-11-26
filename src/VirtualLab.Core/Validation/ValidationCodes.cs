namespace VirtualLab.Core;

/// <summary>Коды ошибок проверки параметров.</summary>
public static class ValidationCodes
{
    public const string InvalidDensity = "INVALID_DENSITY";
    public const string InvalidInitialHeight = "INVALID_INITIAL_HEIGHT";
    public const string InvalidHoleHeight = "INVALID_HOLE_HEIGHT";
    public const string HoleAboveLevel = "HOLE_ABOVE_LEVEL";
    public const string InvalidHeightStep = "INVALID_HEIGHT_STEP";
    public const string InvalidTimeStep = "INVALID_TIME_STEP";
    public const string InvalidGravity = "INVALID_GRAVITY";
    public const string TooManyPoints = "TOO_MANY_POINTS";
}
