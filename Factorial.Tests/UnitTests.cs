using System.Numerics;
using Xunit;

public class UnitTests
{
    [Fact]
    public void SmallFactorials()
    {
        Assert.Equal(BigInteger.One, Factorial.Calculate(0));
        Assert.Equal(BigInteger.One, Factorial.Calculate(1));
        Assert.Equal(new BigInteger(2), Factorial.Calculate(2));
        Assert.Equal(new BigInteger(6), Factorial.Calculate(3));
        Assert.Equal(new BigInteger(120), Factorial.Calculate(5));
    }

    [Fact]
    public void TrailingZeros()
    {
        Assert.Equal(2, Factorial.CountTrailingZeros(10));
        Assert.Equal(24, Factorial.CountTrailingZeros(100));
        Assert.Equal(249, Factorial.CountTrailingZeros(1000));
    }

    [Fact]
    public void DigitEstimate()
    {
        Assert.Equal(158, Factorial.CountDigitsFactorial(100));
        Assert.Equal(2568, Factorial.CountDigitsFactorial(1000));
    }
}
