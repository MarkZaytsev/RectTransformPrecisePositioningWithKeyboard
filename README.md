# RectTransformPrecisePositioningWithKeyboard
This is a script for Unity Editor. 
The script allows to precisely position a selected object with RectTransform component in the scene view using keyboard's arrow keys.
I find it useful option for 2d mode workflow with Canvas objects during layouting according to design. But might be worth it to extend the feature for the Transform conmonent to work with 2d sprites.

- Put the script to Editor folder
- Select a game object in the scene
- Script will process events only if game object is selected and SceneView or Inspector is focused. E.g. you might want to click on object first, and then on inspector or scene view.