using FluentMigrator;

namespace PriceCalculator.Dal.Migrations;

[Migration(4, TransactionBehavior.None)]
public class AddGoodV1Type : Migration
{
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'goods_v1') THEN
            CREATE TYPE goods_v1 as
            (
                  id      bigint
                , user_id bigint
                , width   double precision
                , height  double precision
                , length  double precision
                , weight  double precision
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
        DROP TYPE IF EXISTS goods_v1;
    END
$$;";

        Execute.Sql(sql);
    }
}