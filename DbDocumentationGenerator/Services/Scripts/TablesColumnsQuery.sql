SELECT DB_NAME() [DatabaseName],
    t.object_id [TableId],
    t.name [TableName],
    c.column_id [ColumnId],
    c.name [ColumnName]
        , t2.name [ColumnDataType]
        , t2.max_length [MaxLength]
        , t2.PRECISION [Precision]
        , t2.scale [Scale]
        , c.is_nullable [IsNullable],
    pk.is_primary_key [IsPrimaryKey],
    IIF (pk.[is_primary_key] = 1, pk.[name], NULL) [PkName]
FROM sys.tables t
    JOIN sys.indexes pk
    ON t.object_id = pk.object_id
    JOIN sys.index_columns ic
    ON ic.object_id = pk.object_id
        AND ic.index_id = pk.index_id
    JOIN sys.columns c
    ON pk.object_id = c.object_id
        AND c.column_id = ic.column_id
    JOIN sys.types t2 ON c.system_type_id = t2.system_type_id
WHERE t2.name != 'sysname' AND t.[type] = 'U'