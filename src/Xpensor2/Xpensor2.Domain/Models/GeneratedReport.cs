using System;
using System.Text.Json.Serialization;

namespace Xpensor2.Domain.Models;

public class GeneratedReport
{
    public Guid Id { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public required string UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
}
