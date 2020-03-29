using System.Drawing;
using System.Windows.Forms;

namespace ColorFramework
{

    public class ColorSkinBase
    {
        // A form with custom border and title bar.
        // Some functions, such as resize the window via mouse, are not implemented yet. 
        // Color color = (Color)ColorConverter.ConvertFromString("#FFDFD991");
        //Color colour = ColorTranslator.FromHtml("#E7EFF2");
        // The color and the width of the border.
        private Color borderColor;// = Color.GreenYellow;
        private int borderWidth = 3;
        // The color and region of the header.
        private Color headerColor;// = ColorTranslator.FromHtml("#ff0000");// Color.Red;
        private Rectangle headerRect;
        // The region of the client.
        private Rectangle clientRect;
        // The region of the title text.
        private Rectangle titleRect;
        // The region of the minimum button.
        private Rectangle miniBoxRect;
        // The region of the maximum button.
        private Rectangle maxBoxRect;
        // The region of the close button.
        private Rectangle closeBoxRect;
        // The states of the three header buttons.
        private ButtonState miniState;
        private ButtonState maxState;
        private ButtonState closeState;
        // Store the mouse down point to handle moving the form.
        private int x = 0;
        private int y = 0;
        // The height of the header.
        const int HEADER_HEIGHT = 25;
        // The size of the header buttons.
        private readonly Size BUTTON_BOX_SIZE = new Size(15, 15);
        private System.Windows.Forms.Form _parentForm;
        public ColorSkinBase(System.Windows.Forms.Form ParentForm, Color borderColor, Color headerColor)
        {
            this._parentForm = ParentForm;
            this.borderColor = borderColor;
            this.headerColor = headerColor;
            _parentForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            _parentForm.Paint += new System.Windows.Forms.PaintEventHandler(this.CustomBorderColorForm_Paint);
            _parentForm.Resize += new System.EventHandler(this.CustomBorderColorForm_Resize);
            _parentForm.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CustomBorderColorForm_MouseDown);
            _parentForm.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CustomBorderColorForm_MouseMove);
            _parentForm.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CustomBorderColorForm_MouseUp);
            _parentForm.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CustomBorderColorForm_MouseDoubleClick);
        }
        //private void Form1_Load(System.Object sender, System.EventArgs e)
        //{
        //    // Hide the border and the title bar.
        //    _parentForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        //}
        private void CustomBorderColorForm_Paint(System.Object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // Draw the header.
            using (Brush b = new SolidBrush(borderColor))
            {
                e.Graphics.FillRectangle(b, headerRect);
            }
            // Draw the title text
            using (Brush b = new SolidBrush(headerColor))
            {
                e.Graphics.DrawString(_parentForm.Text, _parentForm.Font, b, titleRect);
            }
            //Draw the header buttons.
            if (_parentForm.MinimizeBox)
                ControlPaint.DrawCaptionButton(e.Graphics, miniBoxRect, CaptionButton.Minimize, miniState);
            if (_parentForm.MinimizeBox)
                ControlPaint.DrawCaptionButton(e.Graphics, maxBoxRect, CaptionButton.Maximize, maxState);
            if (_parentForm.MinimizeBox)
                ControlPaint.DrawCaptionButton(e.Graphics, closeBoxRect, CaptionButton.Close, closeState);
            //Draw the border.
            ControlPaint.DrawBorder(e.Graphics, clientRect, borderColor, borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid);
        }
        // Handle resize to adjust the region ot border, header and so on.
        private void CustomBorderColorForm_Resize(System.Object sender, System.EventArgs e)
        {
            headerRect = new Rectangle(_parentForm.ClientRectangle.Location, new Size(_parentForm.ClientRectangle.Width, HEADER_HEIGHT));
            clientRect = new Rectangle(new Point(_parentForm.ClientRectangle.Location.X, _parentForm.ClientRectangle.Y + HEADER_HEIGHT), new Size(_parentForm.ClientRectangle.Width, _parentForm.ClientRectangle.Height - HEADER_HEIGHT));
            var yOffset = (headerRect.Height + borderWidth - BUTTON_BOX_SIZE.Height) / (double)2;
            titleRect = new Rectangle((int)yOffset, (int)yOffset, _parentForm.ClientRectangle.Width - 3 * (BUTTON_BOX_SIZE.Width + 1) - (int)yOffset, BUTTON_BOX_SIZE.Height);
            miniBoxRect = new Rectangle(_parentForm.ClientRectangle.Width - 3 * (BUTTON_BOX_SIZE.Width + 1), (int)yOffset, BUTTON_BOX_SIZE.Width, BUTTON_BOX_SIZE.Height);
            maxBoxRect = new Rectangle(_parentForm.ClientRectangle.Width - 2 * (BUTTON_BOX_SIZE.Width + 1), (int)yOffset, BUTTON_BOX_SIZE.Width, BUTTON_BOX_SIZE.Height);
            closeBoxRect = new Rectangle(_parentForm.ClientRectangle.Width - 1 * (BUTTON_BOX_SIZE.Width + 1), (int)yOffset, BUTTON_BOX_SIZE.Width, BUTTON_BOX_SIZE.Height);
            _parentForm.Invalidate();
        }
        private void CustomBorderColorForm_MouseDown(System.Object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Start to move the form.
            if ((titleRect.Contains(e.Location)))
            {
                x = e.X;
                y = e.Y;
            }
            // Check and press the header buttons.
            Point mousePos = _parentForm.PointToClient(Control.MousePosition);
            if ((miniBoxRect.Contains(mousePos)))
                miniState = ButtonState.Pushed;
            else if ((maxBoxRect.Contains(mousePos)))
                maxState = ButtonState.Pushed;
            else if ((closeBoxRect.Contains(mousePos)))
                closeState = ButtonState.Pushed;
        }
        private void CustomBorderColorForm_MouseMove(System.Object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Move and refresh.
            if ((x != 0 & y != 0))
            {
                _parentForm.Location = new Point(_parentForm.Left + e.X - x, _parentForm.Top + e.Y - y);
                _parentForm.Refresh();
            }
        }
        private void CustomBorderColorForm_MouseUp(System.Object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Reset the mouse point.
            x = 0;
            y = 0;
            // Check the button states and modify the window state.
            if (miniState == ButtonState.Pushed)
            {
                _parentForm.WindowState = FormWindowState.Minimized;
                miniState = ButtonState.Normal;
            }
            else if (maxState == ButtonState.Pushed)
            {
                if (_parentForm.WindowState == FormWindowState.Normal)
                {
                    _parentForm.WindowState = FormWindowState.Maximized;
                    maxState = ButtonState.Checked;
                }
                else
                {
                    _parentForm.WindowState = FormWindowState.Normal;
                    maxState = ButtonState.Normal;
                }
            }
            else if (closeState == ButtonState.Pushed)
                _parentForm.Close();
        }
        // Handle this event to maxmize/normalize the form via double clicking the title bar.
        private void CustomBorderColorForm_MouseDoubleClick(System.Object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((titleRect.Contains(e.Location)))
            {
                if (_parentForm.WindowState == FormWindowState.Normal)
                {
                    _parentForm.WindowState = FormWindowState.Maximized;
                    maxState = ButtonState.Checked;
                }
                else
                {
                    _parentForm.WindowState = FormWindowState.Normal;
                    maxState = ButtonState.Normal;
                }
            }
        }
    }
}
