[<RequireQualifiedAccess>]
module rec SuperbGraphQL.GetSampleOfTableRows

type InputVariables = {
  schemaName: string
  tableName: string
  count: int
}

type ColumnType = {
  columnKey: string
  columnName: string
  columnDefault: string
  columnComment: string
  dataType: string
  ordinalPosition: int
}

type RowFieldValueType = { key: string; value: string }
type RowType = { values: list<RowFieldValueType> }

type SampleOfTableRowsType = {
  columns: list<ColumnType>
  rows: list<RowType>
}

/// The root query type intended for use with GraphQL. Each member representsan available root field.
type Query = { tableRows: SampleOfTableRowsType }
