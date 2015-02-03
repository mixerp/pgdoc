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
using System.Text;
using MixERP.Net.Utilities.PgDoc.Helpers;
using MixERP.Net.Utilities.PgDoc.Models;

namespace MixERP.Net.Utilities.PgDoc.Parsers
{
    internal static class IndexParser
    {
        internal static string Parse(string content, List<string> matches, ICollection<PgIndex> indices)
        {
            foreach (string match in matches)
            {
                string comment = HtmlHelper.RemoveComment(match);

                if (!comment.StartsWith("Indices"))
                {
                    continue;
                }

                comment = comment.ReplaceFirst("Indices", "");

                StringBuilder items = new StringBuilder();

                foreach (PgIndex index in indices)
                {
                    string indicator = "<i class='grid layout icon' title='Index'></i>";

                    if (index.Type.StartsWith("P"))
                    {
                        indicator = "<i class='red key icon' title='Primary Key'></i>";
                    }

                    if (index.Type.StartsWith("U"))
                    {
                        indicator = "<i class='grid layout yellow icon' title='Unique Inde'></i>";
                    }


                    items.Append(comment.Replace("[Name]", index.Name)
                        .Replace("[IndexSchema]", index.SchemaName)
                        .Replace("[Type]", index.Type)
                        .Replace("[Indicator]", indicator)
                        .Replace("[Owner]", index.Owner)
                        .Replace("[Definition]", index.Definition)
                        .Replace("[AccessMethod]", index.AccessMethod)
                        .Replace("[IsClustered]", index.IsClustered.ToString())
                        .Replace("[IsValid]", index.IsValid.ToString())
                        .Replace("[Description]", index.Description));
                }

                content = content.Replace(match, items.ToString());
            }


            return content;
        }
    }
}