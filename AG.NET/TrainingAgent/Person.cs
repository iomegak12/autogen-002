using Json.Schema.Generation;

namespace TrainingAgent;

[Title("Person")]
[Description("Represents a person with various attributes such as name, age, occupation, city, address, and hobbies.")]
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Occupation { get; set; }
    public string? City { get; set; }
    public string? Address { get; set; }
    public List<string>? Hobbies { get; set; }

    public Person(string name, int age, string occupation)
    {
        Name = name;
        Age = age;
        Occupation = occupation;
    }

    public override string ToString()
    {
        return $"{Name}, {Age} years old, works as a {Occupation}. " +
               $"{(City != null ? $"Lives in {City}. " : "")}" +
               $"{(Address != null ? $"Address: {Address}. " : "")}" +
               $"{(Hobbies != null && Hobbies.Count > 0 ? $"Hobbies: {string.Join(", ", Hobbies)}." : "")}";
    }
}