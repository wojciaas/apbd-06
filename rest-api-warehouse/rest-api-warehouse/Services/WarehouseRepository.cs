using System.Data.Common;
using System.Data.SqlClient;
using rest_api_warehouse.DTOs;
using rest_api_warehouse.Interfaces;

namespace rest_api_warehouse.Services;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly IConfiguration _configuration;

    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> DoesProductExist(int id)
    {
        await using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await using SqlCommand com = new SqlCommand();

        com.Connection = conn;
        com.CommandText = "SELECT 1 FROM cw7.Product WHERE IdProduct = @Id";
        com.Parameters.AddWithValue("@Id", id);

        await conn.OpenAsync();
        
        return await com.ExecuteScalarAsync() != null;
    }

    public async Task<bool> DoesWarehouseExist(int id)
    {
        await using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await using SqlCommand com = new SqlCommand();

        com.Connection = conn;
        com.CommandText = "SELECT 1 FROM cw7.Warehouse WHERE IdProduct = @Id";
        com.Parameters.AddWithValue("@Id", id);

        await conn.OpenAsync();
        
        return await com.ExecuteScalarAsync() != null;
    }

    public async Task<bool> DoesOrderExist(int id, int amount, DateTime createdAt)
    {
        await using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await using SqlCommand com = new SqlCommand();

        com.Connection = conn;
        com.CommandText = "SELECT 1 FROM cw7.Order WHERE IdOrder = @Id AND Amount = @Amount " +
                          "AND CreatedAt < @CreatedAt AND FulfilledAt = NULL";
        com.Parameters.AddWithValue("@Id", id);
        com.Parameters.AddWithValue("@Amount", amount);
        com.Parameters.AddWithValue("@CreatedAt", createdAt);

        await conn.OpenAsync();

        return await com.ExecuteScalarAsync() != null;
    }

    public async Task<int> GetOrderId(int id, int amount, DateTime createdAt)
    {
        await using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await using SqlCommand com = new SqlCommand();

        com.Connection = conn;
        com.CommandText = "SELECT IdOrder FROM cw7.Order WHERE IdProduct = @Id AND Amount = @Amount " +
                          "AND CreatedAt < @CreatedAt AND FulfilledAt = NULL";
        com.Parameters.AddWithValue("@Id", id);
        com.Parameters.AddWithValue("@Amount", amount);
        com.Parameters.AddWithValue("@CreatedAt", createdAt);

        await conn.OpenAsync();
        
        return (int)await com.ExecuteScalarAsync();
    }

    public async Task<decimal> GetProductPrice(int id)
    {
        await using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await using SqlCommand com = new SqlCommand();

        com.Connection = conn;
        com.CommandText = "SELECT Price FROM cw7.Product WHERE IdProduct = @Id";
        com.Parameters.AddWithValue("@Id", id);

        return (decimal)await com.ExecuteScalarAsync();
    }

    public async Task<int> AddGoods(int idWarehouse, int idProduct, int idOrder, int amount, decimal price)
    {
        await using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await using SqlCommand com = new SqlCommand();

        com.Connection = conn;
        com.CommandText = "UPDATE cw7.Order SET FulfilledAt = @FulfilledAt WHERE IdOrder = @IdOrder";
        com.Parameters.AddWithValue("@FulfilledAt", DateTime.Now);
        com.Parameters.AddWithValue("@IdOrder", idOrder);

        await conn.OpenAsync();

        DbTransaction transaction = await conn.BeginTransactionAsync();
        com.Transaction = transaction as SqlTransaction;

        int id;
        
        try
        {
            await com.ExecuteScalarAsync();
            
            com.Parameters.Clear();
            com.CommandText = "INSERT INTO cw7.Product_Warehouse VALUES (@IdWarehouse, @IdProduct, @IdOrder, " +
                              "@Amount, @Price, @CreatedAt);" +
                              "SELECT @@IDENTITY AS ID;";
            com.Parameters.AddWithValue("@IdWarehouse", idWarehouse);
            com.Parameters.AddWithValue("@IdProduct", idProduct);
            com.Parameters.AddWithValue("@IdOrder", idOrder);
            com.Parameters.AddWithValue("@Amount", amount);
            com.Parameters.AddWithValue("@Price", amount * price);
            com.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

            id = Convert.ToInt32(await com.ExecuteScalarAsync());

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }

        return id;
    }
}