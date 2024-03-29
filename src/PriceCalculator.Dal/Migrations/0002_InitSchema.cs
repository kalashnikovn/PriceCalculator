﻿using FluentMigrator;

namespace PriceCalculator.Dal.Migrations;

[Migration(2, TransactionBehavior.None)]
public class InitSchema : Migration
{
    public override void Up()
    {
        Create.Table("goods")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("width").AsDouble().NotNullable()
            .WithColumn("height").AsDouble().NotNullable()
            .WithColumn("length").AsDouble().NotNullable()
            .WithColumn("weight").AsDouble().NotNullable();

        Create.Table("calculations")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("goods_id").AsCustom("bigint[]").NotNullable()
            .WithColumn("total_volume").AsDouble().NotNullable()
            .WithColumn("total_weight").AsDouble().NotNullable()
            .WithColumn("price").AsDouble().NotNullable()
            .WithColumn("at").AsDateTimeOffset().NotNullable();

        Create.Index("goods_user_id_ix")
            .OnTable("goods")
            .OnColumn("user_id");

        Create.Index("calculations_user_id_ix")
            .OnTable("calculations")
            .OnColumn("user_id");
    }

    public override void Down()
    {
        Delete.Table("goods");
        Delete.Table("calculations");
    }
}