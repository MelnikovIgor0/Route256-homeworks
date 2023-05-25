using FluentMigrator;

namespace Route256.Week5.Homework.PriceCalculator.Dal.Migrations;

[Migration(20230302, TransactionBehavior.None)]
public class InitSchema : Migration
{
    public override void Up()
    {
        Create.Table("goods")
            .WithColumn("id").AsInt64().PrimaryKey("goods_pk").Identity()
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("width").AsDouble().NotNullable()
            .WithColumn("height").AsDouble().NotNullable()
            .WithColumn("length").AsDouble().NotNullable()
            .WithColumn("weight").AsDouble().NotNullable();
        
        Create.Table("calculations")
            .WithColumn("id").AsInt64().PrimaryKey("calculations_pk").Identity()
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("good_ids").AsCustom("bigint[]").NotNullable()
            .WithColumn("total_volume").AsDouble().NotNullable()
            .WithColumn("total_weight").AsDouble().NotNullable()
            .WithColumn("price").AsDecimal().NotNullable()
            .WithColumn("at").AsDateTimeOffset().NotNullable();

        Create.Index("goods_user_id_index")
            .OnTable("goods")
            .OnColumn("user_id");
        
        Create.Index("calculations_user_id_index")
            .OnTable("calculations")
            .OnColumn("user_id");
    }

    public override void Down()
    {
        Delete.Table("goods");
        Delete.Table("calculations");
    }
}