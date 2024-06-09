[<RequireQualifiedAccess>]
module rec SuperbGraphQL.GetSampleOfTableRows

type InputVariables = {
  schemaName: string
  tableName: string
  count: int
}

type RowFieldValueType = { key: string; value: string }

type RowType = {
  values: Option<list<Option<RowFieldValueType>>>
}

/// The root query type intended for use with GraphQL. Each member representsan available root field.
type Query = {
  tableRows: Option<list<Option<RowType>>>
}
