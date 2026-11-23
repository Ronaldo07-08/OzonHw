using System.Text;
using Dapper;
using Domain;
using Npgsql;

namespace Infrastructure;

public class PostgresProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public PostgresProductRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<long> Create(Product product, CancellationToken ct = default)
    {
        const string sql = """
            INSERT INTO products (name, price, weight, type, created_at, warehouse_num)
            VALUES (@Name, @Price, @Weight, @Type, @CreatedAt, @WarehouseNum)
            RETURNING id;
            """;

        await using var connection = new NpgsqlConnection(_connectionString);
        var command = new CommandDefinition(
            sql,
            new
            {
                product.Name,
                product.Price,
                product.Weight,
                Type = (int)product.Type,
                CreatedAt = DateTime.SpecifyKind(product.CreatedAt, DateTimeKind.Utc),
                product.WarehouseNum
            },
            cancellationToken: ct);

        return await connection.ExecuteScalarAsync<long>(command);
    }

    public async Task<Product?> GetById(long id, CancellationToken ct = default)
    {
        const string sql = """
            SELECT
                id AS Id,
                name AS Name,
                price AS Price,
                weight AS Weight,
                type AS Type,
                created_at AS CreatedAt,
                warehouse_num AS WarehouseNum
            FROM products
            WHERE id = @Id;
            """;

        await using var connection = new NpgsqlConnection(_connectionString);
        var command = new CommandDefinition(sql, new { Id = id }, cancellationToken: ct);

        return await connection.QuerySingleOrDefaultAsync<Product>(command);
    }

    public async Task<IReadOnlyList<Product>> GetFiltered(
        ProductType? type,
        DateTime? createdAt,
        int? warehouseNum,
        int offset,
        int limit,
        CancellationToken ct = default)
    {
        var sqlBuilder = new StringBuilder("""
            SELECT
                id AS Id,
                name AS Name,
                price AS Price,
                weight AS Weight,
                type AS Type,
                created_at AS CreatedAt,
                warehouse_num AS WarehouseNum
            FROM products
            WHERE 1 = 1
            """);

        var parameters = new DynamicParameters();

        if (type.HasValue)
        {
            sqlBuilder.Append(" AND type = @Type");
            parameters.Add("Type", (int)type.Value);
        }

        if (warehouseNum.HasValue)
        {
            sqlBuilder.Append(" AND warehouse_num = @WarehouseNum");
            parameters.Add("WarehouseNum", warehouseNum.Value);
        }

        if (createdAt.HasValue)
        {
            var startUtc = DateTime.SpecifyKind(createdAt.Value.Date, DateTimeKind.Utc);
            var endUtc = startUtc.AddDays(1);
            sqlBuilder.Append(" AND created_at >= @StartUtc AND created_at < @EndUtc");
            parameters.Add("StartUtc", startUtc);
            parameters.Add("EndUtc", endUtc);
        }

        sqlBuilder.Append(" ORDER BY id LIMIT @Limit OFFSET @Offset;");
        parameters.Add("Limit", limit);
        parameters.Add("Offset", offset);

        await using var connection = new NpgsqlConnection(_connectionString);
        var command = new CommandDefinition(sqlBuilder.ToString(), parameters, cancellationToken: ct);

        var items = await connection.QueryAsync<Product>(command);
        return items.AsList();
    }

    public async Task<bool> Update(Product product, CancellationToken ct = default)
    {
        const string sql = """
            UPDATE products
            SET
                name = @Name,
                price = @Price,
                weight = @Weight,
                type = @Type,
                warehouse_num = @WarehouseNum
            WHERE id = @Id;
            """;

        await using var connection = new NpgsqlConnection(_connectionString);
        var command = new CommandDefinition(
            sql,
            new
            {
                product.Id,
                product.Name,
                product.Price,
                product.Weight,
                Type = (int)product.Type,
                product.WarehouseNum
            },
            cancellationToken: ct);

        var affectedRows = await connection.ExecuteAsync(command);
        return affectedRows > 0;
    }
}