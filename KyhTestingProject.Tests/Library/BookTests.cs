using KyhTestingProject.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KyhTestingProject.Tests.Library;

[TestClass]
public class BookTests
{
    [TestMethod]
    public void When_borrowing_available_count_should_decrease_by_one()
    {
        //ARRANGE
        var sut = new Book("123231231", "JK...", "Harry Potter");
        sut.BuyNewEx(10);

        //ACT
        sut.Borrow("stefan");

        //ASSERT
        Assert.AreEqual(9, sut.Available);

    }

    [TestMethod]
    public void When_borrowing_twice_available_count_should_decrease_by_two()
    {
        //ARRANGE
        var sut = new Book("123231231", "JK...", "Harry Potter");
        sut.BuyNewEx(10);

        //ACT
        sut.Borrow("stefan");
        sut.Borrow("lisa");

        //ASSERT
        Assert.AreEqual(8, sut.Available);

    }

    [TestMethod]
    public void When_borrowing_the_borrower_is_added_to_the_list()
    {
        var sut = new Book("123231231", "JK...", "Harry Potter");
        sut.BuyNewEx(10);

        //ACT
        sut.Borrow("stefan");


        Assert.IsTrue(sut.IsBorrower("stefan"));



    }


    [TestMethod]
    public void Contructor_should_set_correct_values()
    {
        //ARRANGE + ACT
        var sut = new Book("231231", "Stefan", "Hockey idols");

        // ASSERT
        Assert.AreEqual("231231", sut.Isbn);
        Assert.AreEqual("Stefan", sut.Author);
        Assert.AreEqual("Hockey idols", sut.Title);
        Assert.AreEqual(0, sut.Count);
    }

    [TestMethod]
    public void Cant_borrow_book_when_no_available()
    {
        var sut = new Book("231231", "Stefan", "Hockey idols");

        var result = sut.Borrow("Stefan");

        Assert.IsFalse(result);
    }


    //[TestMethod]
    //public void Cant_borrow_book_when_already_borrowed()
    //{
    //    var sut = new Book("231231", "Stefan", "Hockey idols");
    //    sut.BuyNewEx(10);

    //    sut.Borrow("Stefan");
    //    var result = sut.Borrow("Stefan");

    //    Assert.IsFalse(result);
    //}






    //Låna en bok om  alla är utlånade ska inte gå



}