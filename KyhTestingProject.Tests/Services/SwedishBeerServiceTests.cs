using KyhTestingProject.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KyhTestingProject.Tests.Services;

[TestClass]
public class SwedishBeerServiceTests
{
    private SwedishBeerService _sut; 
    public SwedishBeerServiceTests()
    {
        _sut = new SwedishBeerService();
    }

    [TestMethod]
    public void If_promille_is_more_than_one_returns_false()
    {
        // ARRANGE
        var promille = 1.1m;
        var location = Location.Systemet;
        var age = 50;

        // ACT
        var result = _sut.CanIBuyBeer(age, location, promille);

        // ASSERT
        Assert.IsFalse(result);
    }

}