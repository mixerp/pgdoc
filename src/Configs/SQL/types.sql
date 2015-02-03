SELECT 
    row_number() 
    OVER(
        ORDER BY n.nspname, 
        pg_catalog.format_type(t.oid, NULL)
        )                                                           AS row_number,
    n.nspname                                                       AS schema_name,
    t.typname                                                       AS type_name,
    pg_catalog.format_type(t.typbasetype, NULL)                     AS base_type,
    pg_roles.rolname                                                AS owner,
    pg_collation.collname                                           AS collation,
    typdefault                                                      AS default,
    CASE typtype 
    WHEN 'b' THEN 'Base Type'
    WHEN 'c' THEN 'Composite Type'
    WHEN 'd' THEN 'Domain'
    WHEN 'e' THEN 'Enum'
    WHEN 'p' THEN 'Pseudo Type'
    WHEN 'r' THEN 'Range'
    END                                                             AS type,
    CASE  typstorage
    WHEN 'p' THEN 'Plain'
    WHEN 'e' THEN 'Secondary'
    WHEN 'm' THEN 'Compressed Inline'
    WHEN 'x' THEN 'Compressed Inline/Seconary'
    END                                                             AS store_type,
    typnotnull                                                      AS not_null,
    pg_catalog.pg_get_typedef(t.typrelid)                           AS definition,
    pg_catalog.obj_description(t.oid, 'pg_type')                    AS description
FROM pg_catalog.pg_type t
LEFT JOIN pg_collation
ON pg_collation.oid = t.typcollation
INNER JOIN pg_roles
ON t.typowner = pg_roles.oid
LEFT JOIN pg_catalog.pg_namespace n 
ON n.oid = t.typnamespace
WHERE (t.typrelid = 0 OR (SELECT c.relkind = 'c' FROM pg_catalog.pg_class c WHERE c.oid = t.typrelid))
AND NOT EXISTS
(
    SELECT 1 
    FROM pg_catalog.pg_type el 
    WHERE el.oid = t.typelem 
    AND el.typarray = t.oid
)
AND n.nspname != ALL(ARRAY['information_schema', 'pg_catalog', 'pg_toast'])
ORDER BY 1, 2;