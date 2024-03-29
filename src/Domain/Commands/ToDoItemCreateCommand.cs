﻿using Responses;

namespace Domain.Commands;

public class ToDoItemCreateCommand
{
    public string Name { get; set; } = string.Empty;
    public DateTime? Deadline { get; set; }

    public virtual Result Validate()
    {
        var list = new List<KeyValuePair<string, string>>(2);

        if (Deadline.HasValue && Deadline.Value < DateTime.Now.Date)
            list.Add(new KeyValuePair<string, string>("DeadLine",
                "A data de termino não pode der menor que a data atual"));

        if (string.IsNullOrWhiteSpace(Name) || (Name?.Length < 3 || Name?.Length > 60))
            list.Add(new KeyValuePair<string, string>("Name", "O nome deva conter de 3 a 60 letras"));

        return list.Any() ? Result.Fail(new Error("404", "Campo(s) violando regras", list)) : Result.Ok();
    }
}