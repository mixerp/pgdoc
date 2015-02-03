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
using MixERP.Net.Utilities.PgDoc.Helpers;
using MixERP.Net.Utilities.PgDoc.Models;

namespace MixERP.Net.Utilities.PgDoc.Generators
{
    internal static class MaterializedViewRunner
    {
        private static readonly string OutputPath = ConfigurationHelper.GetMaterializedViewOutputDirectory();
        private static readonly string TemplatePath = ConfigurationHelper.GetMaterializedViewTemplatePath();

        internal static void Run(PgMaterializedView view)
        {
            string content = FileHelper.ReadResource(TemplatePath);

            BuildDocumentation(content, view);
        }

        private static void BuildDocumentation(string content, PgMaterializedView view)
        {
            content = content.Replace("[DBName]", Program.Database.ToUpperInvariant());

            content = Parsers.MaterializedViewParser.Parse(content, view);

            string targetPath = System.IO.Path.Combine (OutputPath, view.SchemaName, view.Name + ".html");
            FileHelper.WriteFile(content, targetPath);
        }
    }
}