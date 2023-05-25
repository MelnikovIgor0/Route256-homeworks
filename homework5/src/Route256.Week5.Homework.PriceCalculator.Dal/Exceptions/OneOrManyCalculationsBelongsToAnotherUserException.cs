using System.Runtime.Serialization;

namespace Route256.Week5.Homework.PriceCalculator.Bll.Exceptions;

[Serializable]
public class OneOrManyCalculationsBelongsToAnotherUserException : Exception
{
    //
    // For guidelines regarding the creation of new exception types, see
    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
    // and
    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
    //

    public readonly IEnumerable<long> WrongIds = new List<long>();
    public OneOrManyCalculationsBelongsToAnotherUserException()
    {
    }
    
    public OneOrManyCalculationsBelongsToAnotherUserException(IEnumerable<long> wrongIds)
    {
        WrongIds = wrongIds;
    }

    public OneOrManyCalculationsBelongsToAnotherUserException(string message) : base(message)
    {
    }

    public OneOrManyCalculationsBelongsToAnotherUserException(string message, Exception inner) : base(message, inner)
    {
    }

    protected OneOrManyCalculationsBelongsToAnotherUserException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}
