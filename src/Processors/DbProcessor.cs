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
    internal static class DbProcessor
    {
        internal static PGDatabase GetDatabase()
        {
            PGDatabase database = new PGDatabase();
            Collection<PGDatabase.PGDatabaseSetting> settings = new Collection<PGDatabase.PGDatabaseSetting>();

            string sql = FileHelper.ReadSqlResource("db.sql");

            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                using (DataTable table = DbOperation.GetDataTable(command))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            PGDatabase.PGDatabaseSetting setting = new PGDatabase.PGDatabaseSetting
                            {
                                Name = Conversion.TryCastString(row["name"]),
                                Setting = Conversion.TryCastString(row["setting"]),
                                Description = Conversion.TryCastString(row["description"])
                            };


                            settings.Add(setting);
                        }

                        database.Comment = GetDatabaseComment();
                        database.Settings = settings;
                    }
                }
            }

            return database;
        }

        private static string GetDatabaseComment()
        {
            const string sql =
                "SELECT description FROM pg_shdescription INNER JOIN pg_database ON objoid = pg_database.oid WHERE datname = current_database();";
            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                return Conversion.TryCastString(DbOperation.GetScalarValue(command));
            }
        }
    }
}