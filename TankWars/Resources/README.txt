Following MVC guidelines, we represented the model with our World project, 
inside of which were classes that represented each object that could exist in the game.
We represented the view with our View project,
inside of which was the windows form displaying the client as well as the drawing panel class.
We represented the controller with our GameController project,
inside of which was the Controller class that recieved data from the server, added objects to the
model, recieved inputs from the View, and sent those inputs to the server.

The View created and contained a reference to the Controller, it also contained a reference to the world.
The Controller created and contained a reference to the World.
The Controller created events that used by handlers created in the View, in order to communicate with the View.

The View's DrawingPanel class was responsible for drawing all the visual representations of every object in the game.
It contained a reference to the World which it used to determine which objects existed and should be drawn.

The client communicates wtith the server by sending JSON strings.
The classes representing all objects that can be sent as JSON are located in the World project.

Everything works how it should except for one slight issue with the movement.  It is not as smooth as we wanted and so
while you are moving there is a slight hiccup when changing directions at the same time. 