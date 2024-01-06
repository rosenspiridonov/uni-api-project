using System.Text;

using DataReaderConsole;

//await DataWebClient.UploadRecords();

var organization = await DataWebClient.GetDataAsync("4C119bee275d420");

Console.WriteLine(organization.Name);

//var sb = new StringBuilder();

//sb.AppendLine("Choose operation");
//sb.AppendLine("1. Upload records from file.");
//sb.AppendLine("2. Get record. Input: OrganizationId");
//sb.AppendLine("3. Delete record. Input: OrganizationId");
//sb.AppendLine("4. Authenticate. Input: Username Password");
//sb.AppendLine("5. Register. Input: Email Username Password");


