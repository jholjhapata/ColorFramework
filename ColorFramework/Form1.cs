using System.Drawing;
using System.Windows.Forms;

namespace ColorFramework
{
    public partial class Form1 : Form
    {
        ColorSkinBase colorSkinBase;
        public Form1()
        {
            InitializeComponent();
            colorSkinBase = new ColorSkinBase(this, Color.GreenYellow, Color.Red);
        }
    }
}
