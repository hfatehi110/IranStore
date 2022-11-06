using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Model;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository,
            IMapper mapper,
            IEmailService emailService,
            ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            var newOrder = await _orderRepository.AddAsynce(orderEntity);
            _logger.LogInformation($"Order {newOrder.Id} is Successduly Create.");
            await SendMail(newOrder);
            return newOrder.Id;

        }

        private async Task SendMail(Order newOrder)
        {
            var email = new Email() { To = "hfatehi110@gmail.com", Body = "Order Create Successfuly", Subject = "Order Creating" };
            try
            {
               await _emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Send Email For Order With ID : {newOrder.Id}. Message is : {ex.Message}");
            }
        }
    }
}
