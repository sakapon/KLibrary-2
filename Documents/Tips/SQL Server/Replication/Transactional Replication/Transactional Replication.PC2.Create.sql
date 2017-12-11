-- データベース名 [Sync1] を手動で置換してください。
-- ログイン [keihopcvmdb2\repl] を手動で置換してください。

use [master]

-- データベースの作成
create database [Sync1]

-- SQL Server ユーザーの作成
create login [keihopcvmdb2\repl] from windows with default_database = [master]

go

use [Sync1]

create user [keihopcvmdb2\repl]
exec sp_addrolemember N'db_owner', N'keihopcvmdb2\repl'

go
