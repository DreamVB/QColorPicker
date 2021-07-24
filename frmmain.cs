using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace QColorPicker
{
    public partial class frmmain : Form
    {
        public frmmain()
        {
            InitializeComponent();
        }

        private void UpdateRgbBars(Color c)
        {
            hSbRed.Value = c.R;
            hSbGreen.Value = c.G;
            hSbBlue.Value = c.B;
        }

        int getRgb_rnd()
        {
            Random rnd = new Random();

            return rnd.Next(0, 255);
        }

        void CopyColorToClipboard(int index)
        {
            switch (index)
            {
                case 1:
                    txtRgb.SelectAll();
                    txtRgb.Copy();
                    txtRgb.Focus();
                    break;
                case 2:
                    txthtml.SelectAll();
                    txthtml.Copy();
                    txthtml.Focus();
                    break;
                case 3:
                    txtvbhex.SelectAll();
                    txtvbhex.Copy();
                    txtvbhex.Focus();
                    break;
                case 4:
                    txtvbLong.SelectAll();
                    txtvbLong.Copy();
                    txtvbLong.Focus();
                    break;
                case 5:
                    txtDelphiHex.SelectAll();
                    txtDelphiHex.Copy();
                    txtDelphiHex.Focus();
                    break;
                case 6:
                    txtCssRgbA.SelectAll();
                    txtCssRgbA.Copy();
                    txtCssRgbA.Focus();
                    break;
            }
        }

        private void UpdateRgbTextValues()
        {
            txtRed.Text = hSbRed.Value.ToString();
            txtGreen.Text = hSbGreen.Value.ToString();
            txtBlue.Text = hSbBlue.Value.ToString();
            txtRgbAlpha.Text = hSbAlpha.Value.ToString();

            pColor.BackColor = Color.FromArgb(hSbAlpha.Value,hSbRed.Value,
                hSbGreen.Value, hSbBlue.Value);
            UpdateColors();
        }

        bool is_mouse_down = false;
        private bool win_mouse_down = false;
        private Point prev_loc;

        int rgb2long(Color c)
        {
            return c.B * 65536 + c.G * 256 + c.R;
        }

        string color2html(Color c)
        {
            return "#" + c.R.ToString("X2") +
                c.G.ToString("X2") + c.B.ToString("X2");
        }

        Color html2color(string shtml)
        {
            return ColorTranslator.FromHtml(shtml);
        }

        private void UpdateColors(){
            Color c = pColor.BackColor;
            //Get color in hex format
            string sHex = c.R.ToString("X2") +
                c.G.ToString("X2") + c.B.ToString("X2");

            txtRgb.Text = c.R.ToString() +
                "," + c.G.ToString() + "," + c.B.ToString();

            txthtml.Text = "#" + sHex;
            //Visual basic long
            txtvbLong.Text = rgb2long(c).ToString();
            //Visual basic hex
            txtvbhex.Text = "&H" + c.B.ToString("X2") + 
                c.G.ToString("X2") + c.R.ToString("X2");
            //Delphi Hex
            txtDelphiHex.Text = "$00" + c.B.ToString("X2") +
                c.G.ToString("X2") + c.R.ToString("X2");
            //Photoshop hex
            txtCssRgbA.Text = sHex + c.A.ToString("X2");
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pColor.BackColor = Color.Gray;
            UpdateRgbBars(pColor.BackColor);
            UpdateColors();
            //Add name colors

            foreach (KnownColor color in Enum.GetValues(typeof(KnownColor)))
            {
                if (color != KnownColor.Transparent)
                {
                    //Add to combobox
                    cboNameColors.Items.Add(color.ToString());
                }
            }
        }

        private void pPicker_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                is_mouse_down = true;
                pPicker_MouseMove(sender, e);
            }
        }

        private void pPicker_MouseUp(object sender, MouseEventArgs e)
        {
            is_mouse_down = false;
            UpdateColors();
        }

        private void pPicker_MouseMove(object sender, MouseEventArgs e)
        {
            if (is_mouse_down)
            {
                try
                {
                    MouseEventArgs rato = e as MouseEventArgs;
                    Bitmap b = ((Bitmap)pPicker.Image);
                    int x = rato.X * b.Width / pPicker.ClientSize.Width;
                    int y = rato.Y * b.Height / pPicker.ClientSize.Height;
                    Color c = b.GetPixel(x, y);

                    pColor.BackColor = c;

                    UpdateRgbBars(c);
                }
                catch
                {

                }
            }

        }

        private void pFavColors6_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox p = (PictureBox)sender;
                pColor.BackColor = p.BackColor;
                UpdateRgbBars(pColor.BackColor);
                //Update colors
                UpdateColors();
            }
        }

        private void hSbRed_ValueChanged(object sender, EventArgs e)
        {
            UpdateRgbTextValues();
        }

        private void hSbGreen_ValueChanged(object sender, EventArgs e)
        {
            UpdateRgbTextValues();
        }

        private void hSbBlue_ValueChanged(object sender, EventArgs e)
        {
            UpdateRgbTextValues();
        }

        private void hSbAlpha_ValueChanged(object sender, EventArgs e)
        {
            UpdateRgbTextValues();
        }

        private void cmdSavePal_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Title = "Save Pallete";
            sd.Filter = "QPal Files(*.qpf)|*.qpf";
            sd.FilterIndex = 0;

            if (sd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(sd.FileName))
                    {
                        sw.WriteLine(color2html(pFavColors1.BackColor));
                        sw.WriteLine(color2html(pFavColors2.BackColor));
                        sw.WriteLine(color2html(pFavColors3.BackColor));
                        sw.WriteLine(color2html(pFavColors4.BackColor));
                        sw.WriteLine(color2html(pFavColors5.BackColor));
                        sw.WriteLine(color2html(pFavColors6.BackColor));
                        sw.Close();
                    }
                }
                catch
                {

                }
            }
            sd.Dispose();
        }

        private void mnuFavColor1_Click(object sender, EventArgs e)
        {
            pFavColors1.BackColor = pColor.BackColor;
        }

        private void mnuFavColor2_Click(object sender, EventArgs e)
        {
            pFavColors2.BackColor = pColor.BackColor;
        }

        private void mnuFavColor3_Click(object sender, EventArgs e)
        {
            pFavColors3.BackColor = pColor.BackColor;
        }

        private void mnuFavColor4_Click(object sender, EventArgs e)
        {
            pFavColors4.BackColor = pColor.BackColor;
        }

        private void mnuFavColor5_Click(object sender, EventArgs e)
        {
            pFavColors5.BackColor = pColor.BackColor;
        }

        private void mnuFavColor6_Click(object sender, EventArgs e)
        {
            pFavColors6.BackColor = pColor.BackColor;
        }

        private void cmdOpenPal_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog();
            string sLine = string.Empty;
            int idx = 1;

            od.Title = "Open Pallete";
            od.Filter = "QPal Files(*.qpf)|*.qpf";

            if (od.ShowDialog() == DialogResult.OK)
            {
                //Load colors
                using (StreamReader sr = new StreamReader(od.FileName))
                {
                    while (!sr.EndOfStream)
                    {
                        sLine = sr.ReadLine().Trim();
                        if (sLine.Length > 0)
                        {
                            if (idx <= 6)
                            {
                                //Split line into rgb values
                                switch (idx)
                                {
                                    case 1:
                                        pFavColors1.BackColor = html2color(sLine);
                                        break;
                                    case 2:
                                        pFavColors2.BackColor = html2color(sLine);
                                        break;
                                    case 3:
                                        pFavColors3.BackColor = html2color(sLine);
                                        break;
                                    case 4:
                                        pFavColors4.BackColor = html2color(sLine);
                                        break;
                                    case 5:
                                        pFavColors5.BackColor = html2color(sLine);
                                        break;
                                    case 6:
                                        pFavColors6.BackColor = html2color(sLine);
                                        break;
                                }
                            }
                            //Remove #
                            idx++;
                        }
                    }
                    sr.Close();
                }
            }
            od.Dispose();
        }

        private void pPicHolder_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pFavColors1_MouseHover(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;

            toolTip1.Show(color2html(pb.BackColor), pb);
        }

        private void frmmain_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Left){
                win_mouse_down = true;
                this.Cursor = Cursors.SizeAll;
                prev_loc = e.Location;
            }
        }

        private void frmmain_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Default;
            win_mouse_down = false;
        }

        private void frmmain_MouseMove(object sender, MouseEventArgs e)
        {
            if (win_mouse_down)
            {
                this.Location = new Point(
                    (this.Location.X - prev_loc.X) + e.X, (this.Location.Y - prev_loc.Y) + e.Y);

                this.Update();
            }
        }

        private void pPicHolder_MouseDown(object sender, MouseEventArgs e)
        {
            frmmain_MouseDown(sender, e);
        }

        private void pPicHolder_MouseMove(object sender, MouseEventArgs e)
        {
            frmmain_MouseMove(sender, e);
        }

        private void pPicHolder_MouseUp(object sender, MouseEventArgs e)
        {
            frmmain_MouseUp(sender, e);
        }

        private void cpyRgb_Click(object sender, EventArgs e)
        {
            CopyColorToClipboard(1);
        }

        private void cpyHtml_Click(object sender, EventArgs e)
        {
            CopyColorToClipboard(2);
        }

        private void cpyVBHex_Click(object sender, EventArgs e)
        {
            CopyColorToClipboard(3);
        }

        private void cpyPb_Click(object sender, EventArgs e)
        {
            CopyColorToClipboard(4);
        }

        private void cpyDelphiHex_Click(object sender, EventArgs e)
        {
            CopyColorToClipboard(5);
        }

        private void cpyRgbA_Click(object sender, EventArgs e)
        {
            CopyColorToClipboard(6);
        }

        private void cmdClrDialog_Click(object sender, EventArgs e)
        {

            ColorDialog cd = new ColorDialog();

            cd.Color = pColor.BackColor;

            if (cd.ShowDialog() == DialogResult.OK)
            {
                pColor.BackColor = cd.Color;
                //Update scrollbars
                UpdateRgbBars(cd.Color);
                //Update color
                UpdateColors();
            }
        }

        private void cmdInvertColor_Click(object sender, EventArgs e)
        {
            hSbRed.Value = 255 - hSbRed.Value;
            hSbGreen.Value = 255 - hSbGreen.Value;
            hSbBlue.Value = 255 - hSbBlue.Value;
        }

        private void cmdRand1_Click(object sender, EventArgs e)
        {
            hSbRed.Value = getRgb_rnd();
        }

        private void cmdRand2_Click(object sender, EventArgs e)
        {
            hSbGreen.Value = getRgb_rnd();
        }

        private void cmdRand3_Click(object sender, EventArgs e)
        {
            hSbBlue.Value = getRgb_rnd();
        }

        private void cmdRand4_Click(object sender, EventArgs e)
        {
            hSbAlpha.Value = getRgb_rnd();
        }

        private void cboNameColors_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string c_name = (string)cboNameColors.Items[cboNameColors.SelectedIndex];
                Color c = Color.FromName(c_name);
                //Set scrollbars
                pColor.BackColor = c;
                UpdateRgbBars(c);
                UpdateColors();
            }
            catch { }

        }

        private void cmdAbout_Click(object sender, EventArgs e)
        {
            frmabout frm = new frmabout();
            frm.ShowDialog();
        }
    }
}
