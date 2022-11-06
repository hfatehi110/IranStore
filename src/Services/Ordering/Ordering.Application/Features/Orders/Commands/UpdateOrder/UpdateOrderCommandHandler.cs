using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequest<UpdateOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;
        public UpdateOrderCommandHandler(ILogger<UpdateOrderCommandHandler> logger, IMapper mapper, IOrderRepository orderRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }


        public async Task<Unit> Handle(UpdateOrderCommand request,CancellationToken token)
        {
            var orderToUpdateItem = await _orderRepository.GetByIdAsynce(request.ID);
            if(orderToUpdateItem == null)
            {
                throw new NotFoundException(nameof(Order), request.ID);
            }
            _mapper.Map(request, orderToUpdateItem, typeof(UpdateOrderCommand), typeof(Order));
            await _orderRepository.UpdateAsynce(orderToUpdateItem);
            _logger.LogInformation($"Order {orderToUpdateItem.Id} is successfuly Update");
            return Unit.Value;
        }   
    }
}
