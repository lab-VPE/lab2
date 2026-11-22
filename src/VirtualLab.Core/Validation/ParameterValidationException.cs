namespace VirtualLab.Core;

/// <summary>Параметры расчёта не прошли проверку.</summary>
public sealed class ParameterValidationException : Exception
{
    /// <summary>Все найденные ошибки.</summary>
    public IReadOnlyList<ValidationError> Errors { get; }

    public ParameterValidationException(IReadOnlyList<ValidationError> errors)
        : base("Параметры расчёта некорректны: " + string.Join("; ", errors.Select(e => e.Message)))
    {
        Errors = errors;
    }
}
