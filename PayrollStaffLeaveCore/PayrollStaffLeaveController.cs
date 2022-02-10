using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DbfDataReader;

namespace PayrollStaffLeaveCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollStaffLeaveController: ControllerBase
    {
        private List<PayrollStaffLeave> GetStaffLeave(int staffCode)
        {
            List<PayrollStaffLeave> staffLeaveList = new List<PayrollStaffLeave>();

            var dbfPath = "C:\\PayrollDatabase\\staff.dbf";

            var options = new DbfDataReaderOptions
            {
                SkipDeletedRecords = true
            };

            using (var dbfDataReader = new DbfDataReader.DbfDataReader(dbfPath, options))
            {
                while (dbfDataReader.Read())
                {                    
                    var code = dbfDataReader.GetInt32(0);
                    var hol_bf_d2 = dbfDataReader.GetDecimal(44);
                    var hol_bf_d = dbfDataReader.GetDecimal(45);
                    var hol_cy_d = dbfDataReader.GetDouble(48);
                    var hol_cy_ad = dbfDataReader.GetDouble(49);
                    var sick_bal = dbfDataReader.GetDouble(61);

                    if (staffCode <= 0)
                    {
                        PayrollStaffLeave staffLeave = new PayrollStaffLeave();
                        staffLeave.StaffCode = code;
                        staffLeave.AnnaulLeaveAccrued = hol_cy_d;
                        staffLeave.AnnaulLeaveOutanding = Convert.ToDouble(hol_bf_d2 + hol_bf_d);
                        staffLeave.AnnaulLeaveAdvanced = hol_cy_ad;
                        staffLeave.AnnaulLeaveBalance = staffLeave.AnnaulLeaveAccrued + staffLeave.AnnaulLeaveOutanding - staffLeave.AnnaulLeaveAdvanced;
                        staffLeave.SickLeaveBalance = sick_bal;

                        staffLeaveList.Add(staffLeave);
                    }
                    else
                    {
                        if (code == staffCode) {
                            PayrollStaffLeave staffLeave = new PayrollStaffLeave();
                            staffLeave.StaffCode = code;
                            staffLeave.AnnaulLeaveAccrued = hol_cy_d;
                            staffLeave.AnnaulLeaveOutanding = Convert.ToDouble(hol_bf_d2 + hol_bf_d);
                            staffLeave.AnnaulLeaveAdvanced = hol_cy_ad;
                            staffLeave.AnnaulLeaveBalance = staffLeave.AnnaulLeaveAccrued + staffLeave.AnnaulLeaveOutanding - staffLeave.AnnaulLeaveAdvanced;
                            staffLeave.SickLeaveBalance = sick_bal;

                            staffLeaveList.Add(staffLeave);

                            break;
                        }
                    }                    
                }
            }

            return staffLeaveList;
        }

        [HttpGet]
        public StaffsResult Get()
        {
            StaffsResult result = new StaffsResult();

            var leaveList = GetStaffLeave(-1);

            result.Code = 200;
            result.Msg = "Success";
            result.Leave = leaveList;

            return result;
        }
      
        [HttpGet("{id}")]
        public StaffResult Get(int id)
        {
            StaffResult result = new StaffResult();

            if (id <= 0)
            {
                result.Code = 404;
                result.Msg = "No Row Found";
                result.Leave = null;

                return result;
            }

            var leaveList = GetStaffLeave(id);

            if (leaveList.Count == 0)
            {
                result.Code = 404;
                result.Msg = "No Row Found";
                result.Leave = null;

                return result;
            }

            if (leaveList.Count > 1)
            {
                result.Code = 500;
                result.Msg = "More than one row found";
                result.Leave = null;

                return result;
            }

            result.Code = 200;
            result.Msg = "Success";
            result.Leave = leaveList[0];

            return result;
        }
        
    }
}
