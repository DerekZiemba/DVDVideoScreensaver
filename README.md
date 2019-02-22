# DVDVideoScreensaver
Joke for reddit. DVDVideo logo only hits corners and changes colors. 
How to use:
* Click to switch modes between normal mode, always hit opposite corner, and always hit a corner.
  *  Always hit a corner mode is intended to hit all corners by randomly selecting one then making an arc to that corner if it's not directly opposite.   However, I haven't actually bothered to implement it yet so it will do nothing right now. 
* Press Enter to toggle full screen mode. 
* Use arrow keys to change direction
* Use numpad 0-9 to set speed.

![DVD Logo](https://i.imgur.com/C4yFe8I.gif)

![DVD Logo](https://i.imgur.com/8uy0IR0.gif)

# Multiple versions: Winforms and WPF
There is some jankiness with the animation so I figured I'd try it in WPF. Still janky. If anyone knows how to do smooth animations in either of these frameworks let me know. Otherwise I'm considering going to dotnetcore and using OpenGL. 
* Both versions share the same code and resources under [SharedItems](https://github.com/DerekZiemba/DVDVideoScreensaver/tree/master/SharedItems). 
* This makes fo ran interesting resources consumed comparison:
![resource comparison](https://i.imgur.com/3rxEaDI.png)
