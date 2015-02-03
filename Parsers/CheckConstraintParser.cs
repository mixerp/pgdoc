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
    internal static class CheckConstraintParser
    {
        internal static string Parse(string content, IEnumerable<string> matches,
            ICollection<PgCheckConstraint> constraints)
        {
            foreach (string match in matches)
            {
                string comment = HtmlHelper.RemoveComment(match);

                if (!comment.StartsWith("CheckConstraints")) continue;
                comment = comment.ReplaceFirst("CheckConstraints", "");

                StringBuilder items = new StringBuilder();

                foreach (PgCheckConstraint constraint in constraints)
                {
                    items.Append(comment.Replace("[Name]", constraint.Name)
                        .Replace("[ConstraintSchema]", constraint.SchemaName)
                        .Replace("[ConstraintTable]", constraint.TableName)
                        .Replace("[Definition]", constraint.Definition)
                        .Replace("[Deferrable]", constraint.Deferrable.ToString())
                        .Replace("[Deferred]", constraint.Deferred.ToString())
                        .Replace("[IsLocal]", constraint.IsLocal.ToString())
                        .Replace("[NoInherit]", constraint.NoInherit.ToString())
                        .Replace("[Description]", constraint.Description));
                }

                content = content.Replace(match, items.ToString());
            }
            return content;
        }
    }
}