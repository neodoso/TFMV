using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TFMV.UserControls
{
    public partial class ModelSelect : Form
    {

        public List<string> selected_models { get; set; }


        public ModelSelect(List<string> models)
        {
            InitializeComponent();

            for (int i = 0; i < models.Count; i++)
            {
                listBox_models.Items.Add(models[i]);
            }

            selected_models = new List<string>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox_models.SelectedItems.Count == 0) { MessageBox.Show("Select one or multiple models."); return; }

            foreach (var item in listBox_models.SelectedItems)
            {
                selected_models.Add(item.ToString());
            }

            this.Close();
        }

        private void listBox_models_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            // List<string> models = new List<string>();
            foreach (var item in listBox_models.SelectedItems)
            {
                selected_models.Add(item.ToString());
            }

            this.Close();
        }
    }
}
