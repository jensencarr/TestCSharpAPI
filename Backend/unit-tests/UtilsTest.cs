namespace WebApp;

public class UtilsTest(Xlog Console)
{
    // Read all mock users from file
    private static readonly Arr mockUsers = JSON.Parse(
        File.ReadAllText(FilePath("json", "mock-users.json"))
    );

    [Theory]
    [InlineData("abC9#fgh", true)]  // ok
    [InlineData("stU5/xyz", true)]  // ok too
    [InlineData("abC9#fg", false)]  // too short
    [InlineData("abCd#fgh", false)] // no digit
    [InlineData("abc9#fgh", false)] // no capital letter
    [InlineData("abC9efgh", false)] // no special character
    public void TestIsPasswordGoodEnough(string password, bool expected)
    {
        Assert.Equal(expected, Utils.IsPasswordGoodEnough(password));
    }

    [Theory]
    [InlineData("abC9#fgh", true)]  // ok
    [InlineData("stU5/xyz", true)]  // ok too
    [InlineData("abC9#fg", false)]  // too short
    [InlineData("abCd#fgh", false)] // no digit
    [InlineData("abc9#fgh", false)] // no capital letter
    [InlineData("abC9efgh", false)] // no special character
    public void TestIsPasswordGoodEnoughRegexVersion(string password, bool expected)
    {
        Assert.Equal(expected, Utils.IsPasswordGoodEnoughRegexVersion(password));
    }

    [Theory]
    [InlineData(
        "---",
        "Hello, I am going through hell. Hell is a real fucking place " +
            "outside your goddamn comfy tortoiseshell!",
        "Hello, I am going through ---. --- is a real --- place " +
            "outside your --- comfy tortoiseshell!"
    )]
    [InlineData(
        "---",
        "Rhinos have a horny knob? (or what should I call it) on " +
            "their heads. And doorknobs are damn round.",
        "Rhinos have a --- ---? (or what should I call it) on " +
            "their heads. And doorknobs are --- round."
    )]
    public void TestRemoveBadWords(string replaceWith, string original, string expected)
    {
        Assert.Equal(expected, Utils.RemoveBadWords(original, replaceWith));
    }

    [Fact]
    public void TestCreateMockUsers()
    {
        Arr usersInDb = SQLQuery("SELECT email FROM users");
        Arr emailsInDb = usersInDb.Map(user => user.email);
        Arr mockUsersNotInDb = mockUsers.Filter(
            mockUser => !emailsInDb.Contains(mockUser.email)
        );
        var result = Utils.CreateMockUsers();
        
        Console.WriteLine($"The test expected that {mockUsersNotInDb.Length} users should be added.");
        Console.WriteLine($"And {result.Length} users were added.");
        Console.WriteLine("The test also asserts that the users added " +
            "are equivalent (the same) as the expected users!");
        Console.WriteLine("LENGTH OF mockUsersNotInDb "+ mockUsersNotInDb.Length);
        Console.WriteLine("LENGTH OF result "+ result.Length);
        Assert.Equivalent(mockUsersNotInDb, result);
        Console.WriteLine("The test passed!");
    }
    [Fact]
    public void TestRemoveMockUsers()
{
    Arr usersInDbBeforeRemoval = SQLQuery("SELECT email FROM users");
    Arr emailsInDbBeforeRemoval = usersInDbBeforeRemoval.Map(user => user.email);

    Arr mockUsersInDb = mockUsers.Filter(
        mockUser => emailsInDbBeforeRemoval.Contains(mockUser.email)
    );

    var result = Utils.RemoveMockUsers();

    Console.WriteLine($"The test expected that {mockUsersInDb.Length} users should be removed.");
    Console.WriteLine($"And {result.Length} users were removed.");
    Assert.Equivalent(mockUsersInDb, result);
    Console.WriteLine("The test passed!");
}

[Fact]
public void TestCountDomainsFromUserEmails()
{
    var expected = 3; // Korrekt värde är 3
    var result = Utils.CountSpecificDomainFromUserEmails("nodehill.com");

    Console.WriteLine("Expected: " + expected);
    Console.WriteLine("Result: " + result);

    Assert.Equal(expected, result);
}
}
    // Now write the two last ones yourself!
    // See: https://sys23m-jensen.lms.nodehill.se/uploads/videos/2021-05-18T15-38-54/sysa-23-presentation-2024-05-02-updated.html#8
