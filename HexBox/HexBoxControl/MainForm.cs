using System;
using System.IO;
using System.Windows.Forms;
using HexBoxControl;


namespace HexBoxExample
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Mode.Items.AddRange(Enum.GetNames(typeof(HexBoxViewMode)));
            Mode.SelectedIndex = 1;

            Enable.Checked = true;
            Columns.Value  = DumpBox.Columns;

            EncodingSelect.Items.AddRange
            (
                new object[]
                {
                    new AnsiCharConvertor(),
                    new AsciiCharConvertor(),
                    new Utf8CharConvertor(),
                    new Cp1251CharConvertor()
                }
            );
            EncodingSelect.SelectedIndex = 0;

            byte[] dump = new byte[256];
            for (int i = 0; i < dump.Length; i++)
            {
                dump[i] = (byte)i;
            }
            DumpBox.Dump = dump;
        }

        private void DumpBoxColumnsChanged(object sender, EventArgs e) => Columns.Value = DumpBox.Columns;

		private void ModeSelectedIndexChanged(object sender, EventArgs e)
        {
            Enum.TryParse(Mode.SelectedItem.ToString(), out HexBoxViewMode mode);
            DumpBox.ViewMode = mode;
        }

        private void AutoSizeCheckedChanged(object sender, EventArgs e) => DumpBox.ColumnsAuto = AutoSize.Checked;

		private void EnableCheckedChanged(object sender, EventArgs e) => DumpBox.Enabled = Enable.Checked;

		private void ColumnsValueChanged(object sender, EventArgs e)
        {
            if (!DumpBox.ColumnsAuto)
            {
                DumpBox.Columns = Convert.ToInt32(Columns.Value);
            }
        }

        private void EncodingSelectSelectedIndexChanged(object sender, EventArgs e) => DumpBox.CharConverter = EncodingSelect.SelectedItem as ICharConverter;

		private void OpenFileClick(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DumpBox.Dump = File.ReadAllBytes(dialog.FileName);
            }
        }
    }
}