namespace dbt.Contracts;

public record NoteDto(Guid UserId, string Title, string Description);
