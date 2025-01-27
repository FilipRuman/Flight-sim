![Logo](https://github.com/user-attachments/assets/d09c4e9f-e345-48a8-99ec-e3a5b4770168)
# Aircraft physics simulation 
## easly customizable and fun to use while being realistic.
I made this project alone in godot 4.4.
You can use it for your project under the <a href="https://github.com/LeaveMyAlpaca/Flight-sim/blob/main/LICENSE">MIT License</a> 
## Features
- Highly modifiable design using ***Curves***

![image](https://github.com/user-attachments/assets/3080abc0-f63c-44f7-873d-7b3a9ce20204)
- Customizable engine && wind sound synthesis
  
![image](https://github.com/user-attachments/assets/f3e7f2fc-a572-4e58-b178-3107691e39ad)
- Detailed aerodynamic forces debuging

![image](https://github.com/user-attachments/assets/4150a717-f7ae-4a64-a4b1-4cc51c1720a6)
- flaps && control surfaces animations
- 1'st && 3'rd person camera
- works with any 3d model
## Setup
### Just trying out
- download and run exe form releas page
### Edit project
- download <a href="https://godotengine.org/download/archive/4.4-beta1/">godot 4.4 beta1</a>  in .Net version
- setup your godot <a href="https://www.youtube.com/watch?v=Yi1iIM-B7XQ">like in this video </a>
- clone this github repo
- open cloned directory with godot 4.4
# Story
I was thinking about a ***quick*** side project that involved some math and physics.

As a passionate aircraft lover the only answer was to simulate aerodynamics of an airfoil. 

I didn't want to spend too much time, so my goal was just 1 simple box shaped airfoil. 

To step up the difficulty I selected Godot + rust as my framework.
As I've got no prior experience with Godot this could be really difficult.

And It was, at the beginning, I was struggling to get GdNext to work with rust.
After some time fighting with shitty GdNext's docs, I thought that I don't have time for this.

So I went for gooooooood old C#. 
And this was a great choice! As a lot of people said Godot is **really**  great!

And before the end of that day I had a [working airfoil simulation](https://github.com/LeaveMyAlpaca/WingPhysicsTest)!

I had to read a shit load of Wikipedia and other sites with airfoil aerodynamic graphs and other shitty websites form 20 years ago... 
But at the end I had a working simulation. 

Experience was so grate that I thought that I could make a  full plane simulation.

I started from finding a good air plane model on sketch fab that had separate meshes for control surfaces so I could later make animations for them.

Also it had to have a good license so I could use it for my open source project.
[I found this one!](https://sketchfab.com/3d-models/grumman-f4f-wildcat-airplane-ac26b8bf6be44ba7b903ca7fbdedf7e4)! 

It's absolutely great but i had to manually set textures to materials. Idk. if this was a problem with Godot or with that model but, I know that this was painful...

After this I added wings, thrust, input system(Godot's input system is relay WIP. I hope they make it better), colliders etc.
But after this some things felt wrong... But after a couple hours of:
- debugging
- changing things like lift direction, [AoA](https://en.wikipedia.org/wiki/Angle_of_attack) calculation
- increasing physics calculation rate
... Everything  was working great!

But terrain was looking dull so I added procedural terrain from [terrainy](https://github.com/ninetailsrabbit/Terrainy)!

And after some polishing touches like fog, sky box, control surface animations etc. I had a working project!
It took me about 2 days.

But something was missing.......... ***SOUND***
Sound Is one of the most important features of **every flight sim**

It had to be synthesized, because It needed to react to the speed of aircraft, wind direction, AoA etc.
And I didn't know **anything** about sound synthesis.

So I started with simple goal: Just play simple white noise sound.
But somehow I couldn't find anything about it in the GD docs.
I found a repo that was a synthesize demo but it was written in GdShit or something. 
So I asked **AI**,And as we all know I in word AI stands for " I'll hallucinate "...

And after **MORE SEARCHING** I found on GD docs  exactly what I needed. 
 
But how can I go from White noise to air plane engine?
So..... I watched [this video](https://www.youtube.com/watch?v=B_Vc694xk0E&t=279s) and in there guy at the begging makes something sounding relay similar to aircraft engine.
So I've done more or less what this guy was doing until I achieved something sounding ok.
And God thanks GD has sound filters because if I would have to write my owns this would've taken 4 times as long.
With the wind sound I've done [similar thing](https://www.youtube.com/watch?v=XWUDhdiTXF4).
## If you have any questions regarding this project contact me: LeaveMyAlpacaAlone@outlook.com
