namespace Xpensor2.Domain.Models;

public class User
{
    public User(string? name)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
    }

    public string Id { get; init; }
    public string? Name { get; init; }
    public IList<Expenditure> Expenditures { get; private set; } = [];

    public Expenditure? GetExpenditure(string id)
    {
        return Expenditures.FirstOrDefault(x => x.Id == id);
    }
}
