namespace VirtualLab.Core;

/// <summary>Ошибка проверки входных параметров.</summary>
/// <param name="Code">Машинный код ошибки (см. ValidationCodes).</param>
/// <param name="Field">Имя свойства TorricelliParameters, к которому относится ошибка.</param>
/// <param name="Message">Понятное человеку объяснение на русском.</param>
public sealed record ValidationError(
    string Code,
    string Field,
    string Message);
