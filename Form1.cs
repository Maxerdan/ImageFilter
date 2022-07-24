namespace PicturePixelFilter
{
    public partial class Form1 : Form
    {
        private int _sellSize = 20;
        private Bitmap _originImage;
        private Bitmap _imageFiltered;
        private Dictionary<string, List<Sell>> _dict;

        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) // choose picture
            {
                using (var fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    _originImage = new Bitmap(fs);
                    pictureBox1.Image = _originImage;
                    Transform();
                }
            }
            else
                return;
        }

        private void Transform()
        {
            _dict = new Dictionary<string, List<Sell>>();
            _imageFiltered = new Bitmap(_originImage);
            for (var h = 0; h < _originImage.Height; h++)
            {
                for (var w = 0; w < _originImage.Width; w++)
                {

                    var key = Encode(w / _sellSize, h / _sellSize);
                    if (w / _sellSize % 2 == 0 && h / _sellSize % 2 == 0)
                        if (_dict.ContainsKey(key))
                            _dict[key].Add(new Sell(w, h, _originImage.GetPixel(w, h)));
                        else
                            _dict.Add(key, new List<Sell> { new(w, h, _originImage.GetPixel(w, h)) });
                }
            }
            foreach (var keyValuePair in _dict)
            {
                int a = 0;
                int r = 0;
                int g = 0;
                int b = 0;
                foreach (var sell in keyValuePair.Value)
                {
                    a += sell.Color.A;
                    r += sell.Color.R;
                    g += sell.Color.G;
                    b += sell.Color.B;
                }
                a /= keyValuePair.Value.Count();
                r /= keyValuePair.Value.Count();
                g /= keyValuePair.Value.Count();
                b /= keyValuePair.Value.Count();

                foreach (var sell in keyValuePair.Value)
                    _imageFiltered.SetPixel(sell.X, sell.Y, Color.FromArgb(a, r, g, b));
            }
            DrawBlackLines();

            pictureBox2.Image = _imageFiltered;
        }

        private string Encode(int x, int y) => $"{x}:{y}";
        private int DecodeX(string pairXY) => int.Parse(pairXY.Split(':')[0]);
        private int DecodeY(string pairXY) => int.Parse(pairXY.Split(':')[1]);

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (int.TryParse(toolStripTextBox1.Text, out _sellSize))
                    Transform();
                else
                    MessageBox.Show("Not integer inputed value", "Error", MessageBoxButtons.OK);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            DrawBlackLines();
        }

        private void DrawBlackLines()
        {
            if (checkBox1.Checked)
            {
                SetStaticColorForGrid(Color.Black);
            }
            else
                SetColorForGridFrom();
            pictureBox2.Image = _imageFiltered;

            void SetColorForGridFrom()
            {
                for (var h = 0; h < _originImage.Height; h++)
                    for (var w = 0; w < _originImage.Width; w++)
                        if (h % _sellSize == 0 || w % _sellSize == 0)
                            _imageFiltered.SetPixel(w, h, _originImage.GetPixel(w, h));
            }

            void SetStaticColorForGrid(Color color)
            {
                for (var h = 0; h < _originImage.Height; h++)
                    for (var w = 0; w < _originImage.Width; w++)
                        if (h % _sellSize == 0 || w % _sellSize == 0)
                            _imageFiltered.SetPixel(w, h, color);
            }
        }
    }
}