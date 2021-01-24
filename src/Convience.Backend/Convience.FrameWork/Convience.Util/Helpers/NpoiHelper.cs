using NPOI.HSSF.UserModel;

using System.Collections.Generic;
using System.IO;

namespace Convience.Util.Helpers
{
    public class NpoiHelper
    {
        /// <summary>
        /// 导出excel
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dataList">数据列表</param>
        /// <param name="dic">对象属性和中文对照</param>
        /// <param name="sheetName">页名</param>
        /// <returns></returns>
        public MemoryStream ExportExcel<T>(
            List<T> dataList,
            Dictionary<string, string> dic,
            string sheetName = "")
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet(sheetName);

            // 行下标
            var rowNum = 1;
            var titleRow = sheet.CreateRow(rowNum);

            // 列下标
            var colNum = 0;

            // 获取属性
            var positionDic = new Dictionary<string, int>();

            // 设置标题
            foreach (var item in dic)
            {
                titleRow.CreateCell(colNum).SetCellValue(item.Value);
                positionDic.Add(item.Value, colNum);
                colNum++;
            }

            // 设置内容
            rowNum++;
            var properties = typeof(T).GetProperties();
            foreach (var item in dataList)
            {
                var row = sheet.CreateRow(rowNum);
                for (int i = 0; i < properties.Length; i++)
                {
                    // 属性字典中没有该属性运行下一个属性
                    if (!dic.ContainsKey(properties[i].Name))
                    {
                        continue;
                    }

                    // 获取属性值
                    var value = properties[i].GetValue(item)?.ToString();

                    //获取对应列下标,并设定值
                    var colIndex = positionDic[dic[properties[i].Name]];
                    row.CreateCell(colIndex).SetCellValue(value);
                }
            }

            var ms = new MemoryStream();
            workbook.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);

            //return File(ms, "application/vns.ms-excel", $"{filename}"); ;
            return ms;
        }
    }
}
