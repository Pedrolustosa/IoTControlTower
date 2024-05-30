namespace IoTControlTower.Domain.Validation
{
    public class DomainValidation(string error) : Exception(error)
    {
        public static void When(bool hasError, string error)
        {
            if (hasError)
                throw new DomainValidation(error);
        }
    }
}
