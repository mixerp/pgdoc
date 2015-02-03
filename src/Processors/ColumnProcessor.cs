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
using MixERP.Net.Common;
using MixERP.Net.Utilities.PgDoc.DBFactory;
using MixERP.Net.Utilities.PgDoc.Helpers;
using MixERP.Net.Utilities.PgDoc.Models;
using Npgsql;

namespace MixERP.Net.Utilities.PgDoc.Processors
{
    internal static class ColumnProcessor
    {
        internal static Collection<PgColumn> GetColumns(PgTable pgTable)
        {
            Collection<PgColumn> columns = new Collection<PgColumn>();

            #region sql

            string sql = FileHelper.ReadSqlResource("columns.sql");

            #endregion

            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                command.Parameters.AddWithValue("@SchemaName", pgTable.SchemaName);
                command.Parameters.AddWithValue("@TableName", pgTable.Name);

                using (DataTable table = DbOperation.GetDataTable(command))
                {
                    if (table.Rows.Count.Equals(0))
                    {
                        return columns;
                    }

                    foreach (DataRow row in table.Rows)
                    {
                        PgColumn column = new PgColumn
                        {
                            SchemaName = pgTable.SchemaName,
                            TableName = pgTable.Name,
                            Name = Conversion.TryCastString(row["column_name"]),
                            OrdinalPosition = Conversion.TryCastInteger(row["ordinal_position"]),
                            DefaultValue = Conversion.TryCastString(row["column_default"]),
                            DataType = Conversion.TryCastString(row["data_type"]),
                            IsNullable = Conversion.TryCastBoolean(row["is_nullable"]),
                            MaxLength = Conversion.TryCastInteger(row["character_maximum_length"]),
                            Description = Conversion.TryCastString(row["description"]),
                            PrimaryKeyConstraintName = Conversion.TryCastString(row["key"]),
                            ForiegnKeyName = Conversion.TryCastString(row["constraint_name"]),
                            ForeignSchemaName = Conversion.TryCastString(row["references_schema"]),
                            ForeignTableName = Conversion.TryCastString(row["references_table"]),
                            ForeignColumnName = Conversion.TryCastString(row["references_field"])
                        };

                        column.IsPrimaryKey = !string.IsNullOrWhiteSpace(column.PrimaryKeyConstraintName);

                        columns.Add(column);
                    }
                }
            }

            return columns;
        }
    }
}