# maze-game

## Development Plan
1. Get Unity installed **(done)**
2. Create something in unity. **(done)**
3. Auto-generate anything in Unity (cubes, spheres, etc). **(done)**
4. Auto-generate anything in Unity with varying dimensions based on index, etc.. **(done)**
5. Auto-generate walls in Unity (house, hallway). **(done)**

### Maze-specific
6. Auto-generate a maze in Unity (simple) (Use binary method?)
7. Auto-generate a tough maze. **(done)**
8. Be able to add in plazas and clearings in the maze **(done)**
9. Be able to add in plazas and clearings in the maze while mending the walls so that paths still exist (add fix option to make pathways one block wide excepting plazas) **(done)**
10. Be able to add in plazas and clearings in the maze while keeping (most) complexity **(done)**
11. Be able to dynamically close off pathways (behind the player eventually) **(done)**
12. Be able to close off pathways and open up others that still lead the same way.

### Rendering-specific
13. Controllable dimming
14. Light flickering
15. Controllable tunnel-vision

## Milestones - {stone}: ({proposed solution})
* Support for head movement: (accelerometer in headset)
* Support for eye movement: (camera in headset)
* Support for heart rate: (finger sensor)
* Support for EEG: we might be able to pick up fear sensations, increased perspiration, amygdala responses, need to do more research
* Vibration in response to in game stuff. Insects on skin, etc.
* Electric shock in response to death or pain
* Virtual reality 
* Support for custom controls (tongue, fingers, toes, etc.)
* Auto-generated mazes(environments)
* Eventual seamless support for dynamic mazes (sections being renovated/removed in real time based on “feedback”)

## Planned Features (definite):
* Actual tunnel vision. 
  * Edges of screen close in to small circles that let the player see the environment. 
  * Tunnel vision in proportion to heart rate.
* Palm sweating (Conduction) for fear response/heights response
* Monster gets closer(faster?) as heart rate increases. 
  * Maybe not have this always be the case as it may get predictable. 
  * But sometimes would be fun.
  * definitely add jumpscares when heart rate is low maybe on a random timer. 
* Maze changes increase in frequency as heart rate increases
* Control methods you have available dictate which modes you can play (quadriplegic mode, missing arm mode, wheelchair mode, etc.)
* Dim the scene as heart rate increases.
* Add flicker to lights (if in world) as HR increases
* Maze changes based on the current fear being tested. 
  * Rising water for thalassophobia. 
* Randomly appearing cliffs or collapsing floor for fear of heights.
  * Little “jump scares” little collapsing floors before full on cliffs
* Claustrophobia fear will make the maze close in
* Thalassophobia - death will have to be by drowning or deep sea creature

## Planned Features (possible):
* Player needs to make sure they don’t hit their surroundings. 
  * If they hit their head ingame, have them fall unconscious or lose hp or something
* HP should be a fear meter, if you black out ( vision tunnels all the way) you die
* Difficulty that relates to heart rate thresholds
  * Easy mode: heart rate >160 for tunnel vision
  * Hard Mode: heart rate > 130 for tunnel vision
  * etc.
* Have teacher NPC die before he finishes teaching you. 
  * Maybe first time thing or depending on pre-game actions. 
  * Maybe depending on difficulty.
* Have in-game memory things. 
 * The dude tells you the combination to a safe with a gun in it. 
 * You have to remember it or else you may die. 
 * Increased difficulty has him forget the combo or die before telling you. 
 * Memory is the first thing to go when scared. I can’t remember shit when I’m scared
* Eye-ball controls (blink to fire). 
  * Eyeball may control where the tunnel vision is (stretch goal)
  > This is actually easier than using an eeg. Is eye movement correlated with fear. Do people make frantic eye movements when frightened? It makes sense. Yes you tend to move your head around and keep your peripherals moving when afraid
* Non-constant health of enemies. 
  * Allows for the player to “finish” off a monster with the last round in the magazine.
* Heart rate “profiles” similar to a treadmill.
* Other “NPCs” that interact with the monster (delusions)
* NPCs that don’t have a physical form, just are disembodied voices in order to mess with the player. Player is “forced” to leave them behind.

## Design Philosophies
* Immersion is important - In world explanations over dialogs and pop-ups
* **Don’t commit to the master branch. Only pull request to it.**
* I’ll figure out how to automatically format code in whatever editor. Having a consistent coding style is important.

## Game Timeline
1. Fear profiling (skippable if not first time)
2. Show randomized images of fear inducing things
3. Fears-implementation
   * Fear of needles
     * Needle moves visibly under skin
     * Healing must be IM then IV then IC(injection into heart)
   * Claustrophobia (enclosed spaces)
     * shrinking walls and ceilings
   * Clowns
     * Monster following you is a clown,random clown faced children in maze
   * Snakes
4. Tutorial (im thinking this should be relatively short or skippable maybe have fear “profiles” for each player for non first timers to jump right in)
5. Rorschach type test for people’s fears to make the monster. Show images and record heart rate in response.
6. Show the player how to heal themselves. 
   * If the fear is of needles then force the healing method to be a syringe. 
   * Could also be re-breaking and re-setting bones. 
   * Insect phobia could be brushing insects off your skin. 
     * Those insects could suck away health.
7. Perhaps depending on how the player treats the tutorial (skipping incessantly) just have the monster burst out and fuck them up. 
   * If the player pays attention to (looks directly at) the tutorial pop-up dialogs, give them more time to prepare for the monster.
   * For non-first-timers: maybe leave an open window in the starting house for them to climb out of and start early? 
   > Idunno just some sort of in world explanation for starting early that isnt a cop out like a literal start early button. good idea, i want to keep immersion as much as possible
8. looking tutorial, move head to look around
9. Movement tutorial, move joystick to move your wheelchair
10. Maybe for harder difficulty have only one hand so no moving and firing 
11. Shooting tutorial
12. Heart rate thresholds based on position in game:
    * Early game (<50% max HR)
    * Mid game (50-70% max HR)
    * End game (70+% max HR)
