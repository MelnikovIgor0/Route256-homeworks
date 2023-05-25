using System.Runtime.Serialization;

namespace Route256.Week5.Homework.PriceCalculator.Bll.Exceptions;

[Serializable]
public class OneOrManyCalculationsNotFoundException : Exception
{
    //
    // For guidelines regarding the creation of new exception types, see
    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
    // and
    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
    //

    public OneOrManyCalculationsNotFoundException()
    {
    }

    public OneOrManyCalculationsNotFoundException(string message) : base(message)
    {
    }

    public OneOrManyCalculationsNotFoundException(string message, Exception inner) : base(message, inner)
    {
    }

    protected OneOrManyCalculationsNotFoundException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}
