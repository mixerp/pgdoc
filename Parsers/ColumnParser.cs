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
using System.Text;
using MixERP.Net.Utilities.PgDoc.Helpers;
using MixERP.Net.Utilities.PgDoc.Models;

namespace MixERP.Net.Utilities.PgDoc.Parsers
{
    internal static class ColumnParser
    {
        internal static string Parse(string content, List<string> matches, ICollection<PgColumn> columns)
        {
            foreach (string match in matches)
            {
                string comment = HtmlHelper.RemoveComment(match);

                if (!comment.StartsWith("ColumnInfo"))
                {
                    continue;
                }

                comment = comment.Replace("ColumnInfo", "");

                StringBuilder items = new StringBuilder();

                foreach (PgColumn column in columns)
                {
                    Console.WriteLine("Parsing column {0} on table {1}.{2}.", column.Name, column.SchemaName,
                        column.TableName);
                    string indicator = "<i class='disabled ellipsis vertical icon'></i>";
                    string keyIndicatorCssClass = string.Empty;

                    if (column.IsPrimaryKey)
                    {
                        keyIndicatorCssClass = " class='error'";
                        indicator = "<i class='red key icon' title='Primary Key'></i>";
                    }
                    if (!string.IsNullOrWhiteSpace(column.ForiegnKeyName))
                    {
                        keyIndicatorCssClass = " class='warning'";
                        indicator = "<i class='yellow location arrow icon' title='Foreign Key'></i>";
                    }

                    string nullable =
                        "<input type='checkbox' disabled='disabled' title='This is a NULLABLE column.' />";

                    if (column.IsNullable)
                    {
                        nullable =
                            "<input type='checkbox' disabled='disabled' checked='checked' title='This is a NON NULLABLE column.' />";
                    }


                    items.Append(comment.Replace("[Name]", column.Name)
                        .Replace("[OrdinalPosition]", column.OrdinalPosition.ToString())
                        .Replace("[IsNullable]", nullable)
                        .Replace("[DataType]", column.DataType)
                        .Replace("[MaxLength]", column.MaxLength.ToString())
                        .Replace("[Indicator]", indicator)
                        .Replace("[KeyIndicatorCssClass]", keyIndicatorCssClass)
                        .Replace("[Description]", column.Description));
                }

                content = content.Replace(match, items.ToString());
            }

            return content;
        }
    }
}