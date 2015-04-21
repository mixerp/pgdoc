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
    pg_tables.schemaname                                    AS table_schema, 
    pg_tables.tablename                                     AS table_name, 
    pg_attribute.attname                                    AS column_name,
    pg_description.description,
    constraint_name,
    references_schema, 
    references_table, 
    references_field, 
    pg_attribute.attnum                                     AS ordinal_position,
    NOT pg_attribute.attnotnull                             AS is_nullable, 
    (SELECT 
        pg_attrdef.adsrc 
        FROM pg_attrdef 
        WHERE pg_attrdef.adrelid = pg_class.oid 
        AND pg_attrdef.adnum = pg_attribute.attnum)         AS column_default,    
    format_type(pg_attribute.atttypid, NULL)                AS data_type, 
    format_type(pg_attribute.atttypid, NULL)                AS domain_name, 
    CASE pg_attribute.atttypmod
    WHEN -1 THEN NULL 
    ELSE pg_attribute.atttypmod - 4
    END                                                     AS character_maximum_length,    
    pg_constraint.conname                                   AS key
FROM pg_tables
INNER JOIN pg_class 
ON pg_class.relname = pg_tables.tablename 
INNER JOIN pg_attribute 
    ON pg_class.oid = pg_attribute.attrelid 
    AND pg_attribute.attnum > 0 
LEFT JOIN pg_description
    ON pg_attribute.attrelid = pg_description.objoid 
    AND pg_attribute.attnum = pg_description.objsubid
LEFT JOIN pg_constraint 
    ON pg_constraint.contype = 'p'::char 
    AND pg_constraint.conrelid = pg_class.oid 
    AND (pg_attribute.attnum = ANY (pg_constraint.conkey)) 
LEFT JOIN pg_constraint AS pc2 
    ON pc2.contype = 'f'::char 
    AND pc2.conrelid = pg_class.oid 
    AND (pg_attribute.attnum = ANY (pc2.conkey))    
LEFT JOIN 
(
    SELECT 
        pg_constraint.conname                                           AS constraint_name,
        (SELECT nspname FROM pg_namespace WHERE oid=m.relnamespace)     AS table_schema,
        m.relname                                                       AS table_name,
        (SELECT a.attname FROM pg_attribute a 
        WHERE a.attrelid = m.oid
        AND a.attnum = pg_constraint.conkey[1]
        AND a.attisdropped = FALSE)                                     AS column_name,
        (SELECT nspname FROM pg_namespace
            WHERE oid=f.relnamespace)                                   AS references_schema,
            f.relname                                                   AS references_table,
        (SELECT a.attname FROM pg_attribute a
            WHERE a.attrelid = f.oid
            AND a.attnum = pg_constraint.confkey[1]
            AND a.attisdropped = FALSE)                                 AS references_field
    FROM pg_constraint
    LEFT JOIN pg_class c 
    ON c.oid = pg_constraint.conrelid
    LEFT JOIN pg_class f 
    ON f.oid = pg_constraint.confrelid
    LEFT JOIN pg_class m 
    ON m.oid = pg_constraint.conrelid
    WHERE pg_constraint.contype = 'f'
    AND pg_constraint.conrelid IN
    (
        SELECT oid 
        FROM pg_class c 
        WHERE c.relkind = 'r'
    )
)
relationship_view 
ON pg_tables.schemaname = relationship_view.table_schema 
    AND pg_tables.tablename = relationship_view.table_name 
    AND pg_attribute.attname = relationship_view.column_name 
WHERE NOT pg_attribute.attisdropped
AND schemaname = @SchemaName
AND tablename = @TableName
ORDER BY pg_attribute.attnum;