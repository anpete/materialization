using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace MaterializationSandbox
{
    internal class Program
    {
        public const string ConnectionString
            = @"Data Source=.\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True";

        public const string ConnectionString2
            = @"Data Source=.\SQLEXPRESS;Initial Catalog=AdventureWorks2008;Integrated Security=True";

        private static void Main()
        {
            var times = new List<long>();

            for (var i = 0; i < 50; i++)
            {
                times.Add(RunTest2());
            }

            Console.WriteLine("Average: " + times.Average() + "ms");
        }

        private static void RunTest()
        {
            var stopwatch = new Stopwatch();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.StatisticsEnabled = false;

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from employees";

                    using (var reader = command.ExecuteReader())
                    {
                        var values = new object[reader.FieldCount];
                        var employees = new List<Employee>();

                        stopwatch.Start();

                        while (reader.Read())
                        {
                            reader.GetValues(values);

                            employees.Add(new Employee(values));
                        }

                        stopwatch.Stop();

                        Console.WriteLine("Elapsed: "
                                          + stopwatch.ElapsedMilliseconds
                                          + "ms, Mem: "
                                          + string.Format("{0:N}", GC.GetTotalMemory(true)/1024/1024)
                                          + "MB (" + employees.Count + ")");
                    }
                }
            }
        }

        private static long RunTest2()
        {
            var stopwatch = new Stopwatch();

            using (var connection = new SqlConnection(ConnectionString2))
            {
                connection.Open();
                connection.StatisticsEnabled = false;

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from sales.salesorderheader";

                    using (var reader = command.ExecuteReader())
                    {
                        var values = new object[reader.FieldCount];
                        var employees = new List<SalesOrderHeader>();

                        stopwatch.Start();

                        while (reader.Read())
                        {
                            reader.GetValues(values);

                            employees.Add(new SalesOrderHeader(values));
                        }

                        stopwatch.Stop();

//                        Console.WriteLine("Elapsed: "
//                                          + stopwatch.ElapsedMilliseconds
//                                          + "ms, Mem: "
//                                          + string.Format("{0:N}", GC.GetTotalMemory(true)/1024/1024)
//                                          + "MB (" + employees.Count + ")");
                    }
                }
            }

            return stopwatch.ElapsedMilliseconds;
        }
    }

    public class Employee
    {
        private static readonly Dictionary<string, string> _cache = new Dictionary<string, string>();

        public Employee(object[] values)
        {
            EmployeeId = (int)values[0];
            LastName = FromCache((string)values[1]);
            FirstName = FromCache((string)values[2]);
            Title = ReferenceEquals(DBNull.Value, values[3]) ? null : FromCache((string)values[3]);
            TitleOfCourtesy = ReferenceEquals(DBNull.Value, values[4]) ? null : FromCache((string)values[4]);
            BirthDate = ReferenceEquals(DBNull.Value, values[5]) ? null : (DateTime?)values[5];
            HireDate = ReferenceEquals(DBNull.Value, values[6]) ? null : (DateTime?)values[6];
            Address = ReferenceEquals(DBNull.Value, values[7]) ? null : FromCache((string)values[7]);
            City = ReferenceEquals(DBNull.Value, values[8]) ? null : FromCache((string)values[8]);
            Region = ReferenceEquals(DBNull.Value, values[9]) ? null : FromCache((string)values[9]);
            PostalCode = ReferenceEquals(DBNull.Value, values[10]) ? null : FromCache((string)values[10]);
            Country = ReferenceEquals(DBNull.Value, values[11]) ? null : FromCache((string)values[11]);
            HomePhone = ReferenceEquals(DBNull.Value, values[12]) ? null : FromCache((string)values[12]);
            Extension = ReferenceEquals(DBNull.Value, values[13]) ? null : FromCache((string)values[13]);
            Photo = ReferenceEquals(DBNull.Value, values[14]) ? null : (byte[])values[14];
            Notes = ReferenceEquals(DBNull.Value, values[15]) ? null : FromCache((string)values[15]);
            ReportsTo = ReferenceEquals(DBNull.Value, values[16]) ? null : (int?)values[16];
            PhotoPath = ReferenceEquals(DBNull.Value, values[17]) ? null : FromCache((string)values[17]);
        }

        private static string FromCache(string s)
        {
            string cached;
            if (_cache.TryGetValue(s, out cached))
            {
                return cached;
            }

            _cache.Add(s, s);

            return s;
        }

        public int EmployeeId;
        public string LastName;
        public string FirstName;
        public string Title;
        public string TitleOfCourtesy;
        public DateTime? BirthDate;
        public DateTime? HireDate;
        public string Address;
        public string City;
        public string Region;
        public string PostalCode;
        public string Country;
        public string HomePhone;
        public string Extension;
        public byte[] Photo;
        public string Notes;
        public int? ReportsTo;
        public string PhotoPath;
    }

    public class SalesOrderHeader
    {
        public SalesOrderHeader(object[] values)
        {
            SalesOrderId = (int)values[0];
            RevisionNumber = (byte)values[1];
            OrderDate = (DateTime)values[2];
            ShipDate = ReferenceEquals(DBNull.Value, values[3]) ? null : (DateTime?)values[3];
            DueDate = (DateTime)values[4];
            Status = (byte)values[5];
            OnlineOrderFlag = (bool)values[6];
            SalesOrderNumber = ReferenceEquals(DBNull.Value, values[7]) ? null : (string)values[7];
            PurchaseOrderNumber = ReferenceEquals(DBNull.Value, values[8]) ? null : (string)values[8];
            AccountNumber = ReferenceEquals(DBNull.Value, values[9]) ? null : (string)values[9];
            CustomerId = (int)values[10];
            SalesPersonId = ReferenceEquals(DBNull.Value, values[11]) ? null : (int?)values[11];
            TerritoryId = ReferenceEquals(DBNull.Value, values[12]) ? null : (int?)values[12];
            BillToAddressId = (int)values[13];
            ShipToAddressId = (int)values[14];
            ShipMethodId = (int)values[15];
            CreditCardId = ReferenceEquals(DBNull.Value, values[16]) ? null : (int?)values[16];
            CreditCardApprovalCode = ReferenceEquals(DBNull.Value, values[17]) ? null : (string)values[17];
            CurrencyRateId = ReferenceEquals(DBNull.Value, values[18]) ? null : (int?)values[18];
            SubTotal = (decimal)values[19];
            TaxAmt = (decimal)values[20];
            Freight = (decimal)values[21];
            TotalDue = (decimal)values[22];
            Comment = ReferenceEquals(DBNull.Value, values[23]) ? null : (string)values[23];
            Rowguid = (Guid)values[24];
            ModifiedDate = (DateTime)values[25];
        }

        public int SalesOrderId;
        public byte RevisionNumber;
        public DateTime OrderDate;
        public DateTime? ShipDate;
        public DateTime DueDate;
        public byte Status;
        public bool OnlineOrderFlag;
        public string SalesOrderNumber;
        public string PurchaseOrderNumber;
        public string AccountNumber;
        public int CustomerId;
        public int? SalesPersonId;
        public int? TerritoryId;
        public int BillToAddressId;
        public int ShipToAddressId;
        public int ShipMethodId;
        public int? CreditCardId;
        public string CreditCardApprovalCode;
        public int? CurrencyRateId;
        public decimal SubTotal;
        public decimal TaxAmt;
        public decimal Freight;
        public decimal TotalDue;
        public string Comment;
        public Guid Rowguid;
        public DateTime ModifiedDate;
    }
}