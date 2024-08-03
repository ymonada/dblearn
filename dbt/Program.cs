using dbt.Contracts;
using dbt.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("db"));
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapPost("/notes", CreateNote).WithOpenApi();
app.MapGet("/notes", GetNotes).WithOpenApi();
app.MapGet("/notes{userId}", GetNotesByUser).WithOpenApi();

app.MapGet("/users", GetUsers).WithOpenApi();
app.MapGet("/users{userId}", GetUserWithNote).WithOpenApi();
app.MapPost("/users", CreateUser).WithOpenApi();
app.Run();

async Task<IResult> CreateNote(ApplicationDbContext _context, NoteDto notedto)
{
    var userId = notedto.UserId;
    var user = _context.Users.AsNoTracking().FirstAsync(s=>s.Id == userId);
    if (user == null)
        return Results.BadRequest("not found user");
        
    var note = new Note()
    {
        Id = Guid.NewGuid()
        , Title = notedto.Title
        , Description = notedto.Description
        , UserId = notedto.UserId
    };
    await _context.Notes.AddAsync(note);
    await _context.SaveChangesAsync();
    return Results.Ok(note);
}
async Task<IResult> GetNotes(ApplicationDbContext _context)
{
    var notes = await _context.Notes.AsNoTracking().ToListAsync();
    return Results.Ok(notes);
}
async Task<IResult> GetNotesByUser(ApplicationDbContext _context,Guid userId)
{
    var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Id == userId);
    if (user == null) 
        return Results.BadRequest("not found user");
    var notes = await _context.Notes
        .AsNoTracking()
        .Where(s=>s.UserId == userId)
        .ToListAsync();
    return Results.Ok(notes);
}
async Task<IResult> GetUsers(ApplicationDbContext _context)
{
    var users = await _context.Users.AsNoTracking().ToListAsync();
    return Results.Ok(users);
}
async Task<IResult> GetUserWithNote(ApplicationDbContext _context,Guid userId)
{
    var user = await _context.Users
        .Include(s => s.Notes)
        .AsNoTracking()
        .FirstOrDefaultAsync(s => s.Id == userId);
    if (user == null)
        return Results.NotFound();
    return Results.Ok(user);
}
async Task<IResult> CreateUser(ApplicationDbContext _context, UserDto userdto)
{
    var user = new User()
    {
        Id = Guid.NewGuid()
        , Login =userdto.Login
        , Age =userdto.Age
    };
    await _context.Users.AddAsync(user);
    await _context.SaveChangesAsync();
    return Results.Ok(user);
}