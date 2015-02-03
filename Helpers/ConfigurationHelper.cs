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
using System.Configuration;

namespace MixERP.Net.Utilities.PgDoc.Helpers
{
    internal static class ConfigurationHelper
    {
        internal static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        internal static string GetDatabaseOutputPath()
        {
            return "index.html";
        }

        internal static string GetDatabaseTemplatePath()
        {
            return "MixERP.Net.Utilities.PgDoc.Configs.Template.index.html";
        }

        internal static string GetFunctionOutputDirectory()
        {
            return "functions";
        }

        internal static string GetFunctionsOutputPath()
        {
            return "functions.html";
        }

        internal static string GetFunctionsTemplatePath()
        {
            return "MixERP.Net.Utilities.PgDoc.Configs.Template.functions.html";
        }

        internal static string GetFunctionTemplatePath()
        {
            return "MixERP.Net.Utilities.PgDoc.Configs.Template.function.html";
        }

        internal static string GetMaterializedViewOutputDirectory()
        {
            return "materialized-views";
        }

        internal static string GetMaterializedViewsOutputPath()
        {
            return "materialized-views.html";
        }

        internal static string GetMaterializedViewsTemplatePath()
        {
            return "MixERP.Net.Utilities.PgDoc.Configs.Template.materialized-views.html";
        }

        internal static string GetMaterializedViewTemplatePath()
        {
            return "MixERP.Net.Utilities.PgDoc.Configs.Template.materialized-view.html";
        }

        internal static string GetSchemaOutputDirectory()
        {
            return "schemas";
        }

        internal static string GetSchemasOutputPath()
        {
            return "schemas.html";
        }

        internal static string GetSchemasTemplatePath()
        {
            return "MixERP.Net.Utilities.PgDoc.Configs.Template.schemas.html";
        }

        internal static string GetSchemaTemplatePath()
        {
            return "MixERP.Net.Utilities.PgDoc.Configs.Template.schema.html";
        }

        internal static string GetSequencesOutputPath()
        {
            return "sequences.html";
        }

        internal static string GetSequencesTemplatePath()
        {
            return "MixERP.Net.Utilities.PgDoc.Configs.Template.sequences.html";
        }

        internal static string GetTableOutputDirectory()
        {
            return "tables";
        }

        internal static string GetTablesOutputPath()
        {
            return "tables.html";
        }

        internal static string GetTablesTemplatePath()
        {
            return "MixERP.Net.Utilities.PgDoc.Configs.Template.tables.html";
        }

        internal static string GetTableTemplatePath()
        {
            return "MixERP.Net.Utilities.PgDoc.Configs.Template.table.html";
        }

        internal static string GetTriggersOutputPath()
        {
            return "triggers.html";
        }

        internal static string GetTriggersTemplatePath()
        {
            return "MixERP.Net.Utilities.PgDoc.Configs.Template.triggers.html";
        }

        internal static string GetTypeOutputDirectory()
        {
            return "types";
        }

        internal static string GetTypesOutputPath()
        {
            return "types.html";
        }

        internal static string GetTypesTemplatePath()
        {
            return "MixERP.Net.Utilities.PgDoc.Configs.Template.types.html";
        }

        internal static string GetTypeTemplatePath()
        {
            return "MixERP.Net.Utilities.PgDoc.Configs.Template.type.html";
        }

        internal static string GetViewOutputDirectory()
        {
            return "views";
        }

        internal static string GetViewsOutputPath()
        {
            return "views.html";
        }

        internal static string GetViewsTemplatePath()
        {
            return "MixERP.Net.Utilities.PgDoc.Configs.Template.views.html";
        }

        internal static string GetViewTemplatePath()
        {
            return "MixERP.Net.Utilities.PgDoc.Configs.Template.view.html";
        }
    }
}