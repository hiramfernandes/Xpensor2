using Xpensor2.Domain.Contracts;
using Xpensor2.Domain.Models;

namespace Xpensor2.Application.AddPayment
{
    public interface IPaymentService
    {
        Task<Payment> AddInstallmentPayment(User user, string description, decimal installmentValue, int numberOfInstallments, int dueDay, DateTime startDate);
        Task<Payment> AddRecurringPayment(User user, string description, decimal amount, int dueDay);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Payment> AddRecurringPayment(User user, string description, decimal amount, int dueDay)
        {
            var newPayment = user.CreateRecurringPayment(description, amount, dueDay);
            await _paymentRepository.AddPayment(newPayment);

            return newPayment;
        }

        public async Task<Payment> AddInstallmentPayment(User user,
                                             string description,
                                             decimal installmentValue,
                                             int numberOfInstallments,
                                             int dueDay,
                                             DateTime startDate)
        {
            var installment = user.CreateInstallment(description, installmentValue, numberOfInstallments, dueDay, startDate);
            await _paymentRepository.AddPayment(installment);

            return installment;
        }
    }
}
