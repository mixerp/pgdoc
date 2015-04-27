SELECT 
    nspname                 AS namespace,
    pg_authid.rolname       AS owner,
    description             AS description
FROM pg_namespace
INNER JOIN pg_authid
ON pg_authid.oid = pg_namespace.nspowner
LEFT JOIN pg_description
ON pg_namespace.oid= pg_description.objoid
WHERE pg_namespace.nspname NOT LIKE 'pg_%'
AND pg_namespace.nspname != 'information_schema'
and pg_namespace.nspname similar to @SchemaPattern
and pg_namespace.nspname not similar to @xSchemaPattern
ORDER BY pg_namespace.nspname;