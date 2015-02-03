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
using System.Linq;
using System.Text;
using MixERP.Net.Utilities.PgDoc.Helpers;
using MixERP.Net.Utilities.PgDoc.Models;

namespace MixERP.Net.Utilities.PgDoc.Parsers
{
    internal static class FunctionParser
    {
        internal static string Parse(string content, List<string> matches, ICollection<PgFunction> functions)
        {
            foreach (string match in matches)
            {
                string comment = HtmlHelper.RemoveComment(match);

                string items = "";

                if (!comment.StartsWith("Functions")) continue;

                comment = comment.Replace("Functions", "");

                items = functions.Aggregate(items, (current, procedure) => current + Parse(comment, procedure));

                content = content.Replace(match, items);
            }

            return content;
        }

        internal static string ParseTriggers(string content, List<string> matches, ICollection<PgFunction> functions)
        {
            foreach (string match in matches)
            {
                string comment = HtmlHelper.RemoveComment(match);

                string items = "";

                if (!comment.StartsWith("TriggerFunctions"))
                {
                    continue;
                }

                comment = comment.Replace("TriggerFunctions", "");

                items = functions.Aggregate(items, (current, procedure) => current + Parse(comment, procedure));

                content = content.Replace(match, items);
            }

            return content;
        }

        internal static string Parse(string content, PgFunction procedure)
        {
            StringBuilder items = new StringBuilder();

            items.Append(content.Replace("[Name]", procedure.Name)
                .Replace("[TriggerSchema]", procedure.SchemaName)
                .Replace("[FunctionSchema]", procedure.SchemaName)
                .Replace("[Arguments]", procedure.Arguments)
                .Replace("[RowNumber]", procedure.RowNumber.ToString())
                .Replace("[Owner]", procedure.Owner)
                .Replace("[ResultType]", procedure.ResultType)
                .Replace("[FunctionType]", procedure.FunctionType)
                .Replace("[FunctionOid]", procedure.FunctionOid)
                .Replace("[Definition]", procedure.FunctionDefinition)
                .Replace("[Description]", procedure.Description));

            content = content.Replace(content, items.ToString());

            return content;
        }
    }
}