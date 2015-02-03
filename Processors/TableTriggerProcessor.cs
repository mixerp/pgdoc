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
    internal static class TableTriggerProcessor
    {
        internal static Collection<PgTableTrigger> GetTriggers(PgTable pgTable)
        {
            Collection<PgTableTrigger> triggers = new Collection<PgTableTrigger>();

            string sql = FileHelper.ReadSqlResource("table-triggers.sql");

            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                command.Parameters.AddWithValue("@SchemaName", pgTable.SchemaName);
                command.Parameters.AddWithValue("@TableName", pgTable.Name);
                using (DataTable table = DbOperation.GetDataTable(command))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            PgTableTrigger trigger = new PgTableTrigger
                            {
                                SchemaName = Conversion.TryCastString(row["trigger_schema"]),
                                Name = Conversion.TryCastString(row["trigger_name"]),
                                TargetTableSchema = pgTable.SchemaName,
                                TargetTableName = pgTable.Name,
                                EventName = Conversion.TryCastString(row["event_manipulation"]),
                                Timing = Conversion.TryCastString(row["action_timing"]),
                                Condition = Conversion.TryCastString(row["action_condition"]),
                                Order = Conversion.TryCastInteger(row["action_order"]),
                                Orientation = Conversion.TryCastString(row["action_orientation"]),
                                TargetFunctionSchema = Conversion.TryCastString(row["target_function_schema"]),
                                TargetFunctionName = Conversion.TryCastString(row["target_function_name"]),
                                TargetFunctionOid = Conversion.TryCastString(row["target_function_oid"]),
                                Description = Conversion.TryCastString(row["description"]),
                            };


                            triggers.Add(trigger);
                        }
                    }
                }
            }

            return triggers;
        }
    }
}