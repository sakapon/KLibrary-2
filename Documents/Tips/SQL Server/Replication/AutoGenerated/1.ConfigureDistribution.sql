/****** レプリケーション構成のスクリプトを作成します。スクリプト日付: 2012/06/15 2:02:38 ******/
/****** 注意: セキュリティ上の理由で、パスワード パラメーターは NULL または空文字列として作成されます。 ******/
-- sp_adddistributor に渡すパスワードは空文字列のままでよいです。

/****** サーバーをディストリビューターとしてインストールします。スクリプト日付: 2012/06/15 2:02:38 ******/
use master
exec sp_adddistributor @distributor = N'KEIHOPCVMDB1\SQLSTANDARD', @password = N''
GO
exec sp_adddistributiondb @database = N'distribution', @data_folder = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLSTANDARD\MSSQL\Data', @log_folder = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLSTANDARD\MSSQL\Data', @log_file_size = 2, @min_distretention = 0, @max_distretention = 72, @history_retention = 48, @security_mode = 1
GO

use [distribution] 
if (not exists (select * from sysobjects where name = 'UIProperties' and type = 'U ')) 
	create table UIProperties(id int) 
if (exists (select * from ::fn_listextendedproperty('SnapshotFolder', 'user', 'dbo', 'table', 'UIProperties', null, null))) 
	EXEC sp_updateextendedproperty N'SnapshotFolder', N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLSTANDARD\MSSQL\ReplData', 'user', dbo, 'table', 'UIProperties' 
else 
	EXEC sp_addextendedproperty N'SnapshotFolder', N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLSTANDARD\MSSQL\ReplData', 'user', dbo, 'table', 'UIProperties'
GO

exec sp_adddistpublisher @publisher = N'keihopcvmdb1\SQLStandard', @distribution_db = N'distribution', @security_mode = 0, @login = N'sa', @password = N'P@ssw0rd', @working_directory = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLSTANDARD\MSSQL\ReplData', @trusted = N'false', @thirdparty_flag = 0, @publisher_type = N'MSSQLSERVER'
GO
