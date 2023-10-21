using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using MySqlX.XDevAPI.Relational;

namespace FileEditor.Classes
{
    public class ChangeDataByModerator
    {
        private DBManager dbManager;
        private List<UserInfo> userInfos = new List<UserInfo>();
        private List<List<object>> roleTabel;
        private List<List<object>> Table;
        private DataGrid dataGrid;
        private int rowPosition;
        private TextBox[] textBoxes;
        private ComboBox comboBox;
        private UserInfo userInfo;

        public ChangeDataByModerator(DataGrid dataGrid, TextBox[]textBoxes, ComboBox comboBox)
        {
            dbManager = DBManager.getInstance("");
            this.dataGrid = dataGrid;
            this.textBoxes = textBoxes;
            this.comboBox = comboBox;
            dataGrid.SelectionChanged += DGUserInfo_OnSelectionChanged;
            comboBox.SelectionChanged += ComboBox_SelectionChanged;
            roleTabel = dbManager.selectAll("UsersRole");
            for (int i = 0; i < roleTabel.Count; i++)
            {
                comboBox.Items.Add(roleTabel[i][1]);
            }

        }

        public void Reconect()
        {
            dataGrid.ItemsSource = null;
            dataGrid.Items.Clear();
            dataGrid.Items.Refresh();
            LoadInGrid();
        }

        private void LoadInGrid()
        {
             Table = dbManager.selectAll("UserTable");
            userInfos.Clear();

            for (int i = 0; i < Table.Count; i++)
            {
                if (Table[i][1] is int number)
                {
                    string roleName = roleTabel.FirstOrDefault(item => (int)item[0] == number)?.ElementAt(1).ToString();
                    if (roleName != null)
                    {
                        Table[i][1] = roleName;
                    }
                }
            }

            foreach (var VARIABLE in Table)
            {
                userInfos.Add(new UserInfo
                {
                    Name = VARIABLE[0].ToString(),
                    Email = VARIABLE[3].ToString(),
                    Position = VARIABLE[2].ToString(),
                    Role = VARIABLE[1].ToString()
                });
            }
            dataGrid.ItemsSource = userInfos;
        }
        private void DGUserInfo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int columnIndex = 3;
            rowPosition = ((DataGrid)sender).SelectedIndex;
            if (rowPosition >= 0 && columnIndex >= 0 && rowPosition < dataGrid.Items.Count)
            {
                DataGridCellInfo cellInfo = new DataGridCellInfo(dataGrid.Items[rowPosition], dataGrid.Columns[columnIndex]);
                userInfo = ((UserInfo)cellInfo.Item);
                textBoxes[0].Text = userInfo.Name;
                textBoxes[1].Text = userInfo.Email;
                textBoxes[2].Text = userInfo.Position;
                comboBox.SelectedItem = userInfo.Role;
            }
        }

        public void SearchInGrid(string searchTerm)
        {
            var filteredItems = userInfos
                .Where(item => item.Email.ToLower().Contains(searchTerm.ToLower()) ||
                               item.Name.ToLower().Contains(searchTerm.ToLower())||
                               item.Position.ToLower().Contains(searchTerm.ToLower()))
                .ToList();

            dataGrid.ItemsSource = filteredItems;
        }

        public void DeleteRecord(string TableName)
        {
            dbManager.delete(TableName, $"Mail = '{userInfo.Email}'");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dbManager.Update("UserTable", "UserRole", comboBox.SelectedIndex+1, $"Mail = '{textBoxes[1].Text}'");
            userInfos[rowPosition].Role = comboBox.SelectedItem.ToString();
            dataGrid.Items.Refresh();
        }
    }
}
