using System.Text;

using DataReaderConsole;

bool running = true;


var sb = new StringBuilder();
sb.AppendLine();
sb.AppendLine("Choose operation:");
sb.AppendLine("1. Authenticate");
sb.AppendLine("2. Upload records from file");
sb.AppendLine("3. Get record");
sb.AppendLine("4. Get top 3 biggest organizations");
sb.AppendLine("5. Get number of employees for each industry");
sb.AppendLine("6. Delete record");
sb.AppendLine("7. Close");

while (running)
{
    Console.WriteLine(sb.ToString());
    Console.Write("Enter your choice: ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            await AuthenticateUser();
            break;
        case "2":
            await UploadRecords();
            break;
        case "3":
            await GetRecord();
            break;
        case "4":
            await GetTop3Organizations();
            break;
        case "5":
            await GetIndustryEmployees();
            break;
        case "6":
            await DeleteRecord();
            break;
        case "7":
            running = false;
            break;
        default:
            Console.WriteLine("Invalid choice.");
            break;
    }
}

static async Task AuthenticateUser()
{
    Console.Write("Enter username: ");
    var username = Console.ReadLine();
    Console.Write("Enter password: ");
    var password = Console.ReadLine();

    var result = await DataWebClient.AuthenticateAsync(username, password);
    Console.WriteLine(result ? "Authentication successful." : "Authentication failed.");
}

static async Task UploadRecords()
{
    var result = await DataWebClient.UploadRecords();
    Console.WriteLine(result ? "Records uploaded successfully." : "Failed to upload records.");
}

static async Task GetRecord()
{
    Console.Write("Enter OrganizationId: ");
    var organizationId = Console.ReadLine();
    var organization = await DataWebClient.GetOrganizationAsync(organizationId);
    if (organization is not null)
    {
        Console.WriteLine($"Organization Name: {organization.Name}");
    }
    else
    {
        Console.WriteLine("Organization not found.");
    }
}

static async Task GetTop3Organizations()
{
    var result = await DataWebClient.GetTop3OrganizationsAsync();

    if (result is null)
    {
        Console.WriteLine("Failed to get records.");
        return;
    }

    foreach (var org in result)
    {
        Console.WriteLine(org.Name);
    }
}

static async Task GetIndustryEmployees()
{
    var result = await DataWebClient.GetIndustryEmployees();
    if (result is null)
    {
        Console.WriteLine("Failed to get records.");
        return;
    }

    foreach (var industry in result)
    {
        Console.WriteLine(industry.Industry + " - Employees: " + industry.NumberOfEmployees);
    }
}

static async Task DeleteRecord()
{
    Console.Write("Enter OrganizationId: ");
    var organizationId = Console.ReadLine();
    await DataWebClient.DeleteOrganizationAsync(organizationId);
}
