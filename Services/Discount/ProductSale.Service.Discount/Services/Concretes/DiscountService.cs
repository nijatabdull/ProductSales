using Dapper;
using Dapper.Contrib.Extensions;
using Npgsql;
using ProductSale.Service.Discount.Services.Abstractions;
using ProductSale.Shared.Infrastructure.Response;
using ProductSale.Shared.Infrastructure.Response.Base;
using System.Data;

namespace ProductSale.Service.Discount.Services.Concretes
{
    public class DiscountService : IDiscountService
    {
        private readonly IDbConnection _connection;

        public DiscountService(IConfiguration configuration)
        {
            _connection = new NpgsqlConnection(configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response> All()
        {
            IEnumerable<Models.Discount> discounts = await _connection.QueryAsync<Models.Discount>("Select * from discount");

            return new SuccessResponse<IEnumerable<Models.Discount>>(discounts);
        }

        public async Task<Response> Create(Models.Discount discount)
        {
           int status = await _connection.ExecuteAsync("INSERT INTO discount(userId,rate,code) VALUES(@UserId,@Rate,@Code)",
               new { UserId = discount.UserId, Rate = discount.Rate, Code = discount.Code });

            if (status > 0)
                return new SuccessResponse<Models.Discount>(discount);

            return new ErrorResponse("Can not be inserted");
        }

        public async Task<Response> Delete(int id)
        {
            int status = await _connection.ExecuteAsync("delete from discount where id = @id",new { id });

            if (status > 0)
                return new SuccessResponse<Models.Discount>();

            return new ErrorResponse("Can not be inserted");
        }

        public async Task<Response> GetByCode(string code, string userId)
        {
            Models.Discount discount = await _connection
                    .QueryFirstOrDefaultAsync<Models.Discount>("select * from discount where userid = @UserId and code = @Code", 
                            new { UserId = userId, Code = code });

            return new SuccessResponse<Models.Discount>(discount);
        }

        public async Task<Response> GetById(int id)
        {
            Models.Discount discount = await _connection.QuerySingleOrDefaultAsync<Models.Discount>("Select * from discount where id = @id", new { id });

            return new SuccessResponse<Models.Discount>(discount);
        }

        public async Task<Response> Update(Models.Discount discount)
        {
            bool status = await _connection.UpdateAsync(discount);

            if (status)
                return new SuccessResponse<Models.Discount>(discount);

            return new ErrorResponse("Can not be updated");
        }
    }
}
