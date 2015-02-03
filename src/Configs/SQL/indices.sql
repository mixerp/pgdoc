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
    pg_class.relname                                                AS index_name,
    pg_roles.rolname                                                AS owner,
    CASE WHEN pg_index.indisprimary 
    THEN 'PRIMARY KEY' 
    WHEN pg_index.indisunique 
    THEN 'UNIQUE INDEX' 
    ELSE 'INDEX' 
    END                                                             AS type,
    pg_am.amname                                                    AS access_method,
    array_to_string
    (
        ARRAY
        (
        SELECT pg_get_indexdef(pg_index.indexrelid, k + 1, true)
        FROM generate_subscripts(pg_index.indkey, 1) as k
        ORDER BY k
        ), 
    E'\n')                                                          AS definition,
    indisclustered                                                  AS is_clustered,
    indisvalid                                                      AS is_valid,       
    pg_description.description
FROM   pg_index
INNER JOIN   pg_class
ON pg_class.oid = pg_index.indexrelid
INNER JOIN   pg_class AS pg_class_table
ON pg_class_table.oid = pg_index.indrelid
INNER JOIN   pg_am
ON pg_class.relam = pg_am.oid
INNER JOIN   pg_namespace
ON pg_namespace.oid = pg_class.relnamespace
INNER JOIN pg_roles
ON pg_class.relowner = pg_roles.oid
LEFT JOIN pg_description
ON pg_class.oid = pg_description.objoid
WHERE pg_namespace.nspname = @SchemaName
AND pg_class_table.relname = @TableName
ORDER BY pg_index.indisprimary DESC;