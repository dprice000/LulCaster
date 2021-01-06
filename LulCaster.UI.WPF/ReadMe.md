# LulCaster.UI.WPF
The goal of this UI project is to be operating system agnostic in the future. This project will only contain the code need to manage the UI.


## Dialogs and Modals
LulCaster implements it's own Dialog windows. Please use these when prompting the user.

## Workers
The application has a number of workers to carry out certain tasks in different threads.

### LulWorkerBase
Implements all of the base code needed to run/stop/reset workers on a seperate thread. The base contains an abstract "DoWork" method that is implemented by 
derived class and contains the bulk of the work to be done by the worker.

## Configuration Controllers/Services
Controllers & Services have been provided to read/write/update presets & regions as the user modifies their configuration. Controllers are the highest level abstraction on requesting this data. 
When making requests from the UI use the controllers as the main interface.

## Static Configuration Folder
The "Presets" folder is set to "Always Copy" inorder to make sure that the deployed environment has a directory to save Preset config files(.json)