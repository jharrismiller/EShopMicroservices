using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {

        var coupon = await dbContext.Coupons.SingleOrDefaultAsync(c => c.ProductName == request.ProductName);

        if (coupon == null)
            coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };


        logger.LogInformation("Discount is retrieved for ProductName : {ProductName}, Amount : {Amount}", coupon.ProductName, coupon.Amount);

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        if (coupon == null)
            new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));

        dbContext.Coupons.Add(coupon);

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount is successfully created. ProductId : {Id}, ProductName : {ProductName}, Amount : {Amount}", coupon.Id, coupon.ProductName, coupon.Amount);

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        if (coupon == null)
            new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));

        dbContext.Coupons.Update(coupon);

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount updated for ProductName : {ProductName}, Amount : {Amount}", coupon.ProductName, coupon.Amount);

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

        if (coupon == null)
            new RpcException(new Status(StatusCode.InvalidArgument, $"Discount with ProductName '{request.ProductName}' is not found"));

        dbContext.Coupons.Remove(coupon);

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount removed for ProductName : {ProductName}", coupon.ProductName);

        return new DeleteDiscountResponse() { Success = true };
    }


    public override async Task<GetDiscountsResponse> GetDiscounts(EmptyRequest request, ServerCallContext context)
    {
        var response = new GetDiscountsResponse();
        response.Coupons.AddRange(await dbContext.Coupons.Select(x => x.Adapt<CouponSimple>()).ToArrayAsync());
        return response;
    }
}
