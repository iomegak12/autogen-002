using AutoGen.Core;

namespace TrainingAgent;

public partial class Tools
{
    [Function("UpperCase", "Converts a string to upper case.")]
    public async Task<string> UpperCase(string message)
    {
        Console.WriteLine($"UpperCase called with message: {message}");

        return message.ToUpper();
    }

    [Function("ConcatString", "Concatenates an array of strings into a single string.")]
    public async Task<string> ConcatString(string[] strings)
    {
        Console.WriteLine($"ConcatString called with strings: {string.Join(", ", strings)}");

        return string.Join(" ", strings);
    }

    [Function("CalculateTax", "Calculates the tax based on price and tax rate.")]
    public async Task<string> CalculateTax(int price, float taxRate)
    {
        Console.WriteLine($"CalculateTax called with price: {price}, taxRate: {taxRate}");

        var tax = price * taxRate;

        return $"The tax on {price} at a rate of {taxRate} is {tax}";
    }
}