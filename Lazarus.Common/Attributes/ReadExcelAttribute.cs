using System;

namespace Lazarus.Common.Attributes
{
    public class ReadExcelAttribute
    {
        [AttributeUsage(AttributeTargets.All)]
        public class Column : Attribute
        {
            public int ColumnIndex { get; set; }

            public Column(int column)
            {
                ColumnIndex = column;
            }
        }
    }
}
