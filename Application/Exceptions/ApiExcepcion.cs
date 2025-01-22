using System.Globalization;

namespace Application.Exceptions;

public class ApiExcepcion : Exception
{
    public ApiExcepcion() : base()
    {
    }

    public ApiExcepcion(string message) : base(message)
    {
    }

    public ApiExcepcion(string message, params Object[] args)
        : base(string.Format(new CultureInfo("en-US"), message, args))
    {
    }
}