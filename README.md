# CRG_E-Ticaret
If you are having trouble opening the project; Run "Update-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform -r" to Package Manager Console.
Moreover; The reason for this issue is "Missing plugins installed in the project".
For database connection : in <connectionstring> <add name="MSSQL" connectionString="Server=..........;Database=CRG_E__TICARET;Integrated Security=true" providerName="System.Data.SqlClient" / > write your own sql server name in the dotted space .
