using Xpensor2.Domain.Contracts;
using Xpensor2.Domain.Models;

namespace Xpensor2.Application.AddPayment
{
    public interface IPaymentService
    {
        Payment AddInstallmentPayment(User user, string description, decimal installmentValue, int numberOfInstallments, int dueDay, DateTime startDate);
        Payment AddRecurringPayment(User user, string description, decimal amount, int dueDay);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public Payment AddRecurringPayment(User user, string description, decimal amount, int dueDay)
        {
            var newPayment = user.CreateRecurringPayment(description, amount, dueDay);
            _paymentRepository.AddPayment(newPayment);

            return newPayment;
        }

        public Payment AddInstallmentPayment(User user,
                                             string description,
                                             decimal installmentValue,
                                             int numberOfInstallments,
                                             int dueDay,
                                             DateTime startDate)
        {
            var installment = user.CreateInstallment(description, installmentValue, numberOfInstallments, dueDay, startDate);
            _paymentRepository.AddPayment(installment);

            return installment;
        }
    }
}
