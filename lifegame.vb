Imports System
Imports System.Linq
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic

public class Game
    inherits Form
    private shared CVSize as Int32 = 512
    private shared CellSize as Int32 = 8
    private cellcolor as SolidBrush = new SolidBrush(Color.FromArgb(255, 200, 44, 85))
    private g as Graphics
    private bm as Bitmap
    private pb as PictureBox
    private alive as HashSet(of Point) = new HashSet(of Point)
    private timer as Timer = new Timer() with { .Interval = 100 }
    private spos as Point = new Point()
    public sub new ()
        me.pb = new PictureBox() with { .Width = Game.CVSize, .Height = Game.CVSize }
        me.bm = new Bitmap(me.pb.Width, me.pb.Height)
        me.g = Graphics.FromImage(bm)
        AddHandler me.pb.MouseDown , AddressOf me.MouseDownEvent
        AddHandler me.pb.MouseUp , AddressOf me.MouseUpEvent
        me.Width = Game.CVSize+16
        me.Height = Game.CVSize+16
        me.Text = "LifeGame"
        me.Controls.Add(me.pb)
        AddHandler me.timer.Tick , AddressOf me.GameUpdate
    end sub
    private sub MouseDownEvent (ByVal sender as object, ByVal e as MouseEventArgs)
        if e.Button = MouseButtons.Left then
            me.timer.Stop()
            dim pos as Point = me.PointToClient(System.Windows.Forms.Cursor.Position)
            me.spos = new Point((pos.X - Game.CellSize / 2) / Game.CellSize, (pos.Y - Game.CellSize / 2) / Game.CellSize)
        end if
        if e.Button = MouseButtons.Right then
            me.timer.Start()
        end if
    end sub
    private sub MouseUpEvent (ByVal sender as object, ByVal e as MouseEventArgs)
        if e.Button = MouseButtons.Left then
            dim pos as Point = me.PointToClient(System.Windows.Forms.Cursor.Position)
            dim p = new Point((pos.X - Game.CellSize / 2) / Game.CellSize, (pos.Y - Game.CellSize / 2) / Game.CellSize)
            dim sx as Int32 = Math.Min(p.X, me.spos.X)
            dim ex as Int32 = Math.Max(p.X, me.spos.X)
            dim sy as Int32 = Math.Min(p.Y, me.spos.Y)
            dim ey as Int32 = Math.Max(p.Y, me.spos.Y)
            dim mode as Boolean = me.alive.Contains(me.spos)
            for py as Int32 = sy to ey
                for px as Int32 = sx to ex
                    if mode then
                        me.alive.Remove(new Point(px, py))
                    else
                        me.alive.Add(new Point(px, py))
                    end if
                next px
            next py
            me.Draw()
        end if
        if e.Button = MouseButtons.Right then
            me.timer.Start()
        end if
    end sub
    private sub Draw()
        me.g.FillRectangle(Brushes.White, 0, 0, Game.CVSize, Game.CVSize)
        for each i as Point in me.alive
            dim px as Int32 = Game.CellSize * i.X
            dim py as Int32 = Game.CellSize * i.Y
            me.g.FillRectangle(Brushes.Black, px, py, Game.CellSize, Game.CellSize)
            me.g.FillRectangle(me.cellcolor, px+1, py+1, Game.CellSize-2, Game.CellSize-2)
        next
        me.pb.Image = me.bm
    end sub
    private sub GameUpdate (ByVal sender as object, ByVal e as EventArgs)
        dim s as Int32 = Game.CVSize / Game.CellSize
        dim m as Dictionary(of Point, Int32) = new Dictionary(of Point, Int32)
        for each i as Point in me.alive
            for dy as Int32 = -1 to 1
                for dx as Int32 = -1 to 1
                    dim t as Point = new Point(i.X+dx, i.Y+dy)
                    if m.ContainsKey(t) then
                        m(t) += 1
                    else
                        m(t) = 1
                    end if
                next dx
            next dy
        next
        for each i as KeyValuePair(Of Point, Int32) in m
            if i.Value = 3 : alive.Add(i.Key) : end if
        next
        for each i as Point in alive.ToList()
            dim a as Boolean = m(i) < 3 or m(i) > 4
            dim x as Boolean = Math.Abs(i.X-s) > s*2
            dim y as Boolean = Math.Abs(i.Y-s) > s*2
            if a or x or y : me.alive.Remove(i) : end if
        next
        me.Draw()
    end sub
end class
class Program
    shared sub main ()
        Application.Run(new Game())
    end sub
end class

