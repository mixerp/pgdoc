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
    internal static class TriggerFunctionProcessor
    {
        internal static Collection<PgFunction> GetTriggerFunctions(string schemaName)
        {
            string sql = FileHelper.ReadSqlResource("trigger-functions-by-schema.sql");

            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                command.Parameters.AddWithValue("@SchemaName", schemaName);
                return FunctionProcessor.GetFunctions(DbOperation.GetDataTable(command));
            }
        }

        internal static Collection<PgFunction> GetTriggerFunctions()
        {
            Collection<PgFunction> triggers = new Collection<PgFunction>();

            string sql = FileHelper.ReadSqlResource("trigger-functions.sql");

            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                using (DataTable table = DbOperation.GetDataTable(command))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            PgFunction triggerFunction = new PgFunction
                            {
                                RowNumber = Conversion.TryCastLong(row["row_number"]),
                                FunctionOid = Conversion.TryCastString(row["oid"]),
                                Name = Conversion.TryCastString(row["function_name"]),
                                SchemaName = Conversion.TryCastString(row["object_schema"]),
                                Arguments = Conversion.TryCastString(row["arguments"]),
                                FunctionType = Conversion.TryCastString(row["function_type"]),
                                Owner = Conversion.TryCastString(row["owner"]),
                                Description = Conversion.TryCastString(row["description"])
                            };


                            triggers.Add(triggerFunction);
                        }
                    }
                }
            }

            return triggers;
        }
    }
}