using FluentMigrator;

namespace Route256.Week5.Homework.PriceCalculator.Dal.Migrations;

[Migration(20230303, TransactionBehavior.None)]
public class AddCalculationV1Type : Migration
{
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'calculations_v1') THEN
            CREATE TYPE calculations_v1 as
            (
                  id           bigint
                , user_id      bigint
                , good_ids     bigint[]
                , total_volume double precision
                , total_weight double precision
                , price        numeric(19, 5)
                , at           timestamp with time zone
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
        DROP TYPE IF EXISTS calculations_v1;
    END
$$;";

        Execute.Sql(sql);
    }
}