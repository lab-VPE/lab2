namespace VirtualLab.Core;

/// <summary>Входные параметры расчёта вытекания жидкости.</summary>
/// <param name="Density">Плотность жидкости ρ, кг/м³.</param>
/// <param name="InitialHeight">Начальный уровень жидкости от дна, м.</param>
/// <param name="HoleHeight">Высота отверстия от дна, м.</param>
/// <param name="HeightStep">Шаг по высоте dh, м.</param>
/// <param name="TimeStep">Шаг по времени dt, с.</param>
/// <param name="Gravity">Ускорение свободного падения g, м/с².</param>
public sealed record TorricelliParameters(
    double Density,
    double InitialHeight,
    double HoleHeight,
    double HeightStep,
    double TimeStep,
    double Gravity = 9.8);
