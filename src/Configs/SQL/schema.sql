SELECT
    nspname                 AS namespace,
    pg_authid.rolname       AS owner,
    description             AS description
FROM pg_namespace
INNER JOIN pg_authid
ON pg_authid.oid = pg_namespace.nspowner
LEFT JOIN pg_description
ON pg_namespace.oid= pg_description.objoid
WHERE pg_namespace.nspname = @SchemaName
ORDER BY pg_namespace.nspname;