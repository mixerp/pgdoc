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
namespace MixERP.Net.Utilities.PgDoc.Models
{
    public sealed class PgFunction
    {
        public string Arguments { get; set; }
        public string Description { get; set; }
        public string FunctionType { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string ResultType { get; set; }
        public long RowNumber { get; set; }
        public string SchemaName { get; set; }
        public string FunctionDefinition { get; set; }
        public string FunctionOid { get; set; }
    }
}