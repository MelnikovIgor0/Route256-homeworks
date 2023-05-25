using System.Text;

namespace Route256.Week5.Workshop.PriceCalculator.Api.Extensions;

public static class StringExtensions
{
    public static string ToSnakeCase(this string text)
    {
        var sb = new StringBuilder
        {
            Capacity = 0,
            Length = 0
        };
        sb.Append(char.ToLowerInvariant(text[0]));
        for(int i = 1; i < text.Length; ++i) {
            var c = text[i];
            if(char.IsUpper(c)) 
            {
                sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            } else 
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }
}