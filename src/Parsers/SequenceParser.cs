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
    internal static class SequenceParser
    {
        internal static string Parse(string content, List<string> matches, ICollection<PgSequence> sequences)
        {
            foreach (string match in matches)
            {
                string comment = HtmlHelper.RemoveComment(match);

                if (!comment.StartsWith("Sequences"))
                {
                    continue;
                }
                
                comment = comment.ReplaceFirst("Sequences", "");

                StringBuilder items = new StringBuilder();

                foreach (PgSequence sequence in sequences)
                {
                    items.Append(comment.Replace("[Name]", sequence.Name)
                        .Replace("[RowNumber]", sequence.RowNumber.ToString())
                        .Replace("[SequenceSchema]", sequence.SchemaName)
                        .Replace("[Owner]", sequence.Owner)
                        .Replace("[DataType]", sequence.DataType)
                        .Replace("[StartValue]", sequence.StartValue)
                        .Replace("[Increment]", sequence.Increment)
                        .Replace("[MinimumValue]", sequence.MinimumValue)
                        .Replace("[MaximumValue]", sequence.MaximumValue)
                        .Replace("[Description]", sequence.Description));
                }

                content = content.Replace(match, items.ToString());
            }


            return content;
        }
    }
}