using FluentMigrator;

namespace PriceCalculator.Dal.Migrations;


[Migration(6, TransactionBehavior.None)]
public class AddAnomalyV1Type : Migration
{
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'anomalies_v1') THEN
            CREATE TYPE anomalies_v1 as
            (
                  id      bigint
                , good_id bigint
                , price   numeric(19, 5)
            );
        END IF;
    END
$$;";

        Execute.Sql(sql);
    }

    public override void Down()
    {
        const string sql = @"
DO $$
    BEGIN
        DROP TYPE IF EXISTS anomalies_v1;
    END
$$;";

        Execute.Sql(sql);
    }
}