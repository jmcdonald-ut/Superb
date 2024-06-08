[<RequireQualifiedAccess>]
module rec SuperbGraphQL.GetSchemata

type SchemaType =
    { catalogName: string
      defaultCharacterSetName: string
      defaultCollationName: string
      defaultEncryption: string
      schemaName: string }

type Query =
    { schemata: Option<list<Option<SchemaType>>> }
