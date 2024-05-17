using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_management.Model
{
    public class Employees
    {
        int employee_id;
        string name;
        string department;
        string email;
        string password;

        public Employees() { }

        public int Employee_id
        {
            get { return employee_id; }
            set { employee_id = value; }
        }
        public string Name { get { return name; } set {  name = value; } }

        public string Department { get {  return department; } set {  department = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string Password { get { return password; } set { password = value; } }

        public override string ToString()
        {
            return $"Name:{name}\tDepartment:{department}\temail:{email}\tpassword:{password}";
        }
    }



    
}
