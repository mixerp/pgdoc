SELECT
    trigger_schema,
    trigger_name,
    event_manipulation,
    action_condition,
    action_order,
    action_orientation,
    action_timing,
    pg_trigger.tgfoid           AS target_function_oid,
    pg_pronamespace.nspname     AS target_function_schema,
    pg_proc.proname             AS target_function_name,
    description
FROM pg_trigger
INNER JOIN pg_proc
ON pg_trigger.tgfoid = pg_proc.oid
INNER JOIN pg_namespace AS pg_pronamespace
ON pg_proc.pronamespace = pg_pronamespace.oid
INNER JOIN pg_class
ON pg_trigger.tgrelid = pg_class.oid
INNER JOIN pg_namespace
ON pg_class.relnamespace = pg_namespace.oid
LEFT JOIN pg_description
ON  pg_description.objoid = pg_trigger.oid 
INNER JOIN information_schema.triggers
ON pg_trigger.tgname = information_schema.triggers.trigger_name
AND pg_namespace.nspname = information_schema.triggers.event_object_schema
AND pg_class.relname = information_schema.triggers.event_object_table
WHERE event_object_schema = @SchemaName
AND event_object_table=@TableName;