using Serializer;
using System.Collections;
using System.Text;

var userProfiles = new List<UserProfile>
{
    new() { PrimaryDetail = "kdkd", SecondaryDetail = "vodka" },
    new() { PrimaryDetail = "kdkd", SecondaryDetail = "vodka" },
    new() { PrimaryDetail = "kdkd", SecondaryDetail = "vodka" },
    new() { PrimaryDetail = "kdkd", SecondaryDetail = "vodka", MetricValue = double.NaN, Price = 10.2m, ItemCount = -1 }
};

var singleProfile = new UserProfile() { PrimaryDetail = "kdkd\n", SecondaryDetail = "vodka", UserRectangle = new()
{
        SideA = 12,
        sideB = 13
} };

var mixedValuesList = new ArrayList
{
    1, 2, 3, 4, 1.2, 3.4f, -0, 12m, double.NaN, "price"
};

var numbersArray = new int[4] {
    1,
    2,
    3,
    4
};

var stringArray = new string[] {
    "32",
    "jdkk",
    "kf"
};

var dictionary = new Dictionary<string, int>{
    {"one", 1},
    {"two", 2}
};

var testStr = "lox";

var isActive = true;

var messageBuilder = new StringBuilder();

var currentStatus = Status.Deactivated;

var jsonOptions = new JsonSerializerOptions(){
    IgnoreNullProperties = true,
    SerializeEnumAsString = false
};

var json = JsonSerializer.Serialize(testStr);
File.WriteAllText("Test.json", json);

Console.WriteLine(json);


enum Status
{
    Active,
    Deactivated
}

class Rectangle
{
    public int SideA { get; set; } = 12;
    public int sideB { get; set; } = 25;
}

class UserProfile
{
    public string SecondaryDetail { get; set; }
    public string PrimaryDetail { get; set; }
    public string AdditionalDetail { get; set; }
    public int ItemCount { get; set; }
    public double MetricValue { get; set; }
    public decimal Price { get; set; }
    public List<int> Numbers { get; set; } = [2, 5, 4, 5, 5, 12];
    public Status UserStatus { get; set; }

    public Rectangle UserRectangle { get; set; }
}
