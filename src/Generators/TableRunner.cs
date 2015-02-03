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
using System.Collections.Generic;
using MixERP.Net.Utilities.PgDoc.Helpers;
using MixERP.Net.Utilities.PgDoc.Models;
using MixERP.Net.Utilities.PgDoc.Processors;

namespace MixERP.Net.Utilities.PgDoc.Generators
{
    internal static class TableRunner
    {
        private static readonly string OutputPath = ConfigurationHelper.GetTableOutputDirectory();
        private static readonly string TemplatePath = ConfigurationHelper.GetTableTemplatePath();

        internal static void Run(PgTable table)
        {
            string content = FileHelper.ReadResource(TemplatePath);
            List<string> matches = HtmlHelper.GetMatch(content);

            if (matches == null)
            {
                return;
            }

            BuildDocumentation(content, matches, table);
        }

        private static void BuildDocumentation(string content, List<string> matches, PgTable table)
        {
            table.Columns = ColumnProcessor.GetColumns(table);
            table.Triggers = TableTriggerProcessor.GetTriggers(table);
            table.Indices = IndexProcessor.GetIndices(table);
            table.CheckConstraints = CheckConstraintProcessor.GetCheckConstraints(table);

            content = content.Replace("[DBName]", Program.Database.ToUpperInvariant());
            content = content.Replace("[SchemaName]", table.SchemaName);
            content = content.Replace("[TableName]", table.Name);
            content = content.Replace("[TableComment]", table.Description);

            content = Parsers.ColumnParser.Parse(content, matches, table.Columns);
            content = Parsers.ForeignKeyParser.Parse(content, matches, table.Columns);
            content = Parsers.IndexParser.Parse(content, matches, table.Indices);
            content = Parsers.CheckConstraintParser.Parse(content, matches, table.CheckConstraints);
            content = Parsers.DefaultParser.Parse(content, matches, table.Columns);
            content = Parsers.TriggerParser.Parse(content, matches, table.Triggers);


            string targetPath = System.IO.Path.Combine (OutputPath, table.SchemaName, table.Name + ".html");
            FileHelper.WriteFile(content, targetPath);
        }
    }
}