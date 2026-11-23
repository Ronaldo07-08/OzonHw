using FluentMigrator;

namespace Infrastructure.Migrations;

[Migration(2026112301, "Create products table")]
public class InitialMigration : Migration
{
    public override void Up()
    {
        Create.Table("products")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("price").AsDecimal(18, 2).NotNullable()
            .WithColumn("weight").AsDouble().NotNullable()
            .WithColumn("type").AsInt32().NotNullable()
            .WithColumn("created_at").AsCustom("TIMESTAMPTZ").NotNullable()
            .WithColumn("warehouse_num").AsInt32().NotNullable();

        Create.Index("ix_products_type").OnTable("products").OnColumn("type");
        Create.Index("ix_products_warehouse_num").OnTable("products").OnColumn("warehouse_num");
        Create.Index("ix_products_created_at").OnTable("products").OnColumn("created_at");
    }

    public override void Down()
    {
        Delete.Table("products");
    }
}