using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DbToEntity.Entity;
using System.Configuration;

namespace DbToEntity.Core
{
    public class FileHelper
    {
        static string path = AppDomain.CurrentDomain.BaseDirectory + "/Results/";
        static string nspace = ConfigurationManager.AppSettings["namespace"] ?? "DbToEntity";
        static string author = ConfigurationManager.AppSettings["author"] ?? "DbToEntity";
        static FileHelper()
        {
            path = path + "DataBase-" + Program.DbName + "/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static void CreateFile(string tableName, List<Column> cols, string dbName)
        {
            var fileString = GetFileString(tableName, cols);
            var fileName = path + tableName + ".cs";
            File.Delete(fileName);
            using (var sw = new StreamWriter(fileName, true))
            {
                sw.Write(fileString);
                sw.Flush();
            }
        }

        public static string GetFileString(string tableName, List<Column> cols)
        {
            var sb = new StringBuilder();

            sb.AppendLine("/*******************************************************");
            sb.AppendLine("*创建作者： " + author);
            sb.AppendLine("*类的名称： " + tableName);
            sb.AppendLine("*命名空间： " + nspace);
            sb.AppendLine("*创建时间： " + DateTime.Now.ToShortDateString());
            sb.AppendLine("*工具说明： 生成工具由Jerry开发支持！");
            sb.AppendLine("*开源地址： https://github.com/decadestory/DbToEntity");
            sb.AppendLine("********************************************************/");

            sb.AppendLine("namespace " + nspace);
            sb.AppendLine("{");
            sb.AppendLine("\t public class " + tableName);
            sb.AppendLine("\t {");

            foreach (var item in cols)
            {
                if (!string.IsNullOrEmpty(item.Description))
                {
                    sb.AppendLine("\t\t /// <summary>");
                    sb.AppendLine("\t\t /// " + item.Description);
                    sb.AppendLine("\t\t /// <summary>");
                }

                sb.AppendLine("\t\t public " + item.TypeName + item.NullString + " " + item.EndString);
            }

            sb.AppendLine("\t }");
            sb.AppendLine("}");

            return sb.ToString();
        }


    }
}