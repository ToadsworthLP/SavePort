# SavePort
A port is usually a place to load and unload containers, which is exactly what this Unity library is about: managing data containers in the form of asset-bound variables, be it during gameplay or between sessions.

This project is my implementation of an idea presented during a talk by Schell Games during the GDC. It allows data to be stored and viewed using a reference to the instance of DataContainer<> containing the variable which should be accessed, which, for easier use, can be assigned using the Unity inspector.

It also comes with a system to save the values inside containers to a file, so their values can be restored even after the game session ends, which also makes it a robust and easy-to-use save system, since it can be managed from within the inspector.

For further information, please take a look at the provided example scene.
