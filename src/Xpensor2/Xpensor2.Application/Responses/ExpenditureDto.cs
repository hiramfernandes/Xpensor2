using System;

namespace Xpensor2.Application.Responses;

public record ExpenditureDto
{
    public string? Id { get; init; }
    public string? ExpenseName { get; init; }
    public DateTime DueDate { get; init; }
    public string? GeneralInfo { get; init; }
    public decimal Value { get; init; }
    public bool Paid { get; init; }
    public DateTime? PaymentDate { get; init; }
}
