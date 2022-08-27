using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DiscountService(ILogger logger, IDiscountRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} Not Found"));
            }
            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupone = _mapper.Map<Coupon>(request.Coupon);
            await _repository.CreateDiscount(coupone);
            _logger.LogInformation("Discount is successfully Create .ProductName : {ProductName}", coupone.ProductName);
            var couponeModel = _mapper.Map<CouponModel>(coupone);
            return couponeModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupone = _mapper.Map<Coupon>(request.Coupon);
            await _repository.UpdateDiscount(coupone);
            _logger.LogInformation("Discount is successfully Update .ProductName : {ProductName}", coupone.ProductName);
            var couponeModel = _mapper.Map<CouponModel>(coupone);
            return couponeModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var delete = await _repository.DeleteDiscount(request.ProductName);
            var response = new DeleteDiscountResponse
            {
                Success = delete
            };
            return response;
        }

    }
}
