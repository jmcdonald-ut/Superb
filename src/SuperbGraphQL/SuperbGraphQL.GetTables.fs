[<RequireQualifiedAccess>]
module rec SuperbGraphQL.GetTables

type InputVariables = { schemaName: string }

type TableType =
    { autoIncrement: int
      avgRowLength: int
      checksum: int
      checkTime: System.DateTime
      createOptions: string
      createTime: System.DateTime
      dataFree: int
      dataLength: int
      engine: string
      indexLength: int
      maxDataLength: int
      rowFormat: string
      tableCatalog: string
      tableCollation: string
      tableComment: string
      tableName: string
      tableRows: int
      tableSchema: string
      tableType: string
      updateTime: System.DateTime
      version: Option<int> }

type Query =
    { tables: Option<list<Option<TableType>>> }
