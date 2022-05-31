using System;
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

    [TestMethod]
    public void If_promille_less_than_one_and_age_18_and_krogen_return_true()
    {
        //ARRANGE
        var promille = 0.1m;
        var location = Location.Krogen;
        var age = 18;

        // ACT
        var result = _sut.CanIBuyBeer(age, location, promille);

        // ASSERT
        Assert.IsTrue(result);

    }

    [TestMethod]
    public void If_promille_less_than_one_and_age_17_and_krogen_return_false()
    {
        //ARRANGE
        var promille = 0.1m;
        var location = Location.Krogen;
        var age = 17;

        // ACT
        var result = _sut.CanIBuyBeer(age, location, promille);

        // ASSERT
        Assert.IsFalse(result);
    }




    [TestMethod]
    public void If_promille_less_than_one_and_age_20_and_systemet_return_true()
    {
        //ARRANGE
        var promille = 0.1m;
        var location = Location.Systemet;
        var age = 20;

        // ACT
        var result = _sut.CanIBuyBeer(age, location, promille);

        // ASSERT
        Assert.IsTrue(result);
    }



    [TestMethod]
    public void If_promille_less_than_one_and_age_19_and_systemet_return_false()
    {
        //ARRANGE
        var promille = 0.1m;
        var location = Location.Systemet;
        var age = 19;

        // ACT
        var result = _sut.CanIBuyBeer(age, location, promille);

        // ASSERT
        Assert.IsFalse(result);
    }




}