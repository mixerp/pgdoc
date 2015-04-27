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
    internal static class TypesRunner
    {
        private static readonly string OutputPath = ConfigurationHelper.GetTypesOutputPath();
        private static readonly string TemplatePath = ConfigurationHelper.GetTypesTemplatePath();

        internal static void Run()
        {
            string content = FileHelper.ReadResource(TemplatePath);
            List<string> matches = HtmlHelper.GetMatch(content);

            if (matches == null)
            {
                return;
            }

            BuildDocumentation(content, matches);
        }

        private static void BuildDocumentation(string content, List<string> matches)
        {
			ICollection<PgType> types = TypeProcessor.GetTypes(Program.SchemaPattern);

            content = content.Replace("[DBName]", Program.Database.ToUpperInvariant());

            content = Parsers.TypeParser.Parse(content, matches, types);

            FileHelper.WriteFile(content, OutputPath);
        }
    }
}