namespace VirtualLab.Core;

/// <summary>Расчёт вытекания жидкости из сосуда (закон Торричелли).</summary>
public interface ITorricelliSolver
{
    /// <summary>Проверяет параметры и возвращает ВСЕ найденные ошибки (пустой список — всё хорошо).</summary>
    IReadOnlyList<ValidationError> Validate(TorricelliParameters parameters);

    /// <summary>Считает точки результата. Бросает ParameterValidationException, если параметры некорректны.</summary>
    IReadOnlyList<SimulationPoint> Solve(TorricelliParameters parameters);
}
