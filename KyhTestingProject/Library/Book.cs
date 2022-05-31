namespace KyhTestingProject.Library;

public class Book
{
    public string Isbn { get; }
    public string Author { get; }

    public string Title{ get; }

    public int Count { get; protected set; }
    public int Borrowed => borrowers.Count;

    public bool Borrow(string borrower)
    {
        if (borrowers.Count() >= Count)   return false;
        borrowers.Add(borrower);
        return true;
    }

    public bool IsBorrower(string borrower)
    {
        return borrowers.Contains(borrower);
    }

    public void BuyNewEx(int count)
    {
        Count += count;
    }

    private List<string> borrowers = new List<string>();

    public int Available => Count - Borrowed;

    public Book(string isbn, string author, string title)
    {
        Isbn = isbn;
        Author = author;
        Count = 0;
        Title = title;
    }
}

/*För att förklara hur det används så är den ungefär såhär
		

    sut = new Book();
    sut.Title = "Harry";
    Assert.AreEqual("Harry", sut.Title);

    ```
{
    var books = new List<Book>
    {
        new Book("12223","Nils Nilsson", "Fantastiska dikter"),
        new Book("55551234","Åsa Åsasson", "Mina bästa TV-spel")
    };

    books[0].BuyNewEx(12);
    books[1].BuyNewEx(2);

    var borrower = "Stefan";
    if(books[0].IsBorrower(borrower) == false)
        books[0].Borrow(borrower);
    else
    {
        Console.WriteLine("Du har ju redan lånat den boken");
    }
    Console.WriteLine($"Antal kvar av {books[0].Title} är nu {books[0].Count} ");
                
        ```

		




    -att det inte går att låna en bok ifall alla är utlånade
        - att man blir  en borrower när man lånar en bok
        - att antalet ex ökar när man köper in nya exemplar

*/