using System.Diagnostics;
using System;
using System.IO.Ports;
using System.Data;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Nodes;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualBasic.ApplicationServices;
using System.Text;
using System.Reflection;


namespace JhikimikiNew
{
    public partial class Form1 : Form
    {
        private DataTable dataTable;

        private bool editingZeroRow = false;
        private bool addingRow = false;
        private bool fromFile = false;

        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form1_Load);
            // Attach event handlers
            dataGridView1.RowValidating += DataGridView1_RowValidating;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initTable();
        }

        private void initTable()
        {
            fromFile = false;
            // Specify the path to your JSON file
            //string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "template", "template.json");
            string json = @"
        [
            {
                ""columns"": [ ""T"", ""R"", ""K1_A"", ""K1_AR"", ""K2_A"", ""K2_AR"", ""K3_A"", ""K3_AR"", ""K4_A"", ""K4_AR"", ""K5_A"", ""K5_AR"", ""K6_A"", ""K6_AR"" ],
                ""R_length"": 64
            }
        ]";
            //Debug.WriteLine(jsonFilePath);

            // Read JSON from the file
            //string json = File.ReadAllText(jsonFilePath);

            // Parse JSON
            JArray jsonArray = JArray.Parse(json);
            JObject jsonObject = jsonArray[0].ToObject<JObject>();

            // Extract columns and R_length
            List<string> columns = jsonObject["columns"].ToObject<List<string>>();
            int rLength = jsonObject["R_length"].ToObject<int>();

            // Add columns to DataGridView
            foreach (string column in columns)
            {
                if (column == "R")
                {
                    for (int i = 0; i < rLength; i++)
                    {
                        dataGridView1.Columns.Add($"{column}_{rLength - i}", $"{column}_{rLength - i}");
                    }
                }
                else
                {
                    dataGridView1.Columns.Add(column, column);
                }
            }
            dataGridView1.Columns["T"].Frozen = true;
        }

        private void DataGridView1_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            Debug.WriteLine("Validation Triggred");
            Debug.WriteLine(dataGridView1.IsCurrentRowDirty);
            Debug.WriteLine(dataGridView1.CurrentRow != null);
            Debug.WriteLine(dataGridView1.CurrentRow.Index == e.RowIndex);
            // Fill the new row with zeros when editing a row (if it's a new row)
            if (dataGridView1.IsCurrentRowDirty && dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Index == e.RowIndex)
            {
                foreach (DataGridViewCell cell in dataGridView1.Rows[e.RowIndex].Cells)
                {

                    if (fromFile == false && cell.Value == null)
                    {
                        cell.Value = 0;
                    }

                    else if (fromFile == true && cell.Value.ToString().Length == 0)
                    {
                        cell.Value = 0;
                    }
                }
            }
        }

        private void butScanCOM_click(object sender, EventArgs e)
        {
            // Get an array of available COM ports
            string[] availablePorts = SerialPort.GetPortNames();

            // Check if any ports are available
            if (availablePorts.Length > 0)
            {
                Debug.WriteLine("Available COM Ports:");

                // Display the list of ports
                foreach (string port in availablePorts)
                {
                    //Debug.WriteLine(port);

                    comboBox1.Items.Add(port);
                }
            }
            else
            {
                Debug.WriteLine("No COM Ports are available.");
            }
        }

        private SerialPort serialPort;

        private void butConnect_click(object sender, EventArgs e)
        {
            try
            {
                // Check if a COM port is selected
                if (comboBox1.SelectedItem != null)
                {
                    // Get the selected COM port
                    string selectedPort = comboBox1.SelectedItem.ToString();

                    // If the serial port is not already open, open it
                    if (serialPort == null || !serialPort.IsOpen)
                    {
                        serialPort = new SerialPort(selectedPort, 9600); // Specify the baud rate as needed
                        serialPort.Open();

                        // Now the serial port is open and connected
                        MessageBox.Show($"Connected to {selectedPort}");
                        serialPort.Write("ping\r"); // Write data into the comp port
                        //string data = serialPort.ReadLine();

                        lblCOMStatus.Text = "Connected"; lblCOMStatus.Refresh();
                        //Debug.Write("hello");
                        //if (data != null && data == "pong") 
                        //{
                        //    lblCOMStatus.Text = "Connected"; lblCOMStatus.Refresh();
                        //}

                        //else if(data != null && data != "pong")
                        //{
                        //    lblCOMStatus.Text = "Unknown Device"; lblCOMStatus.Refresh();
                        //}

                        //else if(data == null)
                        //{
                        //    lblCOMStatus.Text = "Invalid response"; lblCOMStatus.Refresh();
                        //}

                        //else
                        //{
                        //    lblCOMStatus.Text = "Connection failed"; lblCOMStatus.Refresh();
                        //}

                        butConnect.Enabled = false;
                        butDisconnect.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Serial port is already open.");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a COM port.");
                }

            }

            catch
            {
                MessageBox.Show("Something went wrong");
            }
        }

        private void butDisconnect_click(object sender, EventArgs e)
        {
            string selectedPort = comboBox1.SelectedItem.ToString();
            if (serialPort.IsOpen)
            {
                serialPort.Close();

                MessageBox.Show($"Disconnected from {selectedPort}");
                lblCOMStatus.Text = "Not Connected"; lblCOMStatus.Refresh();
                butConnect.Enabled = true;
                butDisconnect.Enabled = false;
            }
        }

        private void butAdd_click(object sender, EventArgs e)
        {

        }

        private void butLoad_click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file name
                string filePath = openFileDialog.FileName;

                // Read the content from the selected file
                try
                {
                    JArray jsonArray = new JArray();

                    using StreamReader reader = new StreamReader(filePath);
                    // Read the entire content
                    string fileContent = reader.ReadToEnd();

                    // Split the content into lines
                    string[] lines = fileContent.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    //Debug.WriteLine(lines);
                    if (lines.Length > 0)
                    {
                        foreach (string line in lines)
                        {
                            // Parse the JSON object from the line
                            JObject jObject = JsonConvert.DeserializeObject<JObject>(line);

                            // Append the parsed JSON object to the JArray
                            jsonArray.Add(jObject);
                        }
                    }
                    // Access the first line
                    JObject jsonObject = (JObject)jsonArray[0];

                    Debug.WriteLine(jsonObject);

                    // Clear all rows
                    dataGridView1.Rows.Clear();

                    // Clear all columns
                    dataGridView1.Columns.Clear();

                    createTableFromFile(jsonObject);

                    // Process the JSON object and add it to the DataGridView
                    ProcessJsonPacket(jsonArray);
                    fromFile = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }

        }

        private void butCreate_click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.DataSource = null;

                // Clear all rows
                dataGridView1.Rows.Clear();

                // Clear all columns
                dataGridView1.Columns.Clear();

                initTable();
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void createTableFromFile(JObject data)
        {
            IList<string> keys = data.Properties().Select(p => p.Name).ToList();

            // Create a DataTable
            dataTable = new DataTable();

            foreach (string key in keys)
            {
                if (key == "T")
                {
                    dataTable.Columns.Add(key, typeof(float));
                }
                else if (key == "R")
                {
                    var headerValue = data.GetValue("R_NAME");
                    var rValue = data.GetValue(key);
                    List<System.String> headerList = headerValue?.ToObject<List<System.String>>() ?? new List<System.String>();

                    if (rValue != null)
                    {
                        string rString = rValue.ToString();

                        for (int i = 0; i < rValue?.ToString().Length; i++)
                        {
                            if (headerList[i]?.Length > 0)
                            {
                                dataTable.Columns.Add(headerList[i], typeof(string));
                            }
                            else
                            {
                                dataTable.Columns.Add("R_" + (rValue?.ToString().Length - i).ToString(), typeof(string));
                            }

                            char rChar = rString[i];
                            //dataTable.Rows.Add(rChar);
                        }
                    }

                    //Debug.Write(rValue.ToString());
                    Debug.Write(headerList.ToString());
                }

                else if (key == "R_NAME")
                {
                }

                else
                {
                    JObject? table_row_object = data.GetValue(key) as JObject;

                    if (table_row_object != null)
                    {
                        IList<string> table_keys = table_row_object.Properties().Select(p => p.Name).ToList();
                        Debug.Write(table_keys.ToString());
                        foreach (string table_row_key in table_keys)
                        {
                            dataTable.Columns.Add(key + "_" + table_row_key, typeof(int));
                        }
                    }

                }
            }

            dataGridView1.DataSource = dataTable;

        }

        private void ProcessJsonPacket(JArray jarray)
        {
            try
            {
                JArray rNameArray = (JArray)((JObject)jarray[0])["R_NAME"];
                string[] rNameStrings = rNameArray.Select(rNameValue => rNameValue.ToString()).ToArray();
                // Iterate over each element in the JArray
                foreach (JObject json in jarray)
                {
                    Debug.Write(json.ToString());
                    // Create a new DataRow
                    DataRow newRow = dataTable.NewRow();

                    // Extract values from JSON
                    double tValue = json.Value<double>("T");
                    string rValue = json.Value<string>("R");



                    // Handle K values dynamically
                    foreach (var kvp in json)
                    {
                        if (kvp.Key.StartsWith("K"))
                        {
                            JObject kObject = kvp.Value as JObject;
                            if (kObject != null)
                            {
                                int kAValue = kObject.Value<int>("A");
                                int kArValue = kObject.Value<int>("AR");

                                // Add K values to the DataRow dynamically
                                newRow[kvp.Key.ToString() + "_A"] = kAValue;
                                newRow[kvp.Key.ToString() + "_AR"] = kArValue;
                            }
                        }
                    }

                    // Add values to DataGridView
                    //DataGridViewRow row = new DataGridViewRow();

                    // Add values to DataRow
                    newRow["T"] = json.Value<double>("T");
                    //Debug.WriteLine(string.Join(", ", rNameStrings));

                    // Add individual digits of R value to columns
                    for (int i = 0; i < rValue.Length; i++)
                    {
                        if (rNameStrings[i]?.Length > 0)
                        {
                            //Debug.Write(rNameArray[i]);
                            Debug.WriteLine("R_" + rNameStrings[i]);
                            newRow[rNameStrings[i]] = int.Parse(json.Value<string>("R")[i].ToString());
                        }
                        else
                        {
                            Debug.WriteLine("R_" + (rValue.Length - i).ToString());
                            newRow["R_" + (rValue.Length - i).ToString()] = int.Parse(json.Value<string>("R")[i].ToString());
                        }
                    }

                    // Add the DataRow to the DataTable
                    dataTable.Rows.Add(newRow);

                    // Update the DataGridView with the modified DataTable
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dataTable;
                    dataGridView1.Columns["T"].Frozen = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing JSON packet: " + ex.Message);
            }
        }

        private void butSave_click(object sender, EventArgs e)
        {
            //dumpTableDataToFile("saveFile.txt");
            //SaveDataToFile("saveFile.txt");

            try
            {
                // Create a SaveFileDialog
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    // Set the default file name and extension
                    saveFileDialog.FileName = "output.txt";
                    saveFileDialog.DefaultExt = ".txt";

                    // Set the filter for the file extension and the file type
                    saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

                    // Show the SaveFileDialog
                    DialogResult result = saveFileDialog.ShowDialog();

                    // If the user clicked OK in the dialog
                    if (result == DialogResult.OK)
                    {
                        // Get the selected file name
                        string fileName = saveFileDialog.FileName;

                        // Call the function to save data to the selected file
                        dumpTableDataToFile(fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool dumpTableDataToFile(string filePath)
        {
            try
            {
                //string baseDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                //string filePath = Path.Combine(baseDirectory, fileName);

                //Debug.Write(filePath);

                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.ASCII))
                {
                    // Iterate through each row
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        // Skip the last empty row if it's not a new row
                        if (!row.IsNewRow)
                        {

                            // Create a dictionary to store the row data
                            Dictionary<string, object> rowData = new Dictionary<string, object>();

                            // Create a list to store R_NAME entries
                            List<string> rNameData = new List<string>();

                            // Create a StringBuilder to concatenate R values
                            StringBuilder rValueBuilder = new StringBuilder();

                            // Iterate through each cell in the row
                            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                            {
                                // Access the column name
                                string columnName = dataGridView1.Columns[i].HeaderText;

                                // Access the cell value
                                object cellValue = row.Cells[i].Value;

                                // Check if the column name starts with "R"
                                if (columnName.StartsWith("R"))
                                {
                                    // For R-related values, concatenate them into a single string
                                    rValueBuilder.Append(cellValue);

                                    // Add other column name to the R_NAME list
                                    rNameData.Add(columnName);
                                }

                                // Check if the column name starts with "K"
                                else if (columnName.StartsWith("K"))
                                {
                                    // Extract K-related values 
                                    string[] kParts = columnName.Split('_');
                                    string kName = kParts[0];
                                    string kType = kParts[1];

                                    // Create or update the K dictionary
                                    if (!rowData.ContainsKey(kName))
                                        rowData[kName] = new Dictionary<string, object>();

                                    ((Dictionary<string, object>)rowData[kName])[kType] = cellValue;
                                }

                                else
                                {

                                    // Add column name and value to the dictionary
                                    rowData[columnName] = cellValue;
                                }

                            }

                            // Add the concatenated R value to the dictionary
                            rowData["R"] = rValueBuilder.ToString();

                            // Add the R_NAME list to the dictionary
                            rowData["R_NAME"] = rNameData;

                            // Convert the dictionary to a JSON string
                            string jsonRow = JsonConvert.SerializeObject(rowData);

                            // Write the JSON string to the file
                            writer.WriteLine(jsonRow);
                            //serialPort.Write(jsonRow + "\r");
                        }
                    }
                }

                MessageBox.Show("Data has been successfully saved to the file.", "Save Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data to file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return true;
        }

        private void butPopulate_click(object sender, EventArgs e)
        {
            // Get the input string from the TextBox
            string initialInputString = textBox1.Text;

            // Ensure the input string is exactly 64 characters long
            if (initialInputString.Length != 64)
            {
                MessageBox.Show("Input must be a 64-digit integer.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Concatenate a null value at the start of the input string
            string inputString = "\0" + initialInputString;

            // Create a new DataGridViewRow
            DataGridViewRow newRow = new DataGridViewRow();

            // Iterate through each character in the input string and add cells to the new row
            for (int i = 0; i < Math.Min(inputString.Length, dataGridView1.Columns.Count - 1); i++)
            {
                // Assuming your DataGridView has columns starting from the 2nd column
                int columnIndex = i + 1;

                // Create a new cell and set its value
                DataGridViewCell newCell = new DataGridViewTextBoxCell();
                newCell.Value = inputString[i].ToString();

                // Add the cell to the new row
                newRow.Cells.Add(newCell);
            }

            // Add the new row to the DataGridView
            dataGridView1.Rows.Add(newRow);
        }

        private void butSend_click(object sender, EventArgs e)
        {
            try
            {
                //string baseDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                //string filePath = Path.Combine(baseDirectory, fileName);

                //Debug.Write(filePath);

                // Iterate through each row
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Skip the last empty row if it's not a new row
                    if (!row.IsNewRow)
                    {

                        // Create a dictionary to store the row data
                        Dictionary<string, object> rowData = new Dictionary<string, object>();

                        // Create a list to store R_NAME entries
                        List<string> rNameData = new List<string>();

                        // Create a StringBuilder to concatenate R values
                        StringBuilder rValueBuilder = new StringBuilder();

                        // Iterate through each cell in the row
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            // Access the column name
                            string columnName = dataGridView1.Columns[i].HeaderText;

                            // Access the cell value
                            object cellValue = row.Cells[i].Value;

                            // Check if the column name starts with "K"
                            if (columnName.StartsWith("K"))
                            {
                                // Extract K-related values 
                                string[] kParts = columnName.Split('_');
                                string kName = kParts[0];
                                string kType = kParts[1];

                                // Create or update the K dictionary
                                if (!rowData.ContainsKey(kName))
                                    rowData[kName] = new Dictionary<string, object>();

                                ((Dictionary<string, object>)rowData[kName])[kType] = cellValue;
                            }

                            // Check if the column name starts with "R"
                            else if (columnName.StartsWith("R"))
                            {
                                // For R-related values, concatenate them into a single string
                                rValueBuilder.Append(cellValue);

                                // Add other column name to the R_NAME list
                                rNameData.Add(columnName);
                            }

                            else
                            {

                                // Add column name and value to the dictionary
                                rowData[columnName] = cellValue;
                            }
                        }

                        // Add the concatenated R value to the dictionary
                        rowData["R"] = rValueBuilder.ToString();

                        // Add the R_NAME list to the dictionary
                        rowData["R_NAME"] = rNameData;

                        // Convert the dictionary to a JSON string
                        string jsonRow = JsonConvert.SerializeObject(rowData);

                        // Write the JSON string to the file
                        //writer.WriteLine(jsonRow);
                        serialPort.Write(jsonRow + "\r");
                    }
                }

                MessageBox.Show("Data has been successfully sent to serial port.", "Save Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data to file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void butDelRow_click(object sender, EventArgs e)
        {
            // Check if there is at least one row and a row is selected
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Confirm deletion if necessary
                DialogResult result = MessageBox.Show("Are you sure you want to delete this row?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Delete the selected row
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void butAddRow_click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource != null && dataGridView1.DataSource is DataTable)
            {
                DataTable dataTable = (DataTable)dataGridView1.DataSource;

                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int selectedIndex = dataGridView1.SelectedRows[0].Index;

                    // Add a new row to the DataTable
                    DataRow newRow = dataTable.NewRow();
                    dataTable.Rows.InsertAt(newRow, selectedIndex + 1);
                }
                else
                {
                    // Add a new row at the end of the DataTable
                    DataRow newRow = dataTable.NewRow();
                    dataTable.Rows.Add(newRow);
                }

                // Refresh the DataGridView
                dataGridView1.Refresh();
            }
            else
            {
                // Get the index of the selected row
                int selectedIndex = dataGridView1.SelectedCells[0].RowIndex;

                // Get the number of columns in the DataGridView
                int columnCount = dataGridView1.Columns.Count;

                // Create an array to store zero values for each column
                object[] rowData = new object[columnCount];

                // Fill the array with zero values
                for (int i = 0; i < columnCount; i++)
                {
                    rowData[i] = 0;
                }

                // Insert a new row before the selected row with zero values
                dataGridView1.Rows.Insert(selectedIndex, rowData);
            }
        }
    }
}
