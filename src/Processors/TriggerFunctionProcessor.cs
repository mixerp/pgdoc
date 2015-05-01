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
    internal static class TriggerFunctionProcessor
    {
		internal static Collection<PgFunction> GetTriggerFunctions(string schemaPattern = ".*", string xSchemaPattern = "")
        {
            string sql = FileHelper.ReadSqlResource("trigger-functions-by-schema.sql");

            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
				command.Parameters.AddWithValue("@SchemaPattern", schemaPattern);
				command.Parameters.AddWithValue("@xSchemaPattern", xSchemaPattern);
				return FunctionProcessor.GetFunctions(DbOperation.GetDataTable(command));
            }
        }
    }
}