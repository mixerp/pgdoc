/********************************************************************************
Copyright (C) Binod Nepal, Mix Open Foundation (http://mixof.org).

This file is part of MixERP.

MixERP is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MixERP is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MixERP.  If not, see <http://www.gnu.org/licenses/>.
***********************************************************************************/
using System.Collections.ObjectModel;
using System.Data;
using MixERP.Net.Utilities.PgDoc.DBFactory;
using MixERP.Net.Utilities.PgDoc.Helpers;
using MixERP.Net.Utilities.PgDoc.Models;
using Npgsql;

namespace MixERP.Net.Utilities.PgDoc.Processors
{
    internal static class TypeProcessor
    {
        internal static void AddPgTypeDefinitionFunction()
        {
            string sql = FileHelper.ReadSqlResource("pg_catalog.pg_get_typedef.sql");

            DbOperation.ExecuteNonQuery(new NpgsqlCommand(sql));
        }

        internal static Collection<PgType> GetTypes(string schemaName)
        {
            string sql = FileHelper.ReadSqlResource("types-by-schema.sql");

            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                command.Parameters.AddWithValue("@SchemaName", schemaName);

                return GetTypes(DbOperation.GetDataTable(command));
            }
        }

        internal static Collection<PgType> GetTypes()
        {
            string sql = FileHelper.ReadSqlResource("types.sql");

            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                return GetTypes(DbOperation.GetDataTable(command));
            }
        }

        private static Collection<PgType> GetTypes(DataTable table)
        {
            Collection<PgType> types = new Collection<PgType>();

            if (table.Rows.Count.Equals(0))
            {
                return types;
            }

            foreach (DataRow row in table.Rows)
            {
                PgType type = new PgType
                {
                    RowNumber = Conversion.TryCastLong(row["row_number"]),
                    SchemaName = Conversion.TryCastString(row["schema_name"]),
                    Name = Conversion.TryCastString(row["type_name"]),
                    BaseType = Conversion.TryCastString(row["base_type"]),
                    Owner = Conversion.TryCastString(row["owner"]),
                    Collation = Conversion.TryCastString(row["collation"]),
                    Default = Conversion.TryCastString(row["default"]),
                    Type = Conversion.TryCastString(row["type"]),
                    StoreType = Conversion.TryCastString(row["store_type"]),
                    NotNull = Conversion.TryCastBoolean(row["not_null"]),
                    Definition = Conversion.TryCastString(row["definition"]),
                    Description = Conversion.TryCastString(row["description"])
                };

                types.Add(type);
            }

            return types;
        }
    }
}