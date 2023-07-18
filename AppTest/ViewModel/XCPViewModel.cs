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
            editForm.Tag = this.Form;
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                XCPSignals = editForm.FormItem.XCPSingals;
                Form.CanChannel = editForm.FormItem.CanChannel;
                //ReLoadSignal();
                return true;
            }
            return false;
        }

        public override void ShowSignalDetails(DataGridViewRow row)
        {
            try
            {
                var signal = row.DataBoundItem as XCPSignal;
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
