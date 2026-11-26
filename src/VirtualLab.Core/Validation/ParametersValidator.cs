namespace VirtualLab.Core;

/// <summary>Проверка входных параметров расчёта.</summary>
public static class ParametersValidator
{
    public static IReadOnlyList<ValidationError> Validate(TorricelliParameters parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        var errors = new List<ValidationError>();

        if (!IsPositive(parameters.Density))
            errors.Add(new ValidationError(ValidationCodes.InvalidDensity,
                nameof(parameters.Density), "Плотность должна быть положительным числом"));

        bool levelOk = IsPositive(parameters.InitialHeight);
        if (!levelOk)
            errors.Add(new ValidationError(ValidationCodes.InvalidInitialHeight,
                nameof(parameters.InitialHeight), "Начальный уровень должен быть положительным числом"));

        bool holeOk = double.IsFinite(parameters.HoleHeight) && parameters.HoleHeight >= 0;
        if (!holeOk)
            errors.Add(new ValidationError(ValidationCodes.InvalidHoleHeight,
                nameof(parameters.HoleHeight), "Высота отверстия не может быть отрицательной"));

        bool holeBelowLevel = levelOk && holeOk && parameters.HoleHeight < parameters.InitialHeight;
        if (levelOk && holeOk && !holeBelowLevel)
            errors.Add(new ValidationError(ValidationCodes.HoleAboveLevel,
                nameof(parameters.HoleHeight), "Отверстие должно быть ниже начального уровня жидкости"));

        bool stepOk = IsPositive(parameters.HeightStep);
        if (!stepOk)
            errors.Add(new ValidationError(ValidationCodes.InvalidHeightStep,
                nameof(parameters.HeightStep), "Шаг по высоте должен быть положительным числом"));

        if (!IsPositive(parameters.TimeStep))
            errors.Add(new ValidationError(ValidationCodes.InvalidTimeStep,
                nameof(parameters.TimeStep), "Шаг по времени должен быть положительным числом"));

        if (!IsPositive(parameters.Gravity))
            errors.Add(new ValidationError(ValidationCodes.InvalidGravity,
                nameof(parameters.Gravity), "Ускорение свободного падения должно быть положительным числом"));

        // Слишком мелкий шаг даст слишком много точек
        if (holeBelowLevel && stepOk)
        {
            double count = TorricelliSolver.CountSteps(parameters) + 1.0;
            if (count > TorricelliSolver.MaxPoints)
                errors.Add(new ValidationError(ValidationCodes.TooManyPoints,
                    nameof(parameters.HeightStep),
                    $"Слишком много точек (максимум {TorricelliSolver.MaxPoints}). Увеличьте шаг по высоте"));
        }

        return errors;
    }

    // Число должно быть конечным (не NaN и не бесконечность) и больше нуля
    private static bool IsPositive(double value) => double.IsFinite(value) && value > 0;
}
