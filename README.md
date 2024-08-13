### C# 오목 프로그램
------------------------------------------

### 프로젝트 정보
* 해당 프로젝트 진행 인원 : 2명
* 프로젝트 의도 : 학기 말 프로그램 전시회 제출

### 팀 소개
* 김상호 : 우석대학교 컴퓨터공학과
* 손지승 : 우석대학교 컴퓨터공학과

### 프로젝트 소개
오목이란 바둑 놀이의 하나로 두 사람이 흰 돌과 검은 돌을 가지고 한 번씩 번갈아 가면서 놓다가 외줄로나 모로 다섯 개를 잇따라 먼저 놓는 사람이 게임에서 이긴다는 아주 간단한 규칙을 가지고 있는 게임입니다. 
저희는 실제 오목과 비슷하게 바둑판과 흰 공과 검은 공을 설계할 것이며 서로 번갈아 가면서 바둑알을 놓을 수 있도록 할 것입니다.
외줄로나 모로 다섯 개를 잇따라 놓는 사람이 발생하면 게임이 자동으로 종료되며 승리한 자의 공의 색깔과 함께 승리라고 표시되며 게임이 종료되게 설계했습니다.

### 코드 설명
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

}

여기까지만 설명해 보겠습니다. 우선 변수로 돌size, 눈size, 화점size, pen, wBrush, bBrush를 선언했습니다. 각각 바둑돌의 크기, 눈금의 크기, 화점의 크기, 그리고 검은색 pen, 흰색과 검은색 Brush입니다. 바둑판[19,19]는 돌이 놓여있는 바둑판을 2차원 배열로 표시한 것입니다. 돌이 놓이 곳에 또 돌을 놓지 못하게 하고, 오목이 되었는지 체크하기 위해 필요합니다.

오목은 처음 흑돌부터 시작하여 한번씩 번갈아 돌을 놓기때문에 순번을 정하는 bool 변수로 turn을 설정했습니다. 초기값은 false이고 돌이 한번 놓일 때 마다 true와 false를 토글합니다.

Form1의 생성자에서 pen과 wBrush, bBrush를 만들어줍니다.


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

다음 코딩입니다. OnPaint 메서드에서 바둑판을 그립니다. 이때 그림은 panel1 위에 그려야 하므로 panel1.CreateGraphics()를 사용합니다. this.CreateGraphics()를 쓰면 폼위에 그리므로 panel에 덮여 화면에 보이지 않습니다.

9개의 화점을 그리는 draw화점() 함수는 다음과 같습니다. 화점은 바둑돌보다 작은 원으로 검은색으로 채워줍니다. 


        private void draw화점(Graphics g)
        {
            for(int x=3; x<=15; x+=6)
                for (int y = 3; y <= 15; y += 6)
                {
                    Rectangle r = new Rectangle (10 + 눈size * x - 화점size / 2, 10 + 눈size * y - 화점size / 2, 화점size, 화점size);
                    g.FillEllipse(bBrush, r);
                }
        }

여기까지 코딩하고 실행시켜보면 다음과 같이 바둑판이 나타납니다.  



 

이제 마우스를 클릭하면 해당 위치에 흰돌과 검은돌을 번갈아 표시해줍니다. 그리고 이때 바둑판을 나타내는 2차원 배열에 지금 색깔의 돌을 저장합니다. 주의할 점은 이때의 이벤트 함수는 Form1이 아닌 panel1의 이벤트로 만들어야 합니다. Form1의 이벤트는 panel로 덮여있어 수행되지 않습니다.


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

돌이 놓일 때마다 오목이 만들어졌는지 체크해야 합니다. 이를 위한 함수가 checkOmok()입니다. 어떤 돌이 바둑판에 놓이게 되면 좌우, 상하, 대각선 방향으로 몇개의 같은 색 돌이 연속해서 위치하고 있는지를 체크해서 5개 이상이 연속되어 있으면 메시지 박스에 이겼다고 표시합니다.


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

좌우 방향의 돌이 5개 연속되어 있는지 체크하는 함수는 아래와 같이 만들어집니다. 상하, 대각선방향의 체크도 같은 방법으로 합니다.


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

실행화면의 예를 보겠습니다. 백돌이 이겼군요.

 



메뉴에 대한 코드는 다음과 같습니다.


        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
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

새 게임 메뉴를 누르면 바둑판[,] 배열을 초기화해주고, 흑선으로 만든 후, 화면을 지우고 draw바둑판() 함수로 바둑판을 다시 그려줍니다.

또하나, 지금은 검은색 원, 흰색 원으로 돌을 그려줬는데, 조금 더 이쁘게 하기 위해서는 흑돌, 백돌의 이미지를 가져다가 원 대신 이미지로 그려주면 됩니다. Draw돌() 함수에서 FillEllipse() 부분을 다음과 같이 바꾸면 됩니다. 흑돌, 백돌의 바둑돌 이미지 파일은 인터넷에서 찾아서 썼습니다.  bin/Debug/ 폴더를 기준으로 ../../images 밑에 저장해 두었습니다.

            if (turn == false)    // 검은 돌
            {
                // g.FillEllipse(bBrush, r);
                Bitmap bmp = new Bitmap("../../images/black.png");  
                g.DrawImage(bmp, r);
                바둑판[x, y] = STONE.black;
            }
            else
            {
                //g.FillEllipse(wBrush, r);
                Bitmap bmp = new Bitmap("../../images/white.png");
                g.DrawImage(bmp, r);
                바둑판[x, y] = STONE.white;
            }



