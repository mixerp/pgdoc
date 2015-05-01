SELECT 
    row_number() 
    OVER
    (
        ORDER BY 
        sequence_schema, sequence_name
    )                                           AS row_number,
    sequence_schema,
    sequence_name,
    data_type,
    increment,
    description,
    rolname                                     AS owner,
    start_value,
    minimum_value,
    maximum_value
FROM information_schema.sequences
INNER JOIN pg_class
ON (information_schema.sequences.sequence_schema || '.' || information_schema.sequences.sequence_name)::regclass::oid = pg_class.oid
INNER JOIN pg_roles
ON pg_class.relowner = pg_roles.oid
LEFT JOIN pg_description
ON pg_class.relfilenode = pg_description.objoid
WHERE sequence_schema similar to @SchemaPattern
AND sequence_schema not similar to @xSchemaPattern
ORDER BY sequence_schema, sequence_name;