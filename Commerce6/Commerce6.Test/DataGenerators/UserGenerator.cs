using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Commerce6.Test.DataGenerators
{
    internal class UserGenerator
    {
        //4-word names: add word to middle
        #region name data
        private readonly string[] LastNames =
        {
            "Nguyễn", "Trần", "Lê", "Phan", "Hoàng",
            "Huỳnh", "Võ", "Vũ", "Phạm", "Trương",
            "Bùi", "Đặng", "Đỗ", "Ngô", "Hồ",
            "Dương", "Đinh", "Lý"
        };

        private readonly string[] MaleMiddleNames =
        {
            "Văn", "Hữu", "Tuấn", "Mạnh", "Vỹ",
            "Công", "Nguyên", "Việt", "Đăng", "Bảo",
            "Đức", "Duy", "Hải", "Gia", "Hiếu",
            "Hoàng", "Huy", "Khánh", "Minh", "Nhật",
        };

        private readonly string[] FemaleMiddleNames =
        {
            "Thị", "Quỳnh", "Thu", "Mai", "Hoài"
        };

        private readonly string[] MaleFirstNames =
        {
            "Huy", "Khang", "Bảo", "Minh", "Phúc",
            "Anh", "Khoa", "Phát", "Đạt", "Khôi",
            "Long", "Nam", "Duy", "Quân", "Kiệt",
            "Thịnh", "Tuấn", "Hưng", "Hoàng", "Hiếu",
            "Nhân", "Trí", "Tài", "Phong", "Nguyên",
            "An", "Phú", "Thành", "Đức", "Dũng",
            "Lộc", "Khánh", "Vinh", "Tiến", "Nghĩa",
            "Thiện", "Hòa", "Hải", "Đăng", "Quang",
            "Lâm", "Nhật", "Trung", "Thắng", "Tú",
            "Hùng", "Tâm", "Sang", "Sơn", "Thái",
            "Cường", "Vũ", "Toàn", "Ân", "Thuận",
            "Bình", "Trường", "Danh", "Kiên", "Phước",
            "Thiên", "Tân", "Việt", "Khải", "Tín",
            "Dương", "Tùng", "Quý", "Hậu", "Trọng",
            "Triết", "Luân", "Phương", "Quốc", "Thông",
            "Khiêm", "Hòa", "Than", "Tường", "Kha",
            "Vỹ", "Bách", "Khang", "Mạnh", "Lợi",
            "Đại", "Hiệp", "Đông", "Tấn", "Công",
        };

        private readonly string[] FemaleFirstNames =
        {
            "Vy", "Ngọc", "Nhi", "Hân", "Thư",
            "Linh", "Như", "Ngân", "Phương", "Thảo",
            "My", "Quỳnh", "Trang", "Trâm", "Thy",
            "Châu", "Yến", "Uyên", "Yến", "Tiên",
            "Mai", "Hà", "Vân", "Hương", "Quyên",
            "Duyên", "Trinh", "Thanh", "Hằng", "Dương",
            "Chi", "Giang", "Tâm", "Tú", "Ánh",
            "Hiền", "Khánh", "Minh", "Huyền", "Thương",
            "Ly", "Dung", "Nhung", "Lan", "Nga",
            "Thúy", "Hoa", "Tuyết", "Thủy", "Hạnh",
            "Oanh", "Diệp", "Thương", "Nhiên", "Băng",
            "Hồng", "Loan", "Nguyệt", "Bích", "Đào",
            "Diễm", "Kiều", "Liên", "Trà", "Thắm"
        };
        #endregion

        #region email data
        private readonly string[] Emails =
        {
            "@gmail.com", "@gmail.com", "@gmail.com", "@gmail.com", "@gmail.com",
            "@gmail.com", "@gmail.com", "@gmail.com", "@gmail.com", "@gmail.com",
            "@gmail.com", "@gmail.com", "@gmail.com", "@gmail.com", "@gmail.com",
            "@outlook.com", "@hotmail.com", "@yahoo.com", "@yandex.com"
        };
        #endregion

        internal string GenerateMaleName()
        {
            Random rd = new();
            string lastName = LastNames[rd.Next(LastNames.Length)];
            string middleName = MaleMiddleNames[rd.Next(MaleMiddleNames.Length)];
            string firstName = MaleFirstNames[rd.Next(MaleFirstNames.Length)];
            return lastName + " " + middleName + " " + firstName;
        }

        internal string GenerateFemaleName()
        {
            Random rd = new();
            string lastName = LastNames[rd.Next(LastNames.Length)];
            string middleName = FemaleMiddleNames[rd.Next(FemaleMiddleNames.Length)];
            string firstName = FemaleFirstNames[rd.Next(FemaleFirstNames.Length)];
            return lastName + " " + middleName + " " + firstName;
        }



        internal string GeneratePhone()
        {
            Random rd = new();
            StringBuilder builder = new("0");
            int length = rd.Next(9, 11);                    //phoneNumber haves 10-11 digits

            for (int i = 0; i < length; i++)
                builder.Append(rd.Next(10));
            return builder.ToString();
        }



        /// <summary>
        /// Generate from name and phone
        /// </summary>
        internal string GeneratePassword(string name, string phone)
        {
            //replace all whitespace
            name = name.Replace(" ", "");
            //convert all to lower case, except the first char
            name = $"{char.ToUpper(name[0])}{name.ToLower()[1..]}";
            //convert to eng
            name = ConvertToEng(name);

            //get last 3 chars
            phone = phone[Math.Max(0, phone.Length - 3)..];
            return name + phone;
        }



        /// <summary>
        /// Generate from password
        /// </summary>
        internal string GenerateEmail(string password)
        {
            return password + Emails[new Random().Next(Emails.Length)];
        }



        internal DateTime GenerateDateOfBirth()
        {
            Random rd = new();
            return new DateTime(rd.Next(1970, 2010), rd.Next(1, 12), rd.Next(1, 29));
        }






        private string ConvertToEng(string input)
        {
            Regex regex = new("\\p{IsCombiningDiacriticalMarks}+");
            string temp = input.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
        }
    }
}
