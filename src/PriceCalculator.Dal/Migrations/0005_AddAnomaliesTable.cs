using FluentMigrator;

namespace PriceCalculator.Dal.Migrations;

[Migration(5, TransactionBehavior.None)]
public class AddAnomaliesTable : Migration
{
    public override void Up()
    {
        Create.Table("anomalies")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("good_id").AsInt64().NotNullable()
            .WithColumn("price").AsDecimal().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("anomalies");
    }
}