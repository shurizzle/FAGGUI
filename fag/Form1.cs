using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using fag.api;
using fag.api.types;

namespace fag
{
  public partial class FagMain : Form
  {
    private Base api;
    bool keySkip = false;

    public FagMain()
    {
      api = new Base();

      InitializeComponent();

      expression.KeyDown += new KeyEventHandler(expression_KeyDown);
      expression.KeyPress += new KeyPressEventHandler(expression_KeyPress); 
      this.FlowsList.ItemActivate += new EventHandler(FlowsList_ItemActivate);

      searchFlows();
    }

    private void expression_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return)
        keySkip = true;
      else
        keySkip = false;
    }

    private void expression_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (keySkip)
      {
        e.Handled = true;
        searchFlows();
      }
    }

    private void FlowsList_ItemActivate(object sender, EventArgs e)
    {
      ListView item = sender as ListView;
      Drop[] drops = api.GetFlow(Convert.ToInt32(item.SelectedItems[0].Text)).Drops;
      MessageBox.Show(drops.Length.ToString());
    }

    private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
    {
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
    }

    private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
    {
    }

    private void DropsList_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void searchFlows()
    {
      string search = expression.Text;
      search = search.Length == 0 ? "*" : search;
      expression.Enabled = false;
      FlowsList.UseWaitCursor = true;
      FlowsList.Items.Clear();
      FlowsList.Items.AddRange(Helpers.Convert(api.GetFlows(search, 0, 10)));
      expression.Enabled = true;
      FlowsList.UseWaitCursor = false;
    }
  }
}
