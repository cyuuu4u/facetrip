using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Data;
using xxdwunity.engine;
using xxdwunity.util;

namespace xxdwunity.warehouse
{
    public class CsvSheet
    {
        private bool ready = false;
        public bool Ready
        {
            get
            {
                return this.ready;
            }
        }

        private bool firstRowAsTitle;

        private string name;
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        private int rowNum;
        public int RowNum
        {
            get
            {
                return this.rowNum;
            }
        }

        private int colNum;
        public int ColNum
        {
            get
            {
                return this.rowNum;
            }
        }

        public int CellNum
        {
            get
            {
                return this.rowNum * this.colNum;
            }
        }

        private List<List<string>> grid;
        private DataTable dt;

        public string GetCell(int r, int c)
        {
            if (r < 0 || r >= this.rowNum || c < 0 || c >= this.colNum)
                return "";

            List<string> row = this.grid[this.firstRowAsTitle ? r + 1 : r];
            if (c > row.Count)
                return "";

            return row[c];
        }

        public string this[int r, int c]
        {
            get { return GetCell(r, c); }
        }

        public DataTable Data
        {
            get
            {
                if (this.dt != null) return this.dt;

                this.dt = new DataTable();
                this.dt.TableName = this.name;

                //增加列
                if (this.firstRowAsTitle && this.grid.Count > 0)
                {
                    for (int i = 0; i < this.colNum; i++)
                    {
                        try
                        {
                            List<string> row = this.grid[0];
                            if (i < row.Count)
                            {
                                this.dt.Columns.Add(row[i]);
                            }
                            else
                            {
                                this.dt.Columns.Add(i.ToString());
                            }
                        }
                        catch (DuplicateNameException)
                        {
                            XxdwDebugger.Log("Duplicate column name.");
                            AsyncNotification.Instance.PostNotification(this, Notification.NOTI_SET_HINT, Parameters.STR_CSV_DUPLICATE_TITLE);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < this.colNum; i++)
                    {
                        this.dt.Columns.Add(i.ToString());
                    }
                }

                // 添加数据行
                for (int c = 1; c < this.grid.Count; c++)
                {
                    List<string> row = this.grid[c];

                    DataRow dr = this.dt.NewRow();
                    for (int i = 0; i < this.colNum; i++)
                    {
                        dr[i] = i < row.Count ? row[i] : "";
                    }
                    this.dt.Rows.Add(dr);
                }

                return this.dt;
            }
        }

        private string csvText;          // 地图数据XML文本
        Thread dataThread;               // 声明线程

        /// <summary>
        /// 创建CsvSheet.
        /// </summary>
        public CsvSheet()
        {
            this.ready = false;
            this.firstRowAsTitle = true;
            this.rowNum = 0;
            this.colNum = 0;
            this.grid = new List<List<string>>();
            this.dt = null;
        }

        public void LoadData(string name, string csvText, bool firstRowAsTitle = true)
        {
            if (!this.ready)
            {
                this.firstRowAsTitle = firstRowAsTitle;
                this.name = name;
                this.csvText = csvText;
                this.dataThread = new Thread(new ThreadStart(DataThreadFunc));
                this.dataThread.Start();
            }
        }

        private void DataThreadFunc()
        {
            try
            {
                StringReader sr = new StringReader(this.csvText);

                List<string> row = new List<string>();
                int quotaNumNotMatched = 0;
                bool previousQuota = false;
                string cell = "";
                string line;
                while ((line = sr.ReadLine()) != null)  // 返回的串不包括“\r\n”
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        char ch = line[i];
                        if (ch == '\"')
                        {
                            if (previousQuota)      // 两个引号，忽略前一个，加后一个
                            {
                                cell += ch;
                                previousQuota = false;
                                quotaNumNotMatched--;
                            }
                            else if (quotaNumNotMatched > 0
                                && (i == line.Length - 1 || line[i + 1] != '\"')) // 引用结束
                            {
                                quotaNumNotMatched--;
                            }
                            else
                            {
                                previousQuota = true;
                                quotaNumNotMatched++;
                            }
                        }
                        else if (ch == ',')
                        {
                            if (quotaNumNotMatched > 0)
                            {
                                cell += ch;
                            }
                            else
                            {
                                row.Add(cell.Trim());
                                cell = "";
                            }
                            previousQuota = false;
                        }
                        else
                        {
                            cell += ch;
                            previousQuota = false;
                        }
                    } // for

                    if (quotaNumNotMatched == 0)
                    {
                        cell = cell.Trim();
                        if (cell.Length > 0)
                        {
                            row.Add(cell);
                            cell = "";
                        }

                        bool emptyRow = true;
                        foreach (string cont in row)
                        {
                            if (cont != "")
                            {
                                emptyRow = false;
                                break;
                            }
                        }

                        if (!emptyRow)  // 排除空行
                        {
                            if (row.Count > this.colNum)
                            {
                                this.colNum = row.Count;
                            }
                            this.grid.Add(row);
                        }

                        row = new List<string>();
                    }
                    else
                    {
                        cell += "\n";   // 单元格中包含换行符
                    }
                } // while
                this.rowNum = this.grid.Count - (this.firstRowAsTitle ? 1 : 0);
                sr.Close();

                this.ready = true;
                XxdwDebugger.Log("CsvSheet: DataThreadFunc() post NOTI_MODEL_DATA_READY.");
                AsyncNotification.Instance.PostNotification(this, Notification.NOTI_MODEL_DATA_READY, this.name);
            }
            catch (System.Exception ex)
            {
                XxdwDebugger.LogError(ex.Message);
            }
        }
    }
}

