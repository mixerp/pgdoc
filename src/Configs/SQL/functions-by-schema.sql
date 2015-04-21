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
    row_number() OVER(ORDER BY proname)         AS row_number,
    p.oid,
    pg_get_functiondef(p.oid)::text             AS definition,
    n.nspname                                   AS object_schema,
    p.proname                                   AS function_name,
    a.rolname                                   AS owner,
    pg_catalog.pg_get_function_arguments(p.oid) AS arguments,
    pg_catalog.pg_get_function_result(p.oid)    AS result_type,
    CASE
        WHEN p.proisagg THEN 'AGGREGATE FUNCTION'
        WHEN p.proiswindow THEN 'window'
        ELSE 'RETURN FUNCTION'
    END                                         AS function_type,
    d.description
FROM pg_catalog.pg_proc p
INNER JOIN pg_language 
ON p.prolang = pg_language.oid
INNER JOIN pg_authid a
ON p.proowner = a.oid
LEFT JOIN pg_catalog.pg_namespace n 
ON n.oid = p.pronamespace
LEFT JOIN pg_description d
ON p.oid = d.objoid
WHERE n.nspname = @SchemaName
AND pg_language.lanname NOT IN ('c','internal') 
AND p.prorettype != 'pg_catalog.trigger'::pg_catalog.regtype 
ORDER BY proname;