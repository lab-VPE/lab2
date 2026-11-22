namespace VirtualLab.Core;

/// <summary>Одна точка результата расчёта.</summary>
/// <param name="Time">Время, с.</param>
/// <param name="Height">Уровень жидкости от дна, м.</param>
/// <param name="Velocity">Скорость истечения, м/с.</param>
public sealed record SimulationPoint(
    double Time,
    double Height,
    double Velocity);
