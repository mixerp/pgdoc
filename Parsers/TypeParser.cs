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
    internal static class TypeParser
    {
        internal static string Parse(string content, List<string> matches, ICollection<PgType> types)
        {
            foreach (string match in matches)
            {
                string comment = HtmlHelper.RemoveComment(match);

                if (!comment.StartsWith("Types"))
                {
                    continue;
                }
                
                comment = comment.Replace("Types", "");
                string items = "";
                items = types.Aggregate(items, (current, type) => current + Parse(comment, type));

                content = content.Replace(match, items);
            }
            return content;
        }

        internal static string Parse(string content, PgType type)
        {
            StringBuilder items = new StringBuilder();
            items.Append(content.Replace("[Name]", type.Name)
                .Replace("[RowNumber]", type.RowNumber.ToString())
                .Replace("[TypeSchema]", type.SchemaName)
                .Replace("[BaseType]", type.BaseType)
                .Replace("[Owner]", type.Owner)
                .Replace("[Collation]", type.Collation)
                .Replace("[Default]", type.Default)
                .Replace("[Type]", type.Type)
                .Replace("[StoreType]", type.StoreType)
                .Replace("[Definition]", type.Definition)
                .Replace("[NotNull]", type.NotNull.ToString())
                .Replace("[Description]", type.Description));

            content = content.Replace(content, items.ToString());
            return content;
        }
    }
}