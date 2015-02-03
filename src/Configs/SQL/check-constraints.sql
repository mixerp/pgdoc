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

SELECT
    pg_constraint.conname                               AS constraint_name,    
    pg_get_constraintdef(pg_constraint.oid, true)       AS definition,
    pg_description.description,
    pg_constraint.condeferrable                         AS deferrable,
    pg_constraint.condeferred                           AS deferred,
    pg_constraint.conislocal                            AS is_local,
    pg_constraint.connoinherit                          AS no_inherit
FROM pg_constraint
INNER JOIN pg_class
ON pg_class.oid = pg_constraint.conrelid
INNER JOIN pg_namespace
ON pg_class.relnamespace = pg_namespace.oid
LEFT JOIN pg_description
ON pg_description.objoid = pg_constraint.oid
WHERE contype = ANY(ARRAY['c', 't'])
AND pg_namespace.nspname = @SchemaName
AND pg_class.relname = @TableName;