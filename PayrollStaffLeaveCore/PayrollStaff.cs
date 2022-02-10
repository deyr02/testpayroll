using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollStaffLeaveCore
{
    public class PayrollStaffLeave
    {
        public int StaffCode { get; set; }
        public double AnnaulLeaveAccrued { get; set; }
        public double AnnaulLeaveOutanding { get; set; }
        public double AnnaulLeaveAdvanced { get; set; }
        public double AnnaulLeaveBalance { get; set; }
        public double SickLeaveBalance { get; set; }
    }

    public class StaffResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }

        public PayrollStaffLeave Leave { get; set; }
    }

    public class StaffsResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }

        public List<PayrollStaffLeave> Leave { get; set; }
    }
}
