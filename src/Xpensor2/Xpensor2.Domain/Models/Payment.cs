using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;
using Xpensor2.Domain.Models.Enums;

namespace Xpensor2.Domain.Models;

[BsonIgnoreExtraElements]
public class Payment
{
    // Recurring Payment
    public Payment(string description, User owner, decimal nominalValue, int dueDay)
    {
        //Id = GenerateId();
        Description = description;
        Owner = owner;
        NominalValue = nominalValue;
        DueDay = dueDay;
        PaymentType = PaymentType.Recurring;
        Recurrence = PaymentRecurrence.Monthly;
    }

    // Installment
    public Payment(string description, User owner, decimal nominalValue, int dueDay, int? numberOfInstallments, DateTime startDate)
        : this(description, owner, nominalValue, dueDay)
    {
        NumberOfInstallments = numberOfInstallments;
        StartDate = startDate;
        PaymentType = PaymentType.Installment;
    }

    // Single
    public Payment(string description, User owner, decimal nominalValue, DateTime dueDate)
    {
        //Id = GenerateId();
        Description = description;
        DueDate = dueDate;
        PaymentType = PaymentType.Single;
        NominalValue = nominalValue;
        Owner = owner;
        Recurrence = PaymentRecurrence.None;
    }

    private string GenerateId()
    {
        return Guid.NewGuid().ToString();
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; set; }

    [DataMember]
    [BsonElement("description")]
    public string Description { get; init; }

    [DataMember]
    [BsonElement("nominalValue")]
    public decimal? NominalValue { get; set; }

    [DataMember]
    [BsonElement("dueDate")]
    public DateTime DueDate { get; set; }

    [DataMember]
    [BsonElement("dueDay")]
    public int DueDay { get; set; }

    [DataMember]
    [BsonElement("recurrence")]
    public PaymentRecurrence Recurrence { get; set; }

    [DataMember]
    [BsonElement("startDate")]
    public DateTime? StartDate { get; set; }

    [DataMember]
    [BsonElement("numberOfInstallments")]
    public int? NumberOfInstallments { get; set; }

    [DataMember]
    [BsonElement("paymentType")]
    public PaymentType PaymentType { get; private set; }

    [DataMember]
    [BsonElement("created")]
    public DateTime Created { get; set; }

    [DataMember]
    [BsonElement("modified")]
    public DateTime Modified { get; set; }

    [DataMember]
    [BsonElement("owner")]
    public User Owner { get; set; }
}
