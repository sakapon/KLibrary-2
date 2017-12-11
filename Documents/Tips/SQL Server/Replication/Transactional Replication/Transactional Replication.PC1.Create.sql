-- データベース名 [Sync1] を手動で置換してください。
-- ログイン [keihopcvmdb1\repl] を手動で置換してください。
-- テーブル名を手動で指定してください。

-- データベースの作成
use [master]

create database [Sync1]
go

use [Sync1]

CREATE TABLE [dbo].[Persons](
    [Id] [int] NOT NULL,
    [Name] [nvarchar](10) NOT NULL,
    CONSTRAINT [PK_Persons] PRIMARY KEY CLUSTERED ([Id] ASC)
)

INSERT INTO [Persons] ([Id], [Name]) VALUES (1, N'qqq')
INSERT INTO [Persons] ([Id], [Name]) VALUES (2, N'www')
INSERT INTO [Persons] ([Id], [Name]) VALUES (3, N'eee')
go

use [master]

-- 変数の定義
declare @pc1_instance nvarchar(50) = N'keihopcvmdb1\SQLStandard';

-- ディストリビューションの作成
exec sp_adddistributor @distributor = @pc1_instance, @password = N''
exec sp_adddistributiondb @database = N'distribution', @log_file_size = 2, @min_distretention = 0, @max_distretention = 72, @history_retention = 48, @security_mode = 1

-- SQL Server ユーザーの作成
create login [keihopcvmdb1\repl] from windows with default_database = [master]
--create login [repl] with password = N'P@ssw0rd', default_database = [master]

go

use [distribution] 

-- 変数の定義
declare @pc1_instance nvarchar(50) = N'keihopcvmdb1\SQLStandard';
declare @pc1_sa_password nvarchar(50) = N'P@ssw0rd';
declare @pc1_sql_login nvarchar(50) = N'keihopcvmdb1\repl';
declare @pc1_repldata_path nvarchar(500) = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLSTANDARD\MSSQL\ReplData';

-- ディストリビューションの作成
if (not exists (select * from sysobjects where name = 'UIProperties' and type = 'U ')) 
    create table UIProperties(id int) 
if (exists (select * from ::fn_listextendedproperty('SnapshotFolder', 'user', 'dbo', 'table', 'UIProperties', null, null))) 
    EXEC sp_updateextendedproperty N'SnapshotFolder', @pc1_repldata_path, 'user', dbo, 'table', 'UIProperties' 
else 
    EXEC sp_addextendedproperty N'SnapshotFolder', @pc1_repldata_path, 'user', dbo, 'table', 'UIProperties'

exec sp_adddistpublisher @publisher = @pc1_instance, @distribution_db = N'distribution', @security_mode = 0, @login = N'sa', @password = @pc1_sa_password, @working_directory = @pc1_repldata_path, @trusted = N'false', @thirdparty_flag = 0, @publisher_type = N'MSSQLSERVER'

-- SQL Server ユーザーの作成
create user [keihopcvmdb1\repl]
exec sp_addrolemember N'db_owner', @pc1_sql_login

go

use [Sync1]

-- 変数の定義
declare @target_database nvarchar(50) = N'Sync1';
declare @target_publication nvarchar(50) = N'Sync1_Transactional';
declare @pc2_instance nvarchar(50) = N'keihopcvmdb2\SQLStandard';
declare @pc1_sql_login nvarchar(50) = N'keihopcvmdb1\repl';
declare @pc2_sql_login nvarchar(50) = N'keihopcvmdb2\repl';
declare @pc1_sql_password nvarchar(50) = N'P@ssw0rd';
declare @pc2_sql_password nvarchar(50) = N'P@ssw0rd';

-- SQL Server ユーザーの作成
create user [keihopcvmdb1\repl]
exec sp_addrolemember N'db_owner', @pc1_sql_login

-- パブリケーションの作成
exec sp_replicationdboption @dbname = @target_database, @optname = N'publish', @value = N'true'
exec [Sync1].sys.sp_addlogreader_agent @job_login = @pc1_sql_login, @job_password = @pc1_sql_password, @publisher_security_mode = 1, @job_name = null

exec sp_addpublication @publication = @target_publication, @sync_method = N'concurrent', @retention = 0, @allow_push = N'true', @allow_pull = N'true', @allow_anonymous = N'true', @enabled_for_internet = N'false', @snapshot_in_defaultfolder = N'true', @compress_snapshot = N'false', @ftp_port = 21, @ftp_login = N'anonymous', @allow_subscription_copy = N'false', @add_to_active_directory = N'false', @repl_freq = N'continuous', @status = N'active', @independent_agent = N'true', @immediate_sync = N'true', @allow_sync_tran = N'false', @autogen_sync_procs = N'false', @allow_queued_tran = N'false', @allow_dts = N'false', @replicate_ddl = 1, @allow_initialize_from_backup = N'false', @enabled_for_p2p = N'false', @enabled_for_het_sub = N'false'
exec sp_addpublication_snapshot @publication = @target_publication, @frequency_type = 1, @frequency_interval = 0, @frequency_relative_interval = 0, @frequency_recurrence_factor = 0, @frequency_subday = 0, @frequency_subday_interval = 0, @active_start_time_of_day = 0, @active_end_time_of_day = 235959, @active_start_date = 0, @active_end_date = 0, @job_login = @pc1_sql_login, @job_password = @pc1_sql_password, @publisher_security_mode = 1
exec sp_addarticle @publication = @target_publication, @article = N'Persons', @source_owner = N'dbo', @source_object = N'Persons', @type = N'logbased', @description = null, @creation_script = null, @pre_creation_cmd = N'drop', @schema_option = 0x000000000803509F, @identityrangemanagementoption = N'manual', @destination_table = N'Persons', @destination_owner = N'dbo', @vertical_partition = N'false', @ins_cmd = N'CALL sp_MSins_dboPersons', @del_cmd = N'CALL sp_MSdel_dboPersons', @upd_cmd = N'SCALL sp_MSupd_dboPersons'

-- パブリケーション アクセス リストの設定
exec sp_grant_publication_access @publication = @target_publication, @login = @pc1_sql_login

-- サブスクリプションの作成
exec sp_addsubscription @publication = @target_publication, @subscriber = @pc2_instance, @destination_db = @target_database, @subscription_type = N'Push', @sync_type = N'automatic', @article = N'all', @update_mode = N'read only', @subscriber_type = 0
exec sp_addpushsubscription_agent @publication = @target_publication, @subscriber = @pc2_instance, @subscriber_db = @target_database, @job_login = @pc1_sql_login, @job_password = @pc1_sql_password, @subscriber_security_mode = 1, @frequency_type = 64, @frequency_interval = 0, @frequency_relative_interval = 0, @frequency_recurrence_factor = 0, @frequency_subday = 0, @frequency_subday_interval = 0, @active_start_time_of_day = 0, @active_end_time_of_day = 235959, @active_start_date = 20120616, @active_end_date = 99991231, @enabled_for_syncmgr = N'False', @dts_package_location = N'Distributor'

go
