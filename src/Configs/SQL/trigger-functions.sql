SELECT
    p.oid,
    row_number() OVER(ORDER BY proname)                 AS row_number,
    n.nspname                                           AS object_schema,
    p.proname                                           AS function_name,
    a.rolname                                           AS owner,
    pg_catalog.pg_get_function_arguments(p.oid)         AS arguments,
    CASE
        WHEN p.proisagg THEN 'AGGREGATE FUNCTION'
        WHEN p.proiswindow THEN 'window'
        ELSE 'RETURN FUNCTION'
    END                                                 AS function_type,
    d.description
FROM pg_catalog.pg_proc p
INNER JOIN pg_authid a
ON p.proowner = a.oid
LEFT JOIN pg_catalog.pg_namespace n 
ON n.oid = p.pronamespace
LEFT JOIN pg_description d
ON p.oid = d.objoid
WHERE p.prorettype = 'pg_catalog.trigger'::pg_catalog.regtype 
AND n.nspname != ALL(ARRAY['information_schema', 'pg_catalog', 'pg_toast'])
ORDER BY proname;