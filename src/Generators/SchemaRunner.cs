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
using MixERP.Net.Utilities.PgDoc.Helpers;
using MixERP.Net.Utilities.PgDoc.Models;
using MixERP.Net.Utilities.PgDoc.Parsers;
using MixERP.Net.Utilities.PgDoc.Processors;

namespace MixERP.Net.Utilities.PgDoc.Generators
{
    internal static class SchemaRunner
    {
        private static readonly string OutputPath = ConfigurationHelper.GetSchemaOutputDirectory();
        private static readonly string TemplatePath = ConfigurationHelper.GetSchemaTemplatePath();

        internal static void Run(string schemaName)
        {
            string content = FileHelper.ReadResource(TemplatePath);
            List<string> matches = HtmlHelper.GetMatch(content);

            if (matches == null)
            {
                return;
            }

            BuildDocumentation(content, matches, schemaName);
        }

        private static void BuildDocumentation(string content, List<string> matches, string schemaName)
        {
            PGSchema schema = SchemaProcessor.GetSchema(schemaName);

            content = content.Replace("[DBName]", Program.Database.ToUpperInvariant());
            content = content.Replace("[SchemaName]", schemaName);

            content = SequenceParser.Parse(content, matches, SequenceProcessor.GetSequences(schemaName));
            content = TableParser.Parse(content, matches, schema.Tables);
            content = ViewParser.Parse(content, matches, schema.Views);
            content = SequenceParser.Parse(content, matches, schema.Sequences);
            content = MaterializedViewParser.Parse(content, matches, schema.MaterializedViews);
            content = FunctionParser.Parse(content, matches, schema.Functions);
            content = FunctionParser.ParseTriggers(content, matches, schema.TriggerFunctions);
            content = TypeParser.Parse(content, matches, schema.Types);

            foreach (PgTable table in schema.Tables)
            {
                Console.WriteLine("Generating documentation for table \"{0}\".", table.Name);
                TableRunner.Run(table);
            }


            foreach (PgFunction function in schema.Functions)
            {
                Console.WriteLine("Generating documentation for function \"{0}\".", function.Name);
                FunctionRunner.Run(function);
            }

            foreach (PgFunction function in schema.TriggerFunctions)
            {
                Console.WriteLine("Generating documentation for trigger function \"{0}\".", function.Name);
                FunctionRunner.Run(function);
            }

            foreach (PgMaterializedView materializedView in schema.MaterializedViews)
            {
                Console.WriteLine("Generating documentation for materialized view \"{0}\".", materializedView.Name);
                MaterializedViewRunner.Run(materializedView);
            }

            foreach (PgView view in schema.Views)
            {
                Console.WriteLine("Generating documentation for view \"{0}\".", view.Name);
                ViewRunner.Run(view);
            }
            foreach (PgType type in schema.Types)
            {
                Console.WriteLine("Generating documentation for type \"{0}\".", type.Name);
                TypeRunner.Run(type);
            }

            string targetPath = System.IO.Path.Combine(OutputPath, schemaName + ".html");
            FileHelper.WriteFile(content, targetPath);
        }
    }
}