import System
import System.Linq
import System.Drawing
import System.Windows.Forms
import System.Collections.Generic

class Game(Form):
    private static CVSize = 512
    private static CellSize = 8
    private cellcolor = SolidBrush(Color.FromArgb(255, 200, 44, 85))
    private g as Graphics
    private bm as Bitmap
    private pb as PictureBox
    private alive = HashSet[of Point]()
    private timer as Timer = Timer(Interval : 100)
    private spos = Point()
    def constructor():
        self.pb = PictureBox(Width : Game.CVSize, Height : Game.CVSize)
        self.bm = Bitmap(self.pb.Width, self.pb.Height)
        self.g = Graphics.FromImage(bm)
        self.pb.MouseDown += MouseEventHandler(self.MouseDownEvent)
        self.pb.MouseUp += MouseEventHandler(self.MouseUpEvent)
        self.Width = Game.CVSize+16
        self.Height = Game.CVSize+16
        self.Text = "LifeGame"
        self.Controls.Add(self.pb)
        self.timer.Tick += EventHandler(self.GameUpdate)
    private def MouseDownEvent (sender as object, e as MouseEventArgs):
        if e.Button == MouseButtons.Left:
            self.timer.Stop()
            pos = self.PointToClient(System.Windows.Forms.Cursor.Position)
            self.spos = Point(pos.X / Game.CellSize, pos.Y / Game.CellSize)
        if e.Button == MouseButtons.Right : self.timer.Start()
    private def MouseUpEvent (sender as object, e as MouseEventArgs):
        if e.Button == MouseButtons.Left:
            pos = self.PointToClient(System.Windows.Forms.Cursor.Position)
            p = Point(pos.X / Game.CellSize, pos.Y / Game.CellSize)
            sx = Math.Min(p.X, self.spos.X)
            ex = Math.Max(p.X, self.spos.X)
            sy = Math.Min(p.Y, self.spos.Y)
            ey = Math.Max(p.Y, self.spos.Y)
            mode = self.alive.Contains(self.spos)
            for py in range(sy, ey + 1):
                for px in range(sx, ex + 1):
                    if mode:
                        self.alive.Remove(Point(px, py))
                    else:
                        self.alive.Add(Point(px, py))
            self.Draw()
    private def Draw():
        self.g.FillRectangle(Brushes.White, 0, 0, Game.CVSize, Game.CVSize)
        for i in self.alive:
            px = Game.CellSize * i.X
            py = Game.CellSize * i.Y
            self.g.FillRectangle(Brushes.Black, px, py, Game.CellSize, Game.CellSize)
            self.g.FillRectangle(self.cellcolor, px+1, py+1, Game.CellSize-2, Game.CellSize-2)
        self.pb.Image = self.bm
    private def GameUpdate (sender as object, e as EventArgs):
        s = Game.CVSize / Game.CellSize
        m = Dictionary[of Point, int]()
        for i in self.alive:
            for dy in range(-1, 2):
                for dx in range(-1, 2):
                    t = Point(i.X+dx, i.Y+dy);
                    if m.ContainsKey(t):
                        m[t] += 1
                    else:
                        m[t] = 1
        for i in m:
            if i.Value == 3 : alive.Add(i.Key)
        for i in alive.ToList():
            a = m[i] < 3 or m[i] > 4
            x = Math.Abs(i.X-s) > s*2
            y = Math.Abs(i.Y-s) > s*2
            if a or x or y : self.alive.Remove(i)
        self.Draw()
Application.Run(Game())
