SELECT 
    row_number() OVER(ORDER BY pg_class.relname)            AS row_number,
    pg_namespace.nspname                                    AS object_schema,
    pg_class.relname                                        AS object_name, 
    pg_roles.rolname                                        AS owner,
    CASE 
        WHEN pg_tablespace.spcname IS NULL 
        THEN 'DEFAULT' 
        ELSE pg_tablespace.spcname 
    END                                                     AS tablespace,
    format
    (
        E' CREATE OR REPLACE VIEW %1$s.%2$s\n AS\n%3$s',
        pg_namespace.nspname,
        pg_class.relname,
        pg_get_viewdef(pg_class.oid, true) 
    )                                                       AS definition,
    pg_description.description
FROM pg_class
INNER JOIN pg_roles 
ON pg_roles.oid = pg_class.relowner
INNER JOIN pg_namespace 
ON pg_namespace.oid = pg_class.relnamespace
LEFT JOIN pg_tablespace 
ON pg_class.reltablespace = pg_tablespace.oid
LEFT JOIN pg_description 
ON pg_description.objoid = pg_class.oid
WHERE pg_namespace.nspname similar to @SchemaPattern
and pg_namespace.nspname not similar to @xSchemaPattern
AND pg_class.relkind = 'v'
ORDER BY pg_namespace.nspname, pg_class.relname;