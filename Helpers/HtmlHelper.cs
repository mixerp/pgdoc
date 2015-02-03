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
using System.Linq;
using HtmlAgilityPack;

namespace MixERP.Net.Utilities.PgDoc.Helpers
{
    internal static class HtmlHelper
    {
        internal static List<string> GetMatch(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return null;
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//comment()");

            return
                (from node in nodes where !node.InnerText.StartsWith("<!DOCTYPE html>") select node.InnerText).ToList();
        }

        internal static string RemoveComment(string content)
        {
            return content.Replace("<!--", "").Replace("-->", "").Replace(Environment.NewLine, "").Trim();
        }
    }
}