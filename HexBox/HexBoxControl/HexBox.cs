using System;
using System.Drawing;
using System.Windows.Forms;


namespace HexBoxControl
{
    public enum HexBoxViewMode
    {
        Bytes       = 2,
        BytesAscii  = 3,
        Words       = 4,
        DoubleWords = 8,
        QuadWords   = 16
    };
    
    public class HexBoxEditEventArgs : EventArgs
    {
        #region Ctors
        public HexBoxEditEventArgs(long offset, ulong prev, ulong current)
        {
            Offset   = offset;
            OldValue = prev;
            NewValue = current;
        }
		#endregion

		#region Properties
		public long Offset { get; private set; }

        public ulong OldValue { get; private set; }

        public ulong NewValue { get; private set; }
        #endregion
    }
    
    public delegate void HexBoxEditEventHandler(object sender, HexBoxEditEventArgs e);

    public class HexBox : Control
    {
        #region Fields
        private int    _HeaderLeft   = 4;
        private int    _HeaderTop    = 2;
        private int    _ColumnsDelim = 10;
        private int    _CharWidth    = 1;
        private int    _CharHeight   = 1;
        private int    _AddressWidth;
        private int    _DataLeft;
        private int    _DataCellWidth;
        private int    _DataColums   = 8;
        private byte[] _Dump;
        private long   _Lines;
        private long   _DataRows;
        private long   _Offset       = 0;
        private bool   _ColumnsAuto  = false;
        private int    _LineLength;
        private long   _EditIndex    = -1;

        private HexBoxViewMode _ViewMode;
        private ICharConverter _CharConverter;
        private StringFormat   _StringFormat;
        private VScrollBar     _ScrollBar;
        private TextBox        _Edit;
        #endregion

        #region Ctors
        public HexBox()
        {
            _ScrollBar = new VScrollBar();
            _Edit      = new TextBox();

            _ViewMode      = HexBoxViewMode.BytesAscii;
            _CharConverter = new AnsiCharConvertor();
            _StringFormat  = new StringFormat(StringFormat.GenericTypographic);
            _StringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;

            ClientSize = new Size(380, 198);
            
            _ScrollBar.Size     = new Size(16, Height-2);
            _ScrollBar.Location = new Point(Width-(16+1), 1);
            _ScrollBar.Anchor   = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            _ScrollBar.Scroll  += OnScrolling;
            _ScrollBar.Enabled  = false;
            _ScrollBar.Minimum  = 0;
            Controls.Add(_ScrollBar);
            
            _Edit.BorderStyle      = BorderStyle.None;
            _Edit.Size             = new Size(14, 18);
            _Edit.MaxLength        = 2;
            _Edit.Font             = Font;
            _Edit.ForeColor        = Color.FromArgb(0x2C, 0x2C, 0x2C);
            _Edit.BackColor        = Color.LightGray;
            _Edit.ContextMenuStrip = new ContextMenuStrip();
            _Edit.KeyPress        += EditKeyPress;
            Controls.Add(_Edit);
            _Edit.Hide();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            Font = new Font("Consolas", 10, FontStyle.Regular, GraphicsUnit.Point);
        }
        #endregion

        #region Properties
        private int BytesPerCell => (int)_ViewMode >> 1;

        private int HexSymbolsPerCell => (int)_ViewMode & 0x1E;

        public HexBoxViewMode ViewMode
        {
            get => _ViewMode;

            set
            {
                if (value != _ViewMode)
                {
                    EndEditing();

                    _ViewMode = value;

                    _Edit.Width     = _CharWidth * HexSymbolsPerCell;
                    _Edit.MaxLength = HexSymbolsPerCell;
                    _Offset         = 0;

                    Resume();
                    Invalidate();
                }
            }
        }

        public int Columns
        {
            get => _DataColums;

            set
            {
                if (value > 0 && value != _DataColums)
                {
                    _DataColums = value;

                    EndEditing();

                    Resume();
                    Invalidate();
                }
            }
        }

        public long Rows => _Lines;

        public long DisplayedRows => _DataRows;

        public byte[] Dump
        {
            get => _Dump;

            set
            {
                _Dump = value;

                _Offset    = 0;
                _EditIndex = -1;
                _Edit.Hide();

                Resume();
                Invalidate();
            }
        }
        
        public ICharConverter CharConverter
        {
            get => _CharConverter;

            set
            {
                if (value != null && value != _CharConverter)
                {
                    _CharConverter = value;

                    if (_ViewMode == HexBoxViewMode.BytesAscii)
                    {
                        Invalidate();
                    }
                }
            }
        }

        public bool ColumnsAuto
        {
            get => _ColumnsAuto;

            set
            {
                if (value != _ColumnsAuto)
                {
                    _ColumnsAuto = value;

                    EndEditing();

                    Resume();
                    Invalidate();
                }
            }
        }

        public override Font Font
        {
            get => base.Font;

            set
            {
                if (value != base.Font)
                {
                    EndEditing();

                    base.Font  = value;

                    SizeF _CharSize = CreateGraphics().MeasureString("0", Font, 100, _StringFormat);
                    _CharWidth      = (int)_CharSize.Width;
                    _CharHeight     = (int)_CharSize.Height;
                    _AddressWidth   = 9 * _CharWidth;
                    _DataLeft       = _HeaderLeft + _AddressWidth + _ColumnsDelim;

                    _Edit.Font  = value;
                    _Edit.Width = _CharWidth * HexSymbolsPerCell;

                    Resume();
                    Invalidate();
                }          
            }
        }

        public new bool Enabled
        {
            get => base.Enabled;

            set
            {
                if (value != base.Enabled)
                {
                    base.Enabled = value;
                    EndEditing();
                }
            }
        }

        public Size RequiredSize
        {
            get
            {
                int w = _DataLeft + _ScrollBar.Width + _DataColums * _DataCellWidth;
                int h = _HeaderTop + 4 + (int)_DataRows * _CharHeight;

                if (_ViewMode == HexBoxViewMode.BytesAscii)
                {
                    w += _ColumnsDelim + (2 * _DataColums - 1) * _CharWidth;
                }

                return new Size(w, h);
            }
        }
        #endregion

        #region Methods
        public void Fill(long size, ulong value)
        {
            long len = BytesPerCell;

            byte[] dump = new byte[size*len];
            byte[] data = BitConverter.GetBytes(value);

            for (long offset = 0; offset < dump.Length; offset += len)
            {
                Array.Copy(data, 0, dump, offset, len);
            }

            Dump = dump;
        }
        #endregion

        #region Events
        public event EventHandler ColumsChanged;

        public event HexBoxEditEventHandler Edited;
        #endregion

        #region Handlers
        private void OnScrolling(object sender, ScrollEventArgs e) => Scroll();

        private void EditKeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '\r': EndEditing(true); break;
                
                case 'ф':
                case 'Ф':
                case 'a': e.KeyChar = 'A'; break;
                case 'и':
                case 'И':
                case 'b': e.KeyChar = 'B'; break;
                case 'с':
                case 'С':
                case 'c': e.KeyChar = 'C'; break;
                case 'в':
                case 'В':
                case 'd': e.KeyChar = 'D'; break;
                case 'у':
                case 'У':
                case 'e': e.KeyChar = 'E'; break;
                case 'а':
                case 'А':
                case 'f': e.KeyChar = 'F'; break;

                default: e.Handled = "0123456789ABCDEF\b".IndexOf(e.KeyChar) == -1; break;
            }
        }
        #endregion

        #region Utils
        private Brush GetBrush(Color color) => new SolidBrush(Enabled ? color : Color.LightGray);

		private Pen GetPen(Color color) => new Pen(Enabled? color : Color.LightGray);

		private ulong GetCellValue(long index)
        {
            ulong data = 0;
            index += BytesPerCell;

            for (int i = 0; i < BytesPerCell; i++)
            {
                data <<= 8;
                if (--index < _Dump.Length)
                {
                    data |= _Dump[index];
                }
            }

            return data;
        }

        private void SetCellValue(long index, ulong value)
        {
            for (int i = 0; i < BytesPerCell; i++)
            {
                if (index < _Dump.Length)
                {
                    _Dump[index++] = (byte)value;
                    value >>= 8;
                }
                else
                {
                    break;
                }
            }
        }

        private Point GetCellPos(Point p)
        {
            p.X -= _DataLeft;
            p.Y -= _HeaderTop + 4 + _CharHeight;

            if (p.X >= 0 && p.Y >= 0)
            {
                int x = p.X / _DataCellWidth;
                int y = p.Y / _CharHeight;

                if (x < _DataColums && y < _DataRows)
                {
                    return new Point(x, y);
                }
            }

            return new Point(-1, -1);
        }

        private long GetCellIndex(Point p)
        {
            long index = _Offset + (p.Y * _DataColums + p.X) * BytesPerCell;
            return index < _Dump.Length ? index : -1;
        }

        private void Resume()
        {
            int CellBytes = BytesPerCell;

            _DataCellWidth = (2 * CellBytes + 1) * _CharWidth;

            _DataRows = (Height - (_HeaderTop + 4 + _CharHeight/2)) / _CharHeight;
            if (_ColumnsAuto)
            {
                int a = _DataLeft + _ScrollBar.Width - _CharWidth;
                int b = _DataCellWidth;

                if (_ViewMode == HexBoxViewMode.BytesAscii)
                {
                    a += _ColumnsDelim - _CharWidth;
                    b += 2 * _CharWidth;
                }
                
                int colums = Math.Max(1, (Width - a) / b);
                if (colums != _DataColums)
                {
                    _DataColums = colums;

                    ColumsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            _LineLength = CellBytes * _DataColums;
            if (_Dump != null)
            {
                _Lines = (_Dump.Length + (_LineLength - 1)) / _LineLength;

                _ScrollBar.Enabled = (_DataRows - 1) < _Lines;
                if (_ScrollBar.Enabled)
                {
                    _ScrollBar.Maximum = (int)(_Lines - _DataRows + 10);
                }
            }
        }

        private void Scroll()
        {
            int offset = _ScrollBar.Value * _LineLength;

            if (offset != _Offset)
            {
                EndEditing();

                _Offset = offset;
                Invalidate();
            }
        }

        private void EndEditing(bool apply = false)
        {
            if (_EditIndex != -1)
            {
                if (apply)
                {
                    ulong prev  = GetCellValue(_EditIndex);
                    ulong value = Convert.ToUInt64("0" + _Edit.Text, 16);

                    SetCellValue(_EditIndex, value);

                    Edited?.Invoke(this, new HexBoxEditEventArgs(_EditIndex/BytesPerCell, prev, value));

                    Invalidate();
                }

                _Edit.Hide();
                _EditIndex = -1; 
            }
        }
        #endregion

        #region Drawing
        private int DrawCell(Graphics g, Color color, int x, int y, ulong value, int width)
        {
            string image  = value.ToString("X"+width);

            for (int i = 0; i < image.Length; i++, x += _CharWidth)
            {
                g.DrawString(image.Substring(i, 1), Font, GetBrush(color), x, y, _StringFormat);
            }

            return x;
        }

        private void DrawBackground(Graphics g)
        {
            g.Clear(BackColor);

            g.DrawString("-Address-", Font, GetBrush(Color.Gray), _HeaderLeft, _HeaderTop, _StringFormat);

            int x = _DataLeft;

            for (int i = 0; i < _DataColums; i++, x += _CharWidth)
            {
                x = DrawCell(g, Color.Gray, x, _HeaderTop, (ulong)i, HexSymbolsPerCell);
            }

            if (_ViewMode == HexBoxViewMode.BytesAscii)
            {
                int AsciiWidth = 2 * _CharWidth * _DataColums - _CharWidth;

                x = x - _CharWidth + _ColumnsDelim + (AsciiWidth - 5 * _CharWidth) / 2;

                g.DrawString("ASCII", Font, GetBrush(Color.Gray), x, _HeaderTop, _StringFormat);
            }

            int y = _HeaderTop + _CharHeight + 2;
            int w = _DataLeft + _DataCellWidth*_DataColums - _CharWidth;
            if (_ViewMode == HexBoxViewMode.BytesAscii)
            {
                w += _ColumnsDelim + 2 * _CharWidth * _DataColums - _CharWidth;
            }
            g.DrawLine(GetPen(Color.Gray), _HeaderLeft, y, w, y);
        }

        private void DrawLines(Graphics g)
        {
            long offset = _Offset;
            int  y      = _HeaderTop + 4;

            for (long row = 0; row < _DataRows; row++)
            {
                y += _CharHeight;

                int x = DrawCell(g, Color.Blue, _HeaderLeft, y, (ulong)(offset/BytesPerCell), 8);
                g.DrawString(":", Font, GetBrush(Color.Blue), x, y, _StringFormat);

                int xd = _DataLeft;
                int xa = xd + _DataColums*_DataCellWidth-_CharWidth + _ColumnsDelim;

                for (int i = 0; i < _DataColums; i++, xd += _CharWidth)
                {
                    xd = DrawCell(g, Color.Black, xd, y, GetCellValue(offset), HexSymbolsPerCell);

                    if (_ViewMode == HexBoxViewMode.BytesAscii)
                    {
                        char c = _CharConverter.ToChar(_Dump[offset]);
                        
                        if (c == '\0')
                        {
                            g.DrawRectangle(GetPen(Color.DarkMagenta), xa, y+3, _CharWidth, _CharHeight-4);
                        }
                        else
                        {
                            g.DrawString(c.ToString(), Font, GetBrush(Color.DarkMagenta), xa, y, _StringFormat);
                        }
                        
                        xa += 2 * _CharWidth;
                    }

                    offset += BytesPerCell;
                    if (offset >= _Dump.Length)
                    {
                        row = _DataRows;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Override
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            DrawBackground(e.Graphics);
            if (_Dump != null && _Dump.Length > 0)
            {
                DrawLines(e.Graphics);
            }
            e.Graphics.DrawRectangle(Pens.Gray, new Rectangle(0, 0, Width - 1, Height - 1));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            Resume();

            if (_Dump != null)
            {
                long dl = Math.Max(0, _Lines - _DataRows);

                if (dl < (_Offset / _LineLength))
                {
                    EndEditing();

                    _Offset          = dl * _LineLength;
                    _ScrollBar.Value = (int)dl;
                }
                else
                {
                    _ScrollBar.Value = (int)_Offset / _LineLength;
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (_ScrollBar.Enabled)
            {
                if (e.Delta < 0)
                {
                    if ((_ScrollBar.Maximum - _ScrollBar.Value) > 9)
                    {
                        _ScrollBar.Value++;
                        Scroll();
                    }
                }
                else
                {
                    if (_ScrollBar.Value > 0)
                    {
                        _ScrollBar.Value--;
                        Scroll();
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            EndEditing();

            if (!Focused)
            {
                Focus();
            } 

            Point p = GetCellPos(e.Location);

            if (p.X != -1)
            {
                _EditIndex = GetCellIndex(p);

                if (_EditIndex != -1)
                {
                    p.X = _DataLeft + p.X * _DataCellWidth;
                    p.Y = _HeaderTop + 4 + _CharHeight + p.Y * _CharHeight;

                    _Edit.Text = GetCellValue(_EditIndex).ToString("X"+HexSymbolsPerCell);
                    _Edit.Location = p;
                    _Edit.Show();
                    _Edit.Select();
                }
            }
        }
        #endregion
    }
}