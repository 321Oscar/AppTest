using AppTest.FormType;
using AppTest.Model;
using System;
using System.Windows.Forms;

namespace AppTest.ViewModel
{
    public abstract class XCPViewModel: BaseViewModel
    {
        public XCPSignals XCPSignals;

        public XCPModule XcpModule;

        public override bool ModifiedSignals()
        {
            AddNewForm editForm;
            editForm = new AddNewXcpForm(Form.OwnerProject, Form.OwnerProject.Form.Find(x => x.Name == Form.Name));
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                XCPSignals = editForm.FormItem.XCPSingals;
                //ReLoadSignal();
                return true;
            }
            return false;
        }

        public override void ShowSignalDetails(DataGridView dataGridView,DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                var signal = dataGridView.Rows[e.RowIndex].DataBoundItem as XCPSignal;
                SignalItemForm<XCPSignal> siF = new SignalItemForm<XCPSignal>(signal, signal.SignalName);
                siF.Show();
            }
            catch (Exception ex)
            {
                ShowLog?.Invoke(ex.Message);
            }
        }
    }
}
