using Commerce6.Test.Helpers;
using Commerce6.Web.Models.Contact.AddressDTOs;

namespace Commerce6.Test.DataGenerators
{
    internal class AddressGenerator
    {
        private static List<(string, List<string>)> Administrative = new List<(string, List<string>)>();

        private static string[] StreetNames =
        {
            "Hai Bà Trưng", "Ngô Quyền", "Đinh Bộ Lĩnh", "Lê Đại Hành", "Lý Thái Tổ", "Trần Nhân Tông", "Trần Thánh Tông", "Lê Lợi", "Lê Thánh Tông", "Nguyễn Huệ",
            "Lý Thường Kiệt", "Trần Hưng Đạo", "Nguyễn Tri Phương", "Hoàng Hoa Thám", "Phan Đình Phùng", "Nguyễn Thái Học", "Trần Phú", "Võ Nguyên Giáp", "Nguyễn Chí Thanh"
        };

        static AddressGenerator()
        {
            string path = DirHelper.GetTestProjectDir() + @"\Data\Address\administrative.txt";
            try
            {
                string text = File.ReadAllText(path);
                string[] parts = text.Split(Environment.NewLine + Environment.NewLine);

                string[] lines;
                (string, List<string>) address;
                int i;
                foreach (string part in parts)
                {
                    lines = part.Split(Environment.NewLine);

                    address.Item1 = lines[0].Substring(lines[0].IndexOf('-') + 1);             // != .Split(...)[1]
                    address.Item2 = new List<string>();
                    for (i = 1; i < lines.Length; i++)
                        address.Item2.Add(lines[i].Substring(lines[i].IndexOf('-') + 1));

                    Administrative.Add(address);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static AddressRequestDTO GenerateRandomAddressDTO()
        {
            Random rd = new();
            (string, List<string>) tmp = Administrative[rd.Next(Administrative.Count)];
            string province = tmp.Item1;
            string district = tmp.Item2[rd.Next(tmp.Item2.Count)];
            string street = StreetNames[rd.Next(StreetNames.Length)];
            string streetNumber = "Số " + rd.Next(400);
            return new AddressRequestDTO
            {
                Province = province,
                District = district,
                Street = street,
                StreetNumber = streetNumber
            };
        }
    }
}
