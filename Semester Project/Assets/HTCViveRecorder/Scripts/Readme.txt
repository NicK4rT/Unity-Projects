How to use the scripts and how to implement your own RecorderType:

Create a custom Recordertype:
1. Create a new script which inheritated from the interface "IRecorder" (look at the script "TrajectoryRecorder" for example)
2. Go to the RecorderSettings script and add the custom Recordertype class to the list "RecorderActive[] recorderList" in the same manner
3. create a new instance of the RecorderSettings in the project directory --> right-click --> Create --> ViveRecorderSettings
4. Make the settings in the editor window (e.g. Directory = C:\\user\ , sampling frequency = 2.0, etc.)

Using the Datalogger in the Scene for Debugging:
1. Drag & Drop the Datalogger script onto a gameobject in the scene (there's only one possible, otherwise you will get an error)
2. Drag & Drop the created Instance of RecorderSettings into the RecorderSettings object from the datalogger
3. Optional: Drag & Drop a Toggle into the Datalogger to be able to pause the datalogging during runtime
4. Optional: If you want to change between Scenes, you can put the RecorderGlobals into the scene and make sure to keep the RecorderSettings row in the DataLogger empty