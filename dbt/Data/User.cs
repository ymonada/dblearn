namespace dbt.Data;

public class User
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public int Age { get; set; }
    public ICollection<Note> Notes { get; set; }

}