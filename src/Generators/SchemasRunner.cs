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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MixERP.Net.Utilities.PgDoc.Helpers;
using MixERP.Net.Utilities.PgDoc.Models;
using MixERP.Net.Utilities.PgDoc.Processors;

namespace MixERP.Net.Utilities.PgDoc.Generators
{
    internal static class SchemasRunner
    {
        private static readonly string OutputPath = ConfigurationHelper.GetSchemasOutputPath();
        private static readonly string TemplatePath = ConfigurationHelper.GetSchemasTemplatePath();

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
            Collection<PGSchema> schemas = SchemaProcessor.GetSchemas(Program.SchemaPattern, Program.xSchemaPattern);

            content = content.Replace("[DBName]", Program.Database.ToUpperInvariant());
            content = Parsers.SchemaParser.Parse(content, matches, schemas);

            FileHelper.WriteFile(content, OutputPath);

            Console.WriteLine("Writing tables.");
            TablesRunner.Run();

            Console.WriteLine("Writing triggers.");
            TriggersRunner.Run();

            Console.WriteLine("Writing views.");
            ViewsRunner.Run();

            Console.WriteLine("Writing materialized views.");
            MaterializedViewsRunner.Run();

            Console.WriteLine("Writing sequences.");
            SequencesRunner.Run();

            Console.WriteLine("Writing functions.");
            FunctionsRunner.Run();

            Console.WriteLine("Writing types.");
            TypesRunner.Run();
        }
    }
}