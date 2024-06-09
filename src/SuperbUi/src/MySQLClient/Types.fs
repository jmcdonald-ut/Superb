namespace SuperbUi.MySQLClient

open SuperbGraphQL

module Types =
  type Schema = GetSchemata.SchemaType
  type SchemaTable = GetTables.TableType
  type SelectedSchema = Schema option
  type SelectedTable = SchemaTable option
  type TableRow = GetSampleOfTableRows.RowType
  type SelectSchemaHandler = SelectedSchema -> unit
