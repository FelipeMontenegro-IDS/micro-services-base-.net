namespace Application.Interfaces.Faker;

public interface IFaker<T> where T : class
{
    T Generate();
    IEnumerable<T> Generate(int count);
}