
open System

#r "../../../packages/EntityFramework/lib/net45/EntityFramework.dll"
#r "../../../packages/EntityFramework/lib/net45/EntityFramework.SqlServer.dll"
#r "System.Data"
#r "../../../packages/MySql.Data/lib/net45/MySql.Data.dll"
#r "../../../packages/MySQL.Data.Entities/lib/net45/mysql.data.entity.EF6.dll"
#r "../../../packages/Yaaf.Database/lib/net45/Yaaf.Database.dll"
#r "../../../packages/Yaaf.Database.MySQL/lib/net45/Yaaf.Database.MySQL.dll"
#r "bin/Debug/Yaaf.MessageArchiveManager.MySQL.dll"

open Yaaf.Xmpp.MessageArchiveManager.Sql.MySql.Migrations
open System.Data.Entity.Migrations
open System.Data.Entity.Migrations.Infrastructure
open System.Data.Entity
open System.Data.Entity.Infrastructure

DbConfiguration.SetConfiguration(MySql.Data.Entity.MySqlEFConfiguration())
let config = new Configuration()
config.TargetDatabase <- 
  DbConnectionInfo(
    "Server=localhost;Database=xmpp_develop;Uid=xmpp_sql;Pwd=jkYjgeriE8EIEIPrJNb8", 
    "MySql.Data.MySqlClient")
let migrator = new DbMigrator(config)
let scriptor = new MigratorScriptingDecorator(migrator)
let script = scriptor.ScriptUpdate(sourceMigration = null, targetMigration = null)
let fix_script (script:string) =
  script.Replace("from information_schema.columns where", "FROM information_schema_columns WHERE table_schema = SCHEMA() AND")
  // Possibly add some semicolons
let fixed_script = fix_script script