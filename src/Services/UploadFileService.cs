using ClubManagementSystem.Data;
using ClubManagementSystem.Data.Entities;
using ClubManagementSystem.Enums;
using ClubManagementSystem.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;

namespace ClubManagementSystem.Services
{
    public class UploadFileService : IUploadFileService
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly Dictionary<string, string> _csvColumnToStudentProp =
            new Dictionary<string, string>() {
                { "Preferred First Name", "FirstName" },
                { "Preferred Last Name", "LastName" },
                { "Gender", "Gender" },
                { "Date of Birth", "BirthDate"}
            };

        public UploadFileService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<(UploadMembersOutcome, int?)> UploadMembers(Stream csvStream)
        {
            if (csvStream == null)
                return (UploadMembersOutcome.FileNotFound, null);

            if (csvStream.Length == 0)
                return (UploadMembersOutcome.EmptyFile, null);

            var memoryStream = new MemoryStream();
            await csvStream.CopyToAsync(memoryStream);

            string[] csvColumns = null!;
            string[][] csvRows = null!;

            try
            {
                csvRows = GetCsvRows(memoryStream);
                csvColumns = csvRows[0];
            }
            catch (Exception)
            {
                return (UploadMembersOutcome.InvalidFile, null);
            }

            if (!ValidateColumns(csvColumns))
                return (UploadMembersOutcome.MissingRequiredColumns, null);

            using var context = _dbContextFactory.CreateDbContext();
            var members = await context.Members.ToListAsync();
            var newMembers = new List<Member>();

            for (int row = 1; row < csvRows.Length; row++)
            {
                var member = CreateNewMember(csvColumns, csvRows[row]);
                var memberExists = members
                    .Any(s => s.FirstName.Equals(member.FirstName, StringComparison.OrdinalIgnoreCase)
                      && s.LastName.Equals(member.LastName, StringComparison.OrdinalIgnoreCase));
                if (!memberExists)
                    newMembers.Add(member);
            }

            if (newMembers.Any())
            {
                try
                {
                    await context.Members.AddRangeAsync(newMembers);
                    await context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return (UploadMembersOutcome.InsertFailed, null);
                }
            }
                

            return (UploadMembersOutcome.Success, newMembers.Count);
        }

        private bool ValidateColumns(string[] csvColumns)
        {
            return true;
            // check if all required columns exist
        }

        private string[][] GetCsvRows(MemoryStream csvStream)
        {
            return Encoding.UTF8
               .GetString(csvStream.ToArray())
               .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
               .Select(row => row.Split(",")
                    .Select(rowValue => rowValue.Replace("\"", String.Empty))
                    .ToArray())
               .ToArray();
        }

        private Member CreateNewMember(string[] csvColumns, string[] csvRow)
        {
            var member = new Member();

            var memberType = member.GetType();

            for (int colNum = 0; colNum < csvRow.Length; colNum++)
            {
                var columnName = csvColumns[colNum];
                var cellValue = csvRow[colNum];

                if (_csvColumnToStudentProp.ContainsKey(columnName) &&
                    !string.IsNullOrWhiteSpace(cellValue))
                {
                    var studentPropName = _csvColumnToStudentProp[columnName];
                    var studentProp = memberType.GetProperty(studentPropName);
                    if (studentProp == null)
                        continue;

                    var propValue = GetPropertyValue(cellValue, studentProp);
                    studentProp.SetValue(member, propValue);
                }
            }
            return member;
        }

        private object GetPropertyValue(string cellValue, PropertyInfo prop)
        {
            if (prop.PropertyType == typeof(Gender))
                return Enum.Parse(prop.PropertyType, cellValue);
            if (prop.PropertyType == typeof(DateTime?))
                return DateTime.ParseExact(cellValue, "dd/MM/yyyy", null);

            return cellValue;
        }
    }
}
