using Microsoft.Extensions.DependencyModel;
using MySql.Data.MySqlClient;
using CATLGClassWcsService.Core;
using NPoco;
using NPoco.FluentMappings;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using Microsoft.Data.SqlClient;

namespace CATLGClassWcsService.Repository.Repositories
{
    internal class ScopeDBFactoryImpl : IScopeDBFactory, IAutoInject
    {
        protected static DatabaseFactory DbFactory;
        protected CustomDatabase Db;
        public CustomDatabase GetScopeDb()
        {
            if (DbFactory == null)
            {
                var fluentConfig = FluentMappingConfiguration.Scan(s =>
                {
                    //ViewObject
                    s.Assembly(Assembly.Load("CATLGClassWcsService.AppLayer"));
                    //Entity
                    // s.Assembly(Assembly.Load("CATLGClassWcsService.Entity"));

                    ///自动查找CATLGClassWcsService.XXX.Abstractions进行加载
                    var deps = DependencyContext.Default;
                    var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package" && lib.Name.StartsWith("CATLGClassWcsService") && lib.Name.EndsWith("Abstractions"));
                    foreach (var lib in libs)
                    {
                         s.Assembly(Assembly.Load(lib.Name));
                    }
                    s.WithSmartConventions();
                    //将字段名由AddUser改为add_user的方法
                    //s.Columns.Named(memberInfo => Inflector.AddUnderscores(memberInfo.Name)); 
                    //  s.Columns.IgnoreWhere(mi => ColumnInfo.FromMemberInfo(mi).IgnoreColumn);
                    //s.Columns.ResultWhere(mi => ColumnInfo.FromMemberInfo(mi).ResultColumn);
                    s.Columns.CanNullWhere(t =>
                    {
                        var canNullAttr = t.GetCustomAttribute<MyCanNullColumnAttribute>();
                        return canNullAttr != null ? true : false;
                    });
                    s.Columns.ComputedWhere(t =>
                    {
                        var computedAttr = t.GetCustomAttribute<MyComputedColumnAttribute>();
                        return computedAttr != null ? true : false;
                    });
                    s.Columns.ComputedTypeAs(t =>
                    {
                        var computedAttr = t.GetCustomAttribute<MyComputedColumnAttribute>();
                        if (computedAttr != null)
                        {
                            int computedColumnType = (int)computedAttr.ComputedColumnType;
                            return (ComputedColumnType)computedColumnType;
                        }
                        else
                        {
                            return ComputedColumnType.Always;
                        }
                    });
                    s.Columns.IgnoreWhere(t =>
                    {
                        var ignoreAttr = t.GetCustomAttribute<MyIgnoreAttribute>();
                        return ignoreAttr != null ? true : false;
                    });
                    s.Columns.VersionWhere(t =>
                    {
                        var versionAttr = t.GetCustomAttribute<MyVersionColumnAttribute>();
                        return versionAttr != null ? true : false;
                    });
                    s.Columns.VersionTypeAs(t =>
                    {
                        var versionAttr = t.GetCustomAttribute<MyVersionColumnAttribute>();
                        if (versionAttr != null)
                        {
                            int versionType = (int)versionAttr.VersionColumnType;
                            return (VersionColumnType)versionType;
                        }
                        else
                        {
                            return VersionColumnType.RowVersion;
                        }
                    });
                    s.Columns.ResultWhere(t =>
                    {
                        var resultAttr = t.GetCustomAttribute<MyResultColumnAttribute>();
                        return resultAttr != null ? true : false;
                    }
                    );
                    s.Columns.DbColumnTypeAs(mi =>
                    {
                        var columnType = mi.GetCustomAttribute<MyColumnTypeAttribute>();
                        return columnType == null ? mi.DeclaringType : columnType.Type;
                    });
                    s.TablesNamed(t =>
                    {
                        var pkAttr = t.GetCustomAttribute<MyTableNameAttribute>();
                        return pkAttr != null ? pkAttr.Value : Inflector.AddUnderscores(t.GetTypeInfo().Name);
                    });
                    s.PrimaryKeysNamed(t =>
                    {
                        var pkAttr = t.GetCustomAttribute<MyPrimaryKeyAttribute>();
                        return pkAttr != null ? pkAttr.Value : Inflector.AddUnderscores(t.GetTypeInfo().Name);
                    });
                    s.PrimaryKeysAutoIncremented(t =>
                    {
                        var pkAttr = t.GetCustomAttribute<MyPrimaryKeyAttribute>();
                        return pkAttr != null ? pkAttr.AutoIncrement : true;
                    });

                });

                DbFactory = DatabaseFactory.Config(x =>
                {
                    {
                        x.WithFluentConfig(fluentConfig);

                    }
                });
            }
            if (Db == null)
            {
                if (AppsettingsConfig.DatabaseType == "MYSQL")
                {
                    Db = new CustomDatabase(AppsettingsConfig.DefaultConnectionString, DatabaseType.MySQL, MySqlClientFactory.Instance);
                }

                else
                {
                    Db = new CustomDatabase(AppsettingsConfig.DefaultConnectionString, DatabaseType.SqlServer2012, SqlClientFactory.Instance);
                }

                DbFactory.Build(Db);
            }
            return Db;
        }



    }
}
