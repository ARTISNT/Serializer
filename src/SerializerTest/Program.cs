using Serializer.Deserialization;
using Serializer.Serialization;
using System.Collections;
using System.Text;

var userProfiles = new List<UserProfile>
{
    new() { PrimaryDetail = "kdkd", SecondaryDetail = "vodka" },
    new() { PrimaryDetail = "kdkd", SecondaryDetail = "vodka" },
    new() { PrimaryDetail = "kdkd", SecondaryDetail = "vodka" },
    new() { PrimaryDetail = "kdkd", SecondaryDetail = "vodka", MetricValue = double.NaN, Price = 10.2m, ItemCount = -1 }
};

var singleProfile = new UserProfile
{
    PrimaryDetail = "kdkd\n",
    SecondaryDetail = "vodka",
    // UserRectangle = new()
    // {
    //     SideA = 12,
    //     sideB = 13
    // },
    // InUserProfile = new UserProfile()
    // {
    //     PrimaryDetail = "kjjjjjjj",
    //     SecondaryDetail = "jfkj",
    //     ItemCount = 13
    // }
};

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

var jsonOptions = new JsonSerializerOptions()
{
    IgnoreNullProperties = true,
    SerializeEnumAsString = false
};

var number = 12;

var json = JsonSerializer.Serialize(singleProfile);

var reader = new JsonReader(json);

while (reader.Read())
{
    Console.WriteLine(reader.TokenType);
    Console.WriteLine(reader.Value);
}

File.WriteAllText("Test.json", json);

Console.WriteLine(json);

var rectangle = new Rectangle()
{
    IsItSquare = true,
    NameOfRectangle = "Vasilisa",
    SideA = 13,
    SideB = 14
};

var json2 = JsonSerializer.Serialize(rectangle);
Console.WriteLine(json2);
var obj = JsonDesirializer.Desiriailize<Rectangle>(json2
            // """
            // {
            //     "SideA":12,
            //     "SideB": 23,
            //     "NameOfRectangle":"Vasilias",
            //     "IsItSquare": true
            // }
            // """
            );

var json3 = JsonSerializer.Serialize(obj);
Console.WriteLine(json2);


enum Status
{
    Active,
    Deactivated
}

class Rectangle
{
    public int SideA { get; set; } = 12;
    public int SideB { get; set; } = 25;
    public string NameOfRectangle { get; set; } = "Lolal";
    public bool IsItSquare { get; set; } = true;
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
    public bool IsMale { get; set; }

    public Rectangle UserRectangle { get; set; }

    public UserProfile InUserProfile { get; set; }
}
