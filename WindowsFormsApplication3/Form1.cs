using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        private int 돌size = 18;
        private int 눈size = 20;
        private int 화점size = 8;
        private Pen pen;
        private Brush wBrush, bBrush;

        private bool turn = false; // false: 흑돌, true: 백돌
        enum STONE { none, black, white };
        STONE[,] 바둑판 = new STONE[19, 19];

        public Form1()
        {
            InitializeComponent();
            pen = new Pen(Color.Black);
            wBrush = new SolidBrush(Color.White);
            bBrush = new SolidBrush(Color.Black);
        }
        protected override void OnPaint(PaintEventArgs e)
        {            
            draw바둑판();
        }

        private void draw바둑판()
        {
            Graphics g = panel1.CreateGraphics();
            for (int x = 0; x < 19; x++)
                g.DrawLine(pen, 10, 10 + x * 눈size, 370, 10 + x * 눈size);
            for (int x = 0; x < 19; x++)
                g.DrawLine(pen, 10 + x * 눈size, 10, 10 + x * 눈size, 10 + 18 * 눈size);
            draw화점(g);
        }
        private void draw화점(Graphics g)
        {
            for(int x=3; x<=15; x+=6)
                for (int y = 3; y <= 15; y += 6)
                {
                    Rectangle r = new Rectangle (10 + 눈size * x - 화점size / 2, 10 + 눈size * y - 화점size / 2, 화점size, 화점size);
                    g.FillEllipse(bBrush, r);
                }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            int x, y;
            x = e.X / 눈size;
            y = e.Y / 눈size;
            if (x < 0 || x >= 19 || y < 0 || y >= 19)
                return;
            draw돌(x, y);
        }

            private void draw돌(int x, int y)
        {
            Graphics g = panel1.CreateGraphics();

            // 이미 어떤 돌이 놓여져 있으면 return
            if (바둑판[x, y] == STONE.black || 바둑판[x, y] == STONE.white)
                return;

            Rectangle r = new Rectangle(10 + 눈size * x - 돌size / 2, 
                 10 + 눈size * y - 돌size / 2, 돌size,  돌size);

            if (turn == false)    // 검은 돌
            {
                g.FillEllipse(bBrush, r);
                바둑판[x, y] = STONE.black;
            }
            else  // 흰돌
            {
                g.FillEllipse(wBrush, r);
                바둑판[x, y] = STONE.white;
            }
            turn = !turn;  // 돌 색깔을 토글
            checkOmok(x, y);  // 오목이 만들어졌는지 체크하는 함수
        }
        private void checkOmok(int x, int y)
        {
            if (checkLR(x, y) >= 5)
                MessageBox.Show(바둑판[x, y] + " wins");
            if (checkUD(x, y) >= 5)
                MessageBox.Show(바둑판[x, y] + " wins");
      
            if (checkSLASH(x, y) >= 5)
                MessageBox.Show(바둑판[x, y] + " wins");
            
            if (checkBACKSLASH(x, y) >= 5)
                MessageBox.Show(바둑판[x, y] + " wins"); 
       
        }

        private int checkLR(int x, int y)
        {
            int cnt = 1;
            for (int i = 1; i < 5; i++)
                if (x+i <= 18 && 바둑판[x + i, y] == 바둑판[x, y])
                    cnt++;
                else
                    break;
            for (int i = 1; i < 5; i++)
                if (x - i >=0 &&바둑판[x - i, y] == 바둑판[x, y])
                    cnt++;
                else
                    break;
            return cnt;
        }
        private int checkUD(int x, int y)
        {
            int cnt = 1;
            for (int i = 1; i < 5; i++)
                if (i + y <= 18 && 바둑판[x, y + i] == 바둑판[x, y])
                    cnt++;
                else
                   break;
            for (int i = 1; i < 5; i++)
                if (y - i >= 0 && 바둑판[x, y - i] == 바둑판[x, y])
                    cnt++;
                else
                    break;

            return cnt;
        }
        
        private int checkSLASH(int x, int y)
        {
            int cnt = 1;
            for (int i = 1; i < 5; i++)
                if (x + i <= 18 && y - i >= 0 && 바둑판[x + i, y - i] == 바둑판[x, y])
                    cnt++;
                else
                    break;
            for (int i = 1; i < 5; i++)
                if (x - i >= 0 && y + i <= 18 && 바둑판[x - i, y + i] == 바둑판[x, y])
                    cnt++;
                else
                    break;
            return cnt;
        }
        private int checkBACKSLASH(int x, int y)
        {
            int cnt = 1;
            for (int i = 1; i < 5; i++)
                if (x - i >= 0 && y - i >= 0 && 바둑판[x - i, y - i] == 바둑판[x, y])
                    cnt++;
                else
                    break;
            for (int i = 1; i < 5; i++)
                if (x + i <= 18 && y + i <= 18 && 바둑판[x + i, y + i] == 바둑판[x, y])
                    cnt++;
                else
                    break;
            return cnt;
        }

        private void 새게임ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < 19; x++)
                for (int y = 0; y < 19; y++)
                    바둑판[x, y] = STONE.none;
            turn = false;
            Refresh();
            draw바둑판();
        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
