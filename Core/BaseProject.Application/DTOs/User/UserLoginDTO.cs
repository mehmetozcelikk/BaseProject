using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Application.DTOs.User
{
    public class UserLoginDTO : RefreshedTokenDto
    {
        public UserForLoginDto userForLoginDto { get; set; }
        public string IPAddress { get; set; }
        public string? denemememememe { get; set; }
        public int? denememememesadasdasdasdme { get; set; }
    }
}
