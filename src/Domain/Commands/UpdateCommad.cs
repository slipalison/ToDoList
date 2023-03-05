using Domain.ToDoItems;
using Responses;

namespace Domain.Commands;

public class UpdateCommad : ToDoItemCreateCommand
{
    public Status Status { get; set; }

    public override Result Validate()
    {
        if (string.IsNullOrWhiteSpace(Name) || (Name?.Length < 3 || Name?.Length > 60))
            return Result.Fail(new Error("404", "Campo(s) violando regras",
                new[] { new KeyValuePair<string, string>("Name", "O nome deva conter de 3 a 60 letras") }));
        return Result.Ok();
    }
}