using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KyhTestingProject.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace KyhTestingProject.Tests.Services;

//public class FakeUserRepository : IUserRepository
//{
//    public List<string> Existing { get; set; } = new List<string>();

//    public bool Exists(string email)
//    {
//        return Existing.Contains(email);
//    }

//    public int GetRegistreredCountToday()
//    {
//        return Existing.Count;
//    }
//}

//public class FakeEmailService : IEmailService
//{
//    public bool HasBeenCalled = false;
//    public void SendWelcomeEmail(string email)
//    {
//        HasBeenCalled = true;
//    }
//}

[TestClass]
public class RegistrationServiceTests
{
    private RegistrationService _sut;
    private Mock<IUserRepository> _repositoryMock;
    private Mock<IEmailService> _emailServiceMock;


    public RegistrationServiceTests()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _emailServiceMock = new Mock<IEmailService>();

         _sut = new RegistrationService(_repositoryMock.Object,  _emailServiceMock.Object );
    }

    [TestMethod]
    public void Should_not_allow_registration_if_already_registered()
    {
        //ARRANGE
        var email = "stefan@hej.se";
        _repositoryMock.Setup(r => r.Exists(email)).Returns(true);

        //ACT
        var result = _sut.Register(email);

        //ASSERT
        Assert.AreEqual(RegistrationStatus.AlreadyRegistered, result);
    }






    [TestMethod]
    public void Should_not_allow_registration_if_more_than_10_today()
    {
        //ARRANGE
        _repositoryMock.Setup(r => r.GetRegistreredCountToday()).Returns(11);

        //ACT
        var result = _sut.Register("sss@hej.se");

        //ASSERT
        Assert.AreEqual(RegistrationStatus.TooManyRegistrationsToday, result);
    }



    [TestMethod]
    public void When_email_is_not_correct_domain_should_fail()
    {
        //ARRANGE

        //ACT
        var result = _sut.Register("blabla@aaa.se");

        //ASSERT
        Assert.AreEqual(RegistrationStatus.WrongDomain, result);
    }


    [TestMethod]   
    public void When_registration_is_ok_we_get_ok_result()
    {
        //ARRANGE

        //ACT
        var result = _sut.Register("blabla@hej.se");

        //ASSERT
        Assert.AreEqual(RegistrationStatus.Ok, result);
    }



    [TestMethod]   // when ok så skall vi delegera till emailservicen
    public void When_registration_is_ok_an_email_should_be_sent()
    {
        //ARRANGE

        //ACT
        var result = _sut.Register("blabla@hej.se");

        //ASSERT
        _emailServiceMock.Verify(e => e.SendWelcomeEmail(It.IsAny<string>()), Times.Once());
    }



}