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

var singleProfile = new UserProfile() { PrimaryDetail = "kdkd\n", SecondaryDetail = "vodka" };

var mixedValuesList = new ArrayList
{
    1, 2, 3, 4, 1.2, 3.4f, -0, 12m, double.NaN
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

var isActive = true;

var messageBuilder = new StringBuilder();

var currentStatus = Status.Deactivated;

var json = JsonSerializer.Serialize(singleProfile);

Console.WriteLine(json);


enum Status
{
    Active,
    Deactivated
}

class UserProfile
{
    public string SecondaryDetail { get; set; }
    public string PrimaryDetail { get; set; }
    public string AdditionalDetail { get; set; }
    public int ItemCount { get; set; }
    public double MetricValue { get; set; }
    public decimal Price { get; set; }
}
