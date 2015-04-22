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
    internal static class SequenceProcessor
    {
        internal static Collection<PgSequence> GetSequences()
        {
            string sql = FileHelper.ReadSqlResource("sequences.sql");

            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                return GetSequences(DbOperation.GetDataTable(command));
            }
        }

        internal static Collection<PgSequence> GetSequences(string schemaName)
        {
            string sql = FileHelper.ReadSqlResource("sequences-by-schema.sql");

            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                command.Parameters.AddWithValue("@SchemaName", schemaName);
                return GetSequences(DbOperation.GetDataTable(command));
            }
        }

        private static Collection<PgSequence> GetSequences(DataTable table)
        {
            Collection<PgSequence> sequences = new Collection<PgSequence>();
            if (table == null || table.Rows == null || table.Rows.Count.Equals(0))
            {
                return sequences;
            }

            foreach (DataRow row in table.Rows)
            {
                PgSequence sequence = new PgSequence
                {
                    RowNumber = Conversion.TryCastInteger(row["row_number"]),
                    SchemaName = Conversion.TryCastString(row["sequence_schema"]),
                    Name = Conversion.TryCastString(row["sequence_name"]),
                    DataType = Conversion.TryCastString(row["data_type"]),
                    Increment = Conversion.TryCastString(row["increment"]),
                    Description = Conversion.TryCastString(row["description"]),
                    Owner = Conversion.TryCastString(row["owner"]),
                    StartValue = Conversion.TryCastString(row["start_value"]),
                    MinimumValue = Conversion.TryCastString(row["minimum_value"]),
                    MaximumValue = Conversion.TryCastString(row["maximum_value"])
                };

                sequences.Add(sequence);
            }

            return sequences;
        }
    }
}