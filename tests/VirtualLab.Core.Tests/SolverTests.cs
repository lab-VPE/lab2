using System.Globalization;
using VirtualLab.Core;
using Xunit;

namespace VirtualLab.Core.Tests;

public class SolverTests
{
    // Пример из ТЗ: 91 точка
    private static readonly TorricelliParameters Example = new(1000, 1.0, 0.1, 0.01, 0.1);

    private readonly TorricelliSolver _solver = new();

    [Fact]
    public void Example_Returns91Points()
    {
        var points = _solver.Solve(Example);

        Assert.Equal(91, points.Count);
    }

    [Fact]
    public void Example_FirstPoint_IsInitialState()
    {
        var first = _solver.Solve(Example)[0];

        Assert.Equal(0.0, first.Time, 9);
        Assert.Equal(1.0, first.Height, 9);
        Assert.Equal(4.2, first.Velocity, 9);
    }

    [Fact]
    public void Example_SecondPoint_HasExpectedVelocity()
    {
        var second = _solver.Solve(Example)[1];

        Assert.Equal(0.1, second.Time, 9);
        Assert.Equal(0.99, second.Height, 9);
        Assert.Equal(4.1766, second.Velocity, 4);
    }

    [Fact]
    public void Example_LastPoint_IsAtHoleWithZeroVelocity()
    {
        var last = _solver.Solve(Example)[^1];

        Assert.Equal(9.0, last.Time, 9);
        Assert.Equal(0.1, last.Height, 9);
        Assert.Equal(0.0, last.Velocity, 6);
    }

    [Fact]
    public void OneMeterHead_GivesTorricelliVelocity()
    {
        // h = 1 м, g = 9,8 → √19,6 ≈ 4,4272 м/с
        var parameters = new TorricelliParameters(1000, 1.0, 0.0, 1.0, 1.0);

        var first = _solver.Solve(parameters)[0];

        Assert.Equal(4.4272, first.Velocity, 4);
    }

    [Fact]
    public void Velocity_NeverIncreases_AsLevelDrops()
    {
        var points = _solver.Solve(Example);

        for (int i = 1; i < points.Count; i++)
            Assert.True(points[i].Velocity <= points[i - 1].Velocity);
    }

    [Fact]
    public void Density_DoesNotAffectVelocity()
    {
        var heavy = _solver.Solve(Example with { Density = 1000 });
        var light = _solver.Solve(Example with { Density = 800 });

        Assert.Equal(heavy.Count, light.Count);
        for (int i = 0; i < heavy.Count; i++)
            Assert.Equal(heavy[i].Velocity, light[i].Velocity, 9);
    }

    [Fact]
    public void RangeNotMultipleOfStep_StopsAboveHole()
    {
        // Уровень 1,0 → отверстие 0, шаг 0,3: высоты 1,0 / 0,7 / 0,4 / 0,1
        var parameters = new TorricelliParameters(1000, 1.0, 0.0, 0.3, 0.1);

        var points = _solver.Solve(parameters);

        Assert.Equal(4, points.Count);
        Assert.All(points, p =>
        {
            Assert.True(p.Height >= 0.0);
            Assert.False(double.IsNaN(p.Velocity));
        });
    }

    [Fact]
    public void Solve_WithInvalidParameters_Throws()
    {
        var bad = Example with { Density = 0 };

        var ex = Assert.Throws<ParameterValidationException>(() => _solver.Solve(bad));

        Assert.Contains(ex.Errors, e => e.Code == ValidationCodes.InvalidDensity);
    }

    [Fact]
    public void Result_DoesNotDependOnCulture()
    {
        var invariant = _solver.Solve(Example);

        var previous = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("ru-RU");
            var russian = _solver.Solve(Example);

            Assert.Equal(invariant.Count, russian.Count);
            for (int i = 0; i < invariant.Count; i++)
                Assert.Equal(invariant[i], russian[i]);
        }
        finally
        {
            CultureInfo.CurrentCulture = previous;
        }
    }
}
