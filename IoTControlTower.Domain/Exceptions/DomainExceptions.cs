namespace IoTControlTower.Domain.Validation;

public class DomainExceptions(string error) : Exception(error)
{
    public static void When(bool hasError, string error)
    {
        if (hasError)
            throw new DomainExceptions(error);
    }
}
