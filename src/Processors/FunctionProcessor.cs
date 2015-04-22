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
    internal static class FunctionProcessor
    {
        internal static Collection<PgFunction> GetFunctions(string schemaName)
        {
            string sql = FileHelper.ReadSqlResource("functions-by-schema.sql");

            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                command.Parameters.AddWithValue("@SchemaName", schemaName);
                return GetFunctions(DbOperation.GetDataTable(command));
            }
        }

        internal static Collection<PgFunction> GetFunctions()
        {
            string sql = FileHelper.ReadSqlResource("functions.sql");

            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                return GetFunctions(DbOperation.GetDataTable(command));
            }
        }

        internal static Collection<PgFunction> GetFunctions(DataTable table)
        {
            Collection<PgFunction> procedures = new Collection<PgFunction>();

            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    PgFunction function = new PgFunction
                    {
                        FunctionOid = Conversion.TryCastString(row["oid"]),
                        FunctionDefinition = Conversion.TryCastString(row["definition"]),
                        RowNumber = Conversion.TryCastLong(row["row_number"]),
                        Name = Conversion.TryCastString(row["function_name"]),
                        SchemaName = Conversion.TryCastString(row["object_schema"]),
                        Arguments = Conversion.TryCastString(row["arguments"]),
                        ResultType = Conversion.TryCastString(row["result_type"]),
                        FunctionType = Conversion.TryCastString(row["function_type"]),
                        Owner = Conversion.TryCastString(row["owner"]),
                        Description = Conversion.TryCastString(row["description"])
                    };


                    procedures.Add(function);
                }
            }

            return procedures;
        }
    }
}