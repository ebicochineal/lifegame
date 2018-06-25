#! /usr/bin/env python3
import tkinter

cellcolor = '#c82c55'
d = [(x, y) for x in [-1, 0, 1] for y in [-1, 0, 1]]
alive = set()
rid = None
spos = (0, 0)
def draw():
    cv.delete('cells')
    for x, y in alive:
        rx, ry= x*8+2, y*8+2
        cv.create_rectangle(rx, ry, rx+7, ry+7, fill = cellcolor, tag = 'cells')
def leftdown(e):#
    global spos
    if rid : root.after_cancel(rid)
    k = ((e.x-2) // 8, (e.y-2) // 8)
    spos = k
def leftup(e):
    k = ((e.x-2) // 8, (e.y-2) // 8)
    s = spos
    sx, ex = min(s[0], k[0]), max(s[0], k[0])
    sy, ey = min(s[1], k[1]), max(s[1], k[1])
    mode = (s[0], s[1]) in alive
    for x in range(sx, ex + 1):
        for y in range(sy, ey + 1):
            if mode:
                if (x, y) in alive : alive.remove((x, y))
            else:
                alive.add((x, y))
    draw()
def rightdown(e):
    if rid : root.after_cancel(rid)
    update()
def update():
    global rid
    m = dict()
    for sx, sy in alive:
        for dx, dy in d:
            t = (sx+dx, sy+dy)
            if t in m:
                m[t] += 1
            else:
                m[t] = 1
    for k, v in m.items():
        if v == 3 : alive.add(k)
    for i in list(alive):
        a = m[i] < 3 or m[i] > 4
        x = abs(i[0]-32) > 64
        y = abs(i[1]-32) > 64
        if a or x or y : alive.remove(i)
    draw()
    rid = root.after(100, update)
root = tkinter.Tk()
cv = tkinter.Canvas(root, width = 512, height = 512)
cv.pack()
root.bind('<Button-1>', leftdown)
root.bind('<ButtonRelease-1>', leftup)
root.bind('<Button-3>', rightdown)
root.mainloop()
