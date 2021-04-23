CLIENT SIDE CODE

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

-----------------------------------------------------------------------------------------------------------------------------------

SERVER SIDE CODE

Most of our server side code is located in the ServerController class. Our Server class is responsible only for starting the server,
calling the ServerController's update methods, parsing the settings file, and printing out connection messages to the console. The 
ServerController is responsible for connecting to new clients, adding objects to the World, sending updated world data to clients, 
and checking collisons and disconnects. Most of the movement logic for our tanks and projectiles are located in the Tank and
Projectile classes respectively, along with variables and methods to control fire rate and fire type.

All of the settings parsed from the settings.xml file are stored as varibales in our Constants class.

In order to store the client connections, our server makes use of 2 dictionaries. One that maps a client's SocketState to
their client ID, and another that maps the client ID back to the SocketState of that player. We did this so that we could 
both determine which tank was associated with a specific connection and determine which connection was linked to a specific
disconnected tank.

The new gamemode we implementeed increases the max health for all tanks to 6 and makes it so that powerups fully restore 
health in addition to allowing the player to fire a beam. In order to access this gamemode, the value of Mode in the 
settings file must be changed from "Default" to "Power".