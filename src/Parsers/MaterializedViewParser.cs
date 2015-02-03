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
    internal static class MaterializedViewParser
    {
        internal static string Parse(string content, List<string> matches, ICollection<PgMaterializedView> matViews)
        {
            foreach (string match in matches)
            {
                string comment = HtmlHelper.RemoveComment(match);

                if (!comment.StartsWith("MaterializedViews"))
                {
                    continue;
                }

                comment = comment.Replace("MaterializedViews", "");

                string items = "";
                items = matViews.Aggregate(items, (current, matView) => current + Parse(comment, matView));

                content = content.Replace(match, items);
            }

            return content;
        }

        internal static string Parse(string content, PgMaterializedView matView)
        {
            StringBuilder items = new StringBuilder();
            items.Append(content.Replace("[Name]", matView.Name)
                .Replace("[ViewSchema]", matView.SchemaName)
                .Replace("[RowNumber]", matView.RowNumber.ToString())
                .Replace("[Owner]", matView.Owner)
                .Replace("[Tablespace]", matView.Tablespace)
                .Replace("[Definition]", matView.Definition)
                .Replace("[Description]", matView.Description));
            content = content.Replace(content, items.ToString());

            return content;
        }
    }
}