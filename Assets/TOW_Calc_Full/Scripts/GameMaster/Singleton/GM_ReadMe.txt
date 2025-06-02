#### General Info ####
* The GM script is a Singleton that is meant to hold references to scripts and objects that need to be accessed from multiple places in the game
* The GM is usually used in conjunction with Managers
* Usually the GM is it's own GameObject in the scene with Managers as it's children
* The GM is created an destroyed with scenes, meaning that it is not persistent between scenes


IMPORTANT: The GM must be moved above/before standard execution order!

Execution order is recorded in the .meta file, thus this needs to be part of the code repository.