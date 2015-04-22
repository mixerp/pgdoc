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

using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using Npgsql;

namespace MixERP.Net.Utilities.PgDoc.DBFactory
{
    internal class DbOperation
    {
        internal static bool ExecuteNonQuery(NpgsqlCommand command)
        {
            if (command != null)
            {
                if (ValidateCommand(command))
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(DbConnection.ConnectionString()))
                    {
                        command.Connection = connection;
                        connection.Open();

                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }

            return false;
        }

        internal static NpgsqlDataAdapter GetDataAdapter(NpgsqlCommand command)
        {
            if (command != null)
            {
                if (ValidateCommand(command))
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(DbConnection.ConnectionString()))
                    {
                        command.Connection = connection;

                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                        {
                            return adapter;
                        }
                    }
                }
            }

            return null;
        }

        internal static NpgsqlDataReader GetDataReader(NpgsqlCommand command)
        {
            if (command != null)
            {
                if (ValidateCommand(command))
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(DbConnection.ConnectionString()))
                    {
                        command.Connection = connection;
                        command.Connection.Open();
                        return command.ExecuteReader();
                    }
                }
            }

            return null;
        }

        internal static DataSet GetDataSet(NpgsqlCommand command)
        {
            if (ValidateCommand(command))
            {
                using (NpgsqlDataAdapter adapter = GetDataAdapter(command))
                {
                    using (DataSet set = new DataSet())
                    {
                        adapter.Fill(set);
                        set.Locale = CultureInfo.CurrentUICulture;
                        return set;
                    }
                }
            }

            return null;
        }

        internal static DataTable GetDataTable(NpgsqlCommand command, string connectionString)
        {
            if (command != null)
            {
                if (ValidateCommand(command))
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        command.Connection = connection;

                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                        {
                            using (DataTable dataTable = new DataTable())
                            {
                                dataTable.Locale = Thread.CurrentThread.CurrentCulture;
                                adapter.Fill(dataTable);
                                return dataTable;
                            }
                        }
                    }
                }
            }

            return null;
        }

        internal static DataTable GetDataTable(NpgsqlCommand command)
        {
            return GetDataTable(command, DbConnection.ConnectionString());
        }

        internal static DataView GetDataView(NpgsqlCommand command)
        {
            if (ValidateCommand(command))
            {
                using (DataView view = new DataView(GetDataTable(command)))
                {
                    return view;
                }
            }

            return null;
        }

        internal static object GetScalarValue(NpgsqlCommand command)
        {
            if (command != null)
            {
                if (ValidateCommand(command))
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(DbConnection.ConnectionString()))
                    {
                        command.Connection = connection;
                        connection.Open();
                        return command.ExecuteScalar();
                    }
                }
            }

            return null;
        }

        internal static bool IsServerAvailable()
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(DbConnection.ConnectionString()))
                {
                    connection.Open();
                }

                return true;
            }
            catch (NpgsqlException)
            {
                //swallow exception
            }

            return false;
        }


        private static Collection<string> GetCommandTextParameterCollection(string commandText)
        {
            Collection<string> parameters = new Collection<string>();

            foreach (Match match in Regex.Matches(commandText, @"@(\w+)"))
            {
                parameters.Add(match.Value);
            }

            return parameters;
        }

        private static bool ValidateCommand(NpgsqlCommand command)
        {
            return ValidateParameters(command);
        }

        private static bool ValidateParameters(NpgsqlCommand command)
        {
            Collection<string> commandTextParameters = GetCommandTextParameterCollection(command.CommandText);

            foreach (NpgsqlParameter npgsqlParameter in command.Parameters)
            {
                bool match = false;

                foreach (string commandTextParameter in commandTextParameters)
                {
                    if (npgsqlParameter.ParameterName.Equals(commandTextParameter))
                    {
                        match = true;
                    }
                }

                if (!match)
                {
                    throw new InvalidOperationException(string.Format("Invalid parameter name '{0}'.", npgsqlParameter.ParameterName));
                }
            }

            return true;
        }

    }
}