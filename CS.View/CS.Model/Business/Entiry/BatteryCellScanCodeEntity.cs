using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class BatteryCellScanCodeEntity
    {
        public long Id { get; set; }

        public string BatteryCellCode { get; set; }

        public bool HasCode { get; set; }

        public ushort IsMatched { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
