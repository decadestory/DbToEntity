using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orm.Son.Core;
using DbToEntity.Entity;
using System.Data.SqlClient;

namespace DbToEntity.Core
{
    public static class DbHelper
    {
        public static  Tuple<string, List<TableName>> GetTablesName()
        {
            using (var db = new DbConnection())
            {
                var sql = string.Format("SELECT * FROM {0}..SysObjects Where XType='U' ORDER BY Name", db.Database);
                var data = db.ExecuteQuery<TableName>(sql);
                return new Tuple<string, List<TableName>>(db.Database, data);
            }
        }

        public static List<Column> GetColumns(string tableName)
        {
            using (var db = new DbConnection())
            {
                var sql = @"select 
                cast(ep.value as varchar) [Description],
                (case when  ty.[name] in ('text','ntext' ,'char','nchar', 'varchar', 'nvarchar') then 'string'
                when ty.[name] in ('date' , 'datetime' , 'datetime2','smalldatetime') then 'DateTime'
                when ty.[name] in ('bit') then 'bool'
                when ty.[name] in ('smallint') then 'short'
                when ty.[name] in ('bigint') then 'long'
                when ty.[name] in ('real') then 'float'
                when ty.[name] in ('float') then 'double'
                when ty.[name] in ('money') then 'decimal'
                when ty.[name] in ('uniqueidentifier') then 'Guid'
                else ty.[name] end) as TypeName,
                (case c.[is_nullable] when 1 then case when ty.[name] not in('text','ntext' ,'char','nchar', 'varchar', 'nvarchar') then '?' else '' end
                 else '' end) as NullString,c.name+' {set;get;}' as EndString from sys.tables t
                INNER JOIN sys.columns c ON t.object_id = c.object_id
                LEFT JOIN sys.extended_properties ep ON ep.major_id = c.object_id AND ep.minor_id = c.column_id 
                left JOIN sys.types ty on ty.[system_type_id]=c.[user_type_id] and ty.[name]!='sysname'
                WHERE ep.class =1 AND t.name='" + tableName + "' or c.object_id=Object_Id('" + tableName + "')";
                var data = db.ExecuteQuery<Column>(sql);
                return data;
            }
        }

    }
}
