SELECT 
    row_number() OVER(ORDER BY pg_class.relname)    AS row_number,
    pg_namespace.nspname                            AS object_schema,
    pg_class.relname                                AS object_name, 
    pg_roles.rolname                                AS owner,
    CASE 
        WHEN pg_tablespace.spcname IS NULL 
        THEN 'DEFAULT' 
        ELSE pg_tablespace.spcname 
    END                                             AS tablespace,
    pg_description.description
FROM pg_class
INNER JOIN pg_roles ON pg_roles.oid = pg_class.relowner
INNER JOIN pg_namespace ON pg_namespace.oid = pg_class.relnamespace
LEFT JOIN pg_tablespace ON pg_class.reltablespace = pg_tablespace.oid
LEFT JOIN pg_description ON pg_description.objoid = pg_class.oid
AND pg_description.objsubid = 0
WHERE pg_namespace.nspname = @SchemaName
AND pg_class.relkind = 'r'
ORDER BY pg_class.relname;