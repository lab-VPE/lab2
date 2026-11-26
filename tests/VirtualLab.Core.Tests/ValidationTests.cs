using VirtualLab.Core;
using Xunit;

namespace VirtualLab.Core.Tests;

public class ValidationTests
{
    private readonly TorricelliSolver _solver = new();

    [Fact]
    public void CorrectParameters_HaveNoErrors()
    {
        var errors = _solver.Validate(new TorricelliParameters(1000, 1.0, 0.1, 0.01, 0.1));

        Assert.Empty(errors);
    }

    [Theory]
    [InlineData(0, 1.0, 0.1, 0.01, 0.1, 9.8, ValidationCodes.InvalidDensity, "Density")]
    [InlineData(-5, 1.0, 0.1, 0.01, 0.1, 9.8, ValidationCodes.InvalidDensity, "Density")]
    [InlineData(1000, 0, 0.0, 0.01, 0.1, 9.8, ValidationCodes.InvalidInitialHeight, "InitialHeight")]
    [InlineData(1000, 1.0, -0.1, 0.01, 0.1, 9.8, ValidationCodes.InvalidHoleHeight, "HoleHeight")]
    [InlineData(1000, 1.0, 1.0, 0.01, 0.1, 9.8, ValidationCodes.HoleAboveLevel, "HoleHeight")]
    [InlineData(1000, 1.0, 1.5, 0.01, 0.1, 9.8, ValidationCodes.HoleAboveLevel, "HoleHeight")]
    [InlineData(1000, 1.0, 0.1, 0.0, 0.1, 9.8, ValidationCodes.InvalidHeightStep, "HeightStep")]
    [InlineData(1000, 1.0, 0.1, 0.01, 0.0, 9.8, ValidationCodes.InvalidTimeStep, "TimeStep")]
    [InlineData(1000, 1.0, 0.1, 0.01, 0.1, 0.0, ValidationCodes.InvalidGravity, "Gravity")]
    [InlineData(1000, 1.0, 0.1, 0.000001, 0.1, 9.8, ValidationCodes.TooManyPoints, "HeightStep")]
    public void IncorrectParameter_ReturnsExpectedError(
        double density, double initialHeight, double holeHeight,
        double heightStep, double timeStep, double gravity,
        string expectedCode, string expectedField)
    {
        var parameters = new TorricelliParameters(
            density, initialHeight, holeHeight, heightStep, timeStep, gravity);

        var errors = _solver.Validate(parameters);

        Assert.Contains(errors, e => e.Code == expectedCode && e.Field == expectedField);
    }

    [Fact]
    public void NotANumber_IsRejected()
    {
        var parameters = new TorricelliParameters(double.NaN, 1.0, 0.1, 0.01, 0.1);

        var errors = _solver.Validate(parameters);

        Assert.Contains(errors, e => e.Code == ValidationCodes.InvalidDensity);
    }

    [Fact]
    public void Infinity_IsRejected()
    {
        var parameters = new TorricelliParameters(1000, double.PositiveInfinity, 0.1, 0.01, 0.1);

        var errors = _solver.Validate(parameters);

        Assert.Contains(errors, e => e.Code == ValidationCodes.InvalidInitialHeight);
    }

    [Fact]
    public void SeveralProblems_AreReportedAtOnce()
    {
        var parameters = new TorricelliParameters(0, 1.0, 0.1, 0.0, 0.1);

        var errors = _solver.Validate(parameters);

        Assert.True(errors.Count >= 2);
        Assert.Contains(errors, e => e.Code == ValidationCodes.InvalidDensity);
        Assert.Contains(errors, e => e.Code == ValidationCodes.InvalidHeightStep);
    }

    [Fact]
    public void EachField_HasAtMostOneError()
    {
        var parameters = new TorricelliParameters(0, 0, 5.0, 0.0, 0.0, 0.0);

        var errors = _solver.Validate(parameters);

        Assert.Equal(errors.Count, errors.Select(e => e.Field).Distinct().Count());
    }
}
