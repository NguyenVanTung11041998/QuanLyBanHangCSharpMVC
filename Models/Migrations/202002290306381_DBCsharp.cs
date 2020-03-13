namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBCsharp : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Cards", newName: "Carts");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Carts", newName: "Cards");
        }
    }
}
