[<RequireQualifiedAccess>]
module rec SuperbGraphQL.GetSchemata

type SchemaType = {
  catalogName: string
  defaultCharacterSetName: string
  defaultCollationName: string
  defaultEncryption: string
  schemaName: string
}

/// The root query type intended for use with GraphQL. Each member representsan available root field.
type Query = {
  schemata: Option<list<Option<SchemaType>>>
}
