using System;
using System.Data;

namespace DTO // 👈 PHẢI LÀ DTO
{
    public class Bill
    {
        public int ID { get; set; }
        public DateTime? DateCheckIn { get; set; }
        public DateTime? DateCheckOut { get; set; }
        public int Status { get; set; }

        public Bill(int id, DateTime? dateCheckin, DateTime? dateCheckOut, int status)
        {
            this.ID = id;
            this.DateCheckIn = dateCheckin;
            this.DateCheckOut = dateCheckOut;
            this.Status = status;
        }


        public Bill(DataRow row)
        {
            this.ID = (int)row["MaHD"];
            this.DateCheckIn = (DateTime?)row["NgayLap"];
            var dateCheckOutTemp = row["NgayRa"];
            if (dateCheckOutTemp.ToString() != "")
                this.DateCheckOut = (DateTime?)dateCheckOutTemp;
            this.Status = (int)row["TrangThai"];
        }
    }
}