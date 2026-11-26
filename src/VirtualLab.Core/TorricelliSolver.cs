namespace VirtualLab.Core;

/// <summary>Расчёт вытекания жидкости по закону Торричелли (как в методичке).</summary>
public sealed class TorricelliSolver : ITorricelliSolver
{
    /// <summary>Максимальное число точек результата.</summary>
    public const int MaxPoints = 100_000;

    // Запас на ошибки округления при делении
    private const double Tolerance = 1e-9;

    public IReadOnlyList<ValidationError> Validate(TorricelliParameters parameters)
        => ParametersValidator.Validate(parameters);

    public IReadOnlyList<SimulationPoint> Solve(TorricelliParameters parameters)
    {
        var errors = Validate(parameters);
        if (errors.Count > 0)
            throw new ParameterValidationException(errors);

        int steps = CountSteps(parameters);
        var points = new List<SimulationPoint>(steps + 1);

        for (int i = 0; i <= steps; i++)
        {
            double height = parameters.InitialHeight - i * parameters.HeightStep;
            double time = i * parameters.TimeStep;

            // Высота столба над отверстием; max защищает от крошечного минуса из-за округления
            double head = Math.Max(0.0, height - parameters.HoleHeight);
            double velocity = Math.Sqrt(2.0 * parameters.Gravity * head);

            points.Add(new SimulationPoint(time, height, velocity));
        }

        return points;
    }

    /// <summary>Число шагов по высоте от начального уровня до отверстия.</summary>
    internal static int CountSteps(TorricelliParameters parameters)
    {
        double steps = Math.Floor(
            (parameters.InitialHeight - parameters.HoleHeight) / parameters.HeightStep + Tolerance);

        // Если шаг ничтожно мал, число шагов может не влезть в int
        return steps > int.MaxValue - 1 ? int.MaxValue - 1 : (int)steps;
    }
}
