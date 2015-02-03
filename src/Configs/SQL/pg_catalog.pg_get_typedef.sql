CREATE OR REPLACE FUNCTION pg_catalog.pg_get_typedef(oid)
RETURNS text
AS
$$
    DECLARE _namespace text;
    DECLARE _name text;
    DECLARE _cols text;
    DECLARE _sql text;
    DECLARE _typtype char;
BEGIN
    SELECT 
        pg_namespace.nspname,
        pg_class.relname
    INTO
        _namespace,
        _name
    FROM pg_class
    INNER JOIN pg_namespace
    ON pg_class.relnamespace = pg_namespace.oid
    WHERE pg_class.oid=$1;

    IF(COALESCE(_namespace, '') = '') THEN
        RETURN '--NOT A COMPOSITE TYPE';
    END IF;

    SELECT 
    INTO _cols
    array_to_string
    (
    ARRAY(
    (
    SELECT
        format
        (
            E'&nbsp;&nbsp;&nbsp;&nbsp;%1$s %2$s%3$s%4$s', 
            attname, 
            format_type(t.oid,NULL),
            CASE WHEN atttypmod > 0 THEN '(' || atttypmod ::text || ')' ELSE '' END, 
            CASE WHEN COALESCE(c.collname::text, 'default') = 'default' THEN '' ELSE ' COLLATE ' || nspc.nspname || '.' || collname || '' END
        ) as cols
    FROM pg_attribute att
    JOIN pg_type t 
    ON t.oid=atttypid
    JOIN pg_namespace nsp 
    ON t.typnamespace=nsp.oid
    LEFT OUTER JOIN pg_type b 
    ON t.typelem=b.oid
    LEFT OUTER JOIN pg_collation c 
    ON att.attcollation=c.oid
    LEFT OUTER JOIN pg_namespace nspc 
    ON c.collnamespace=nspc.oid
    WHERE att.attrelid=$1
    ORDER by attnum
    )), E',\n');

    _sql := format(E'CREATE TYPE %1$s.%2$s AS\n(\n%3$s\n);', _namespace, _name, _cols);

    RETURN _sql;
END
$$
LANGUAGE plpgsql;