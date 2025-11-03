# Snake Code Demo
This is a small demo of a snake game for a job interview. The purpose is to show off different coding design choices that I think is important when creating a game in Unity. The point is not to make a mindblowing game. When I say important, I mean both in terms of making it easier to keep the game performant, but also make the code versatile to change.

## Design points in this demonstration

### Separate data and logic from presentation
To me, it is good practice to separate the logic and data from the presentation. In Unity "presentation" is primarily handled by MonoBehaviours, and oftentimes you see that they also contain all the data and logic of the game. This usually also means that the behaviours reference each other in an intricate network that needs to be setup correctly in the scene in order to get things done.

In this demo there is a main **GameData** class that will hold instances of all the active classes that control different aspects of the game: Snake, GameArea, Food and FoodArea. The MonoBehaviours of the game, that I have postfixed with View, will get initialized with the GameData, and pick up the specific parts it needs to function. In this way these subparts work as the conduit between the behaviours instead of having direct references between the behaviours and use that. 

The biggest example of this in the game is probably the Snake where the SnakeView "ticks" the snake to move it and then show it. The game area view will then separately check if the snake is inside the game area and kill it if it is not.

### Avoid singleton manager classes
A related design point is that the game has no singleton manager classes that contain all the logic for the game. This is a common pattern in Unity games, and oftentimes ends up with having one class that needs to get modified whenever anything changes because it has too much responsibility.

In this demo I prefer to loosely couple a lot of different Views that can then be added and removed interdependently because they have their own responsibilities.

### Use SciptableObjects to create the data classes
In order to give some different game modes, there are a handful of different scriptable objects that are used to create the data classes when the game starts. These are selected from the main menu. This decouples some of the settings from what is in the scene.

### Events instead of polling
Another common pattern in Unity games is that you have different behaviours that in their Update methods look at their data every frame and present it to keep things updated. I much prefer having signals for when data is changed and then do the update. In this demo this is done with regular C# events, but it could also have been done in other ways.

The point is that it keeps the amount of Update methods down so it is easier to control script performance. Not shown much in this demo, but some UI implementations also suffer with performance if they are changed every frame because there are cascading changes.

### Use asynchronous code to control distributed animations
When the game ends, there is a small animation happening where the snake disappears and the remaining food despawns. The settings for these animations are on the views that starts them so they can be tweaked outside of code. It is the **GameData** class that holds the functionality to hook up the desire to present something when the game ends. It is used by the **SnakeView** and the **FoodAreaView**. I am using the third party library **UniTask** for doing this instead of using Coroutines, as I think the async/await syntax is easier to understand.
