# Unity Default Material Shader Extension
Override shader of newly created materials in the Editor. 

Specify a shader in Preferences. Newly created materials will have their shader overridden with this one.
### Limitations: 
Couldn't hook into Unity's initial SaveAndRename call which opens the Material Inspector and file renaming. The shader is only set once the Material asset has been named.
