using Microsoft.Extensions.Configuration;
using Moq;
using Tasky;
using Tasky.Services;

namespace TaskyTest;

public class HolidayInfoLoaderTest
{
    private HolidayInfoLoader _holiday;

    [SetUp]
    public void Setup()
    {
        var mockConfigure = new Mock<IConfiguration>();

        mockConfigure.Setup(c => c.GetSection("ConnectionStrings")["holidays"])
            .Returns("holidays.json");

        _holiday = new HolidayInfoLoader(mockConfigure.Object);
    }

    [TestCase("2024-01-01")]
    [TestCase("2024-03-01")]
    [TestCase("2024-12-25")]
    public void IsHoliday_Holiday_ReturnTrue(DateTime date)
    {
        Assert.That(_holiday.IsHoliday(date), Is.True);
    }

    [TestCase("2024-01-09")]
    [TestCase("2024-03-03")]
    [TestCase("2024-10-24")]
    public void IsHoliday_NotHoliday_ReturnFalse(DateTime date)
    {
        Assert.That(_holiday.IsHoliday(date), Is.False);
    }
}