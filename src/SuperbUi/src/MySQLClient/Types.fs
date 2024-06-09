namespace SuperbUi.MySQLClient

open SuperbGraphQL

module Types =
  type Schema = GetSchemata.SchemaType
  type SchemaTable = GetTables.TableType
  type SelectedSchema = Schema option
  type SelectSchemaHandler = SelectedSchema -> unit
