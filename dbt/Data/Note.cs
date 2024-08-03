using System.Text.Json.Serialization;

namespace dbt.Data;

public class Note
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid UserId { get; set; }
    [JsonIgnore]
    public User user { get; set; }

}