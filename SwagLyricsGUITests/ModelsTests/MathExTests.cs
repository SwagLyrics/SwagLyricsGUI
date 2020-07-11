using SwagLyricsGUI.Models;
using System;
using Xunit;

namespace SwagLyricsGUITests.Models
{
    public class MathExTests
    {
        [Theory]
        [InlineData(0,1)]
        [InlineData(1,2)]
        [InlineData(100,300)]
        public void TestThatLerpReturnsMidpoint(double start, double end)
        {
           double result = MathEx.Lerp(start, end, 0.5);
            double expected = (start + end) / 2;
            Assert.Equal(expected, result);
        }
    }
}
