-- データベース名 [Sync1] を手動で置換してください。
-- ログイン [keihopcvmdb1\repl] を手動で置換してください。

use [Sync1]

-- 変数の定義
declare @target_database nvarchar(50) = N'Sync1';
declare @target_publication nvarchar(50) = N'Sync1_Transactional';
declare @pc2_instance nvarchar(50) = N'keihopcvmdb2\SQLStandard';

-- サブスクリプションの削除
exec sp_dropsubscription @publication = @target_publication, @subscriber = @pc2_instance, @destination_db = @target_database, @article = N'all'

-- パブリケーションの削除
exec sp_droppublication @publication = @target_publication
exec sp_replicationdboption @dbname = @target_database, @optname = N'publish', @value = N'false'

go

use [master]

-- ディストリビューションの削除
exec sp_dropdistributor @no_checks = 1

-- データベースの削除
drop database [Sync1]

-- SQL Server ユーザーの削除
drop login [keihopcvmdb1\repl]

go
