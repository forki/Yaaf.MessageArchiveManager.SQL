// ----------------------------------------------------------------------------
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
// ----------------------------------------------------------------------------
namespace Test.Yaaf.MessageArchiveManager.SQL

open Test.Yaaf.MessageArchiveManager
open Yaaf.Xmpp.MessageArchiveManager
open Yaaf.Xmpp.MessageArchiving
open Yaaf.Xmpp.MessageArchiveManager.Sql
open FsUnit
open NUnit.Framework
open Yaaf.TestHelper
open Yaaf.Database
open Yaaf.Xmpp
open Swensen.Unquote
open Foq
open System.Data.Entity

[<DbConfigurationType (typeof<EmptyConfiguration>)>]
type ApplicationDbTestContext() =
    inherit AbstractMessageArchivingDbContext(ApplicationDbTestContext.ConnectionName)

    override x.Init() = System.Data.Entity.Database.SetInitializer(new NUnitInitializer<ApplicationDbTestContext>())
    static member ConnectionName
      with get () =  
        let env = System.Environment.GetEnvironmentVariable ("connection_mssql")
        if System.String.IsNullOrWhiteSpace env then "ArchiveDb_MSSQL" else env
//type NormalDbTestContext() = 
//    inherit AbstractMessageArchivingDbContext("unittest")

[<TestFixture>]
[<Ignore>]
[<Category("MSSQL")>]
type ``Test-Yaaf-MessageArchiveManager-SQL: Test archive store``() = 
    inherit MessageArchivingStoreTest()

    override x.Setup() =
        // Fix for some bug, see http://stackoverflow.com/questions/15693262/serialization-exception-in-net-4-5
        System.Configuration.ConfigurationManager.GetSection("dummy") |> ignore
        base.Setup()

    override x.CreateArchivingStore(jid) = 
        use context = new ApplicationDbTestContext() :> AbstractMessageArchivingDbContext
        context.Database.Delete() |> ignore
        context.SaveChanges() |> ignore
        let store = MessageArchivingStore(fun () -> new ApplicationDbTestContext() :> AbstractMessageArchivingDbContext) :> IMessageArchivingStore
        store.GetArchiveStore (jid) |> waitTask

[<TestFixture>]
[<Category("MSSQL")>]
type ``Test-Yaaf-MessageArchiveManager-SQL: Test preference store``() = 
    inherit PreferenceStoreTests()

    override x.Setup() =
        // Fix for some bug, see http://stackoverflow.com/questions/15693262/serialization-exception-in-net-4-5
        System.Configuration.ConfigurationManager.GetSection("dummy") |> ignore
        base.Setup()

    override x.CreatePreferenceStore(jid) = 
        use context = new ApplicationDbTestContext() :> AbstractMessageArchivingDbContext
        context.Database.Delete() |> ignore
        context.SaveChanges() |> ignore
        let store = MessageArchivingStore(fun () -> new ApplicationDbTestContext() :> AbstractMessageArchivingDbContext) :> IMessageArchivingStore
        store.GetPreferenceStore(jid) |> waitTask
